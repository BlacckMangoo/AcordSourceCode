
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

	public PlayerData Data;

	#region COMPONENTS
	public Rigidbody2D RB { get; private set; }
	public PhysicsMaterial2D normal;
	public PlayerAttack playerAttack { get; private set; }
	public PlayerAnimator animHandler { get; private set; }
	#endregion

	#region STATE PARAMETERS

	public bool isLocked;
	public bool IsFacingRight { get; private set; }
	public bool IsJumping { get; private set; }
	public bool IsWallJumping { get; private set; }
	public bool IsDashing { get; private set; }
	public bool IsSliding { get; private set; }
	public bool punching { get; private set; }
	//Timers (also all fields, could be private and a method returning a bool could be used)
	public float LastOnGroundTime { get; private set; }
	public float LastOnWallTime { get; private set; }
	public float LastOnWallRightTime { get; private set; }
	public float LastOnWallLeftTime { get; private set; }


	//Jump
	private bool _isJumpCut;
	private bool _isJumpFalling;
    [SerializeField] private float minJumpForce = 5f;
    [SerializeField] private float maxJumpForce = 10f;
    [SerializeField] private float jumpForceDecayRate = 0.5f;
    private float currentJumpForce;

    //Wall Jump
    private float _wallJumpStartTime;
	private int _lastWallJumpDir;

	//Dash
	private int _dashesLeft;
	private bool _dashRefilling;
	private Vector2 _lastDashDir;
	private bool _isDashAttacking;

	

	//Roll
	public bool isRolling;
	[SerializeField] private float rollSpeedMax;
	float rolltime;
	float xinput;
	//Attack
	bool shoudPushBack;
	#endregion

	#region INPUT PARAMETERS
	public Vector2 _moveInput;

	public float LastPressedJumpTime { get; private set; }
	public float LastPressedDashTime { get; private set; }
	#endregion

	#region CHECK PARAMETERS
	
	[Header("Checks")]
	[SerializeField] private Transform _groundCheckPoint;
	//Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
	[SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
	[Space(5)]
	[SerializeField] private Transform _frontWallCheckPoint;
	[SerializeField] private Transform _backWallCheckPoint;
	[SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);
	#endregion

	#region LAYERS & TAGS
	[Header("Layers & Tags")]
	[SerializeField] private LayerMask _groundLayer;
	#endregion

	[SerializeField] SpriteRenderer gun;

	private void Awake()
	{
		RB = GetComponent<Rigidbody2D>();
		animHandler = GetComponentInChildren<PlayerAnimator>();
	}

	private void Start()
	{
		rolltime = Data.Rolltime;
		SetGravityScale(Data.gravityScale);
		IsFacingRight = true;
	}

	private void Update()
	{
	
		#region TIMERS
		LastOnGroundTime -= Time.deltaTime;
		LastOnWallTime -= Time.deltaTime;
		LastOnWallRightTime -= Time.deltaTime;
		LastOnWallLeftTime -= Time.deltaTime;

		LastPressedJumpTime -= Time.deltaTime;
		LastPressedDashTime -= Time.deltaTime;


        #endregion
         xinput = Input.GetAxisRaw("Horizontal");

        #region INPUT HANDLER
        if (!isLocked)
		{
			_moveInput.x = xinput ;
        }
		else
		{
			_moveInput.x = 0;
		}

        if (Input.GetKeyDown(KeyCode.L))
        {
            isLocked = !isLocked;
        }

		// x input controls turning 
		//_moveinput.x controls movement 

        if (xinput != 0)
			CheckDirectionToFace(xinput > 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpInput();
        }

        if (Input.GetKey(KeyCode.Space) && IsJumping )
        {
            currentJumpForce = Mathf.Max(currentJumpForce - jumpForceDecayRate * Time.deltaTime, minJumpForce);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            OnJumpUpInput();
        }

        if (Input.GetKey(KeyCode.D) && _moveInput.x != 0 && !IsDashing  )
		{

			animHandler.isRolling = true;
			isRolling = true;
			//Data.runMaxSpeed = rollSpeedMax;
			rolltime -= Time.deltaTime;

			if(rolltime < 0)
			{
				isRolling = false;
                animHandler.isRolling = false;
                isRolling = false;
               
            }
		}
		else if (Input.GetKeyUp(KeyCode.D) || _moveInput.x == 0)
		{
			animHandler.isRolling = false;
			isRolling = false;
			rolltime = Data.Rolltime;
		}

		

		if (Input.GetMouseButtonDown(1))
		{
			OnDashInput();
		}
        #endregion
        if (shoudPushBack)
        {
            Invoke("PushBack", 0.15f);
            shoudPushBack = false;
        }

		
		
		
		#region COLLISION CHECKS
		if (!IsDashing && !IsJumping)
		{
				
			//Ground Check
			if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer) || Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer)) //checks if set box overlaps with ground
			{
				if (LastOnGroundTime < -0.2f)
				{
					animHandler.justLanded = true;

				}
				animHandler.isGrounded = true;
				LastOnGroundTime = Data.coyoteTime; //if so sets the lastGrounded to coyoteTime
			}
			else
			{
				animHandler.isGrounded = false;
			}

			//Right Wall Check
			if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer)  && IsFacingRight)
					|| (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer)  && !IsFacingRight)) && !IsWallJumping)
			{
				LastOnWallRightTime = Data.coyoteTime;
				
				

			}

			

			//Right Wall Check	
			if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)
				|| (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)) && !IsWallJumping)
			{
				animHandler.isWallTouching = true;
				LastOnWallLeftTime = Data.coyoteTime;
				
				
			}

			//Two checks needed for both left and right walls since whenever the play turns the wall checkPoints swap sides
			LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);

			if (LastOnWallTime > 0 && !IsWallJumping && _moveInput.x != 0)
			{
				animHandler.isWallTouching = true;
			}
			else
			{
				animHandler.isWallTouching = false;
			}

		}
		#endregion

		#region JUMP CHECKS
		if (RB.velocity.y < 0 && !IsWallJumping && !IsDashing)
		{
			IsJumping = false;

			_isJumpFalling = true;
		}

		if (IsWallJumping && Time.time - _wallJumpStartTime > Data.wallJumpTime)
		{
			IsWallJumping = false;
		}

		if (LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
		{
			_isJumpCut = false;

			_isJumpFalling = false;
		}

		if (!IsDashing)
		{
			//Jump
			if (CanJump() && LastPressedJumpTime > 0)
			{
				IsJumping = true;
				IsWallJumping = false;
				_isJumpCut = false;
				_isJumpFalling = false;
				Jump(Data.jumpForce);


			}
			//WALL JUMP
			else if (CanWallJump() && LastPressedJumpTime > 0)
			{
				IsWallJumping = true;
				IsJumping = false;
				_isJumpCut = false;
				_isJumpFalling = false;
				animHandler.startedJumping = true;
				animHandler.isGrounded = false;
				_wallJumpStartTime = Time.time;
				_lastWallJumpDir = (LastOnWallRightTime > 0) ? -1 : 1;

				WallJump(_lastWallJumpDir);

			}
            

            if (_isJumpFalling)
			{
				animHandler.isfalling = true;
			}
			else
			{
				animHandler.isfalling = false;
			}

			
		}
		#endregion

		#region DASH CHECKS
		if (CanDash() && LastPressedDashTime > 0)
		{
			//Freeze game for split second. Adds juiciness and a bit of forgiveness over directional input
			Sleep(Data.dashSleepTime);


			_lastDashDir = IsFacingRight ? Vector2.right : Vector2.left;



			IsDashing = true;
			IsJumping = false;
			IsWallJumping = false;
			_isJumpCut = false;

			StartCoroutine(nameof(StartDash), _lastDashDir);



        }
		#endregion

		

		#region GRAVITY
		if (!_isDashAttacking)
		{
			//Higher gravity if we've released the jump input or are falling
			if (IsSliding)
			{
				SetGravityScale(Data.slidegravityScale);
			}

			else if (RB.velocity.y < 0 && _moveInput.y < 0)
			{
				//Much higher gravity if holding down
				SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);
				//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
				RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFastFallSpeed));
			}

			else if (_isJumpCut)
			{
				//Higher gravity if jump button released
				SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
				RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
			}
			else if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
			{
				SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
			}
			else if (RB.velocity.y < 0)
			{
				//Higher gravity if falling
				SetGravityScale(Data.gravityScale * Data.fallGravityMult);
				//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
				RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
			}
			else
			{
				//Default gravity if standing on a platform or moving upwards
				SetGravityScale(Data.gravityScale);
			} 
		}
		else
		{
			SetGravityScale(0);
		}
		
		#endregion

		
    }

   

    private void FixedUpdate()
	{
		//Handle Run
		if (!IsDashing)
		{
			if (IsWallJumping)
				Run(Data.wallJumpRunLerp);
			else
				Run(1);
		}
		else if (_isDashAttacking)
		{
			Run(Data.dashEndRunLerp);
		}

		//Handle Slide
		if (IsSliding)
			Slide();

    

    }

	#region INPUT CALLBACKS
	//Methods which whandle input detected in Update()
	public void OnJumpInput()
	{
		LastPressedJumpTime = Data.jumpInputBufferTime;
	}

	public void OnJumpUpInput()
	{
        if (CanJumpCut() || CanWallJumpCut())
        {
            _isJumpCut = true;
            if (RB.velocity.y > 0)
            {
                RB.velocity = new Vector2(RB.velocity.x, RB.velocity.y * 0.5f);
            }
        }
    }

	public void OnDashInput()
	{
		LastPressedDashTime = Data.dashInputBufferTime;
	}
	#endregion

	#region GENERAL METHODS
	public void SetGravityScale(float scale)
	{
		RB.gravityScale = scale;
	}

	private void Sleep(float duration)
	{

		StartCoroutine(nameof(PerformSleep), duration);
	}

	private IEnumerator PerformSleep(float duration)
	{
		Time.timeScale = 0;
		yield return new WaitForSecondsRealtime(duration); //Must be Realtime since timeScale with be 0 
		Time.timeScale = 1;
	}
	#endregion

	//MOVEMENT METHODS
	#region RUN METHODS
	private void Run(float lerpAmount)
	{
		//Calculate the direction we want to move in and our desired velocity
		float targetSpeed = _moveInput.x * Data.runMaxSpeed;
		//We can reduce are control using Lerp() this smooths changes to are direction and speed
		targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

		if (isRolling)
		{
			targetSpeed = Data.rollSpeed * _moveInput.x;
		}
		else
		{
			targetSpeed = Data.runMaxSpeed * _moveInput.x;
		}
		#region Calculate AccelRate
		float accelRate;

		//Gets an acceleration value based on if we are accelerating (includes turning) 
		//or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
		if (LastOnGroundTime > 0)
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
		else
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
		#endregion

		#region Add Bonus Jump Apex Acceleration
		//Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
		if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
		{
			accelRate *= Data.jumpHangAccelerationMult;
			targetSpeed *= Data.jumpHangMaxSpeedMult;
		}
		#endregion

		#region Conserve Momentum
		//We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
		if (Data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
		{
			//Prevent any deceleration from happening, or in other words conserve are current momentum
			//You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
			accelRate = 0;
		}
		#endregion

		//Calculate difference between current velocity and desired velocity
		float speedDif = targetSpeed - RB.velocity.x;
		//Calculate force along x-axis to apply to thr player

		float movement = speedDif * accelRate;

		//Convert this to a vector and apply to rigidbody
		RB.AddForce(movement * Vector2.right, ForceMode2D.Force);




	}

	private void Turn()
	{

		transform.Rotate(Vector3.up, 180f);

		IsFacingRight = !IsFacingRight;
	}
	#endregion

	#region JUMP METHODS
	public void Jump(float force)
	{

        //Ensures we can't call Jump multiple times from one press
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;

        #region Perform Jump
        currentJumpForce = maxJumpForce;
        float jumpForce = currentJumpForce;

        if (RB.velocity.y < 0)
            jumpForce -= RB.velocity.y;

        RB.velocity = new Vector2(RB.velocity.x, 0f); // Reset vertical velocity
        RB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        IsJumping = true;
        animHandler.startedJumping = true;
        animHandler.isGrounded = false;
        #endregion
    }

    private void WallJump(int dir)
	{
		//Ensures we can't call Wall Jump multiple times from one press
		LastPressedJumpTime = 0;
		LastOnGroundTime = 0;
		LastOnWallRightTime = 0;
		LastOnWallLeftTime = 0;

		#region Perform Wall Jump
		Vector2 force = new Vector2(Data.wallJumpForce.x, Data.wallJumpForce.y);
		force.x *= dir; //apply force in opposite direction of wall

		if (Mathf.Sign(RB.velocity.x) != Mathf.Sign(force.x))
			force.x -= RB.velocity.x;
		
		if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
				{
			force.x = 3f;

		}
		
		if (RB.velocity.y < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
			force.y -= RB.velocity.y;

		//Unlike in the run we want to use the Impulse mode.
		//The default mode will apply are force instantly ignoring masss
		RB.AddForce(force, ForceMode2D.Impulse);
		#endregion
	}
	

	#region DASH METHODS
	//Dash Coroutine
	private IEnumerator StartDash(Vector2 dir)
	{
		//Overall this method of dashing aims to mimic Celeste, if you're looking for
		// a more physics-based approach try a method similar to that used in the jump

		LastOnGroundTime = 0;
		LastPressedDashTime = 0;

		float startTime = Time.time;

		_dashesLeft--;
		_isDashAttacking = true;

		SetGravityScale(0);

		//We keep the player's velocity at the dash speed during the "attack" phase (in celeste the first 0.15s)
		while (Time.time - startTime <= Data.dashAttackTime)
		{
			RB.velocity = dir.normalized * Data.dashSpeed;
			//Pauses the loop until the next frame, creating something of a Update loop. 

			yield return null;
		}

		startTime = Time.time;

		_isDashAttacking = false;

		//Begins the "end" of our dash where we return some control to the player but still limit run acceleration (see Update() and Run())
		SetGravityScale(Data.gravityScale);
		RB.velocity = Data.dashEndSpeed * dir.normalized;

		while (Time.time - startTime <= Data.dashEndTime)
		{
			yield return null;
		}

		//Dash over
		IsDashing = false;
	}

	//Short period before the player is able to dash again
	private IEnumerator RefillDash(int amount)
	{
		//SHoet cooldown, so we can't constantly dash along the ground, again this is the implementation in Celeste, feel free to change it up
		_dashRefilling = true;
		yield return new WaitForSeconds(Data.dashRefillTime);
		_dashRefilling = false;
		_dashesLeft = Mathf.Min(Data.dashAmount, _dashesLeft + 1);
	}
	private void PushBack()
	{
		if(IsFacingRight)
		{
			RB.AddForce(new Vector2(-Data.pushback_x,Data.pushback_y),ForceMode2D.Impulse);
		}
		else
		{
			RB.AddForce(new Vector2 (Data.pushback_x, Data.pushback_y),ForceMode2D.Impulse);
		}
	}
	
	#endregion

	#region OTHER MOVEMENT METHODS
	private void Slide()
	{
		//We remove the remaining upwards Impulse to prevent upwards sliding
		if (RB.velocity.y > 0)
		{
			RB.AddForce(-RB.velocity.y * Vector2.up, ForceMode2D.Impulse);
		}


	
		float speedDif = Data.slideSpeed - RB.velocity.y;
		float movement = speedDif * Data.slideAccel;
		//So, we clamp the movement here to prevent any over corrections (these aren't noticeable in the Run)
		//The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called. For more info research how force are applied to rigidbodies.
		movement = Mathf.Clamp(movement,-Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

		//RB.AddForce(movement * Vector2.up);
	
	}
	#endregion




	#region CHECK METHODS
	public void CheckDirectionToFace(bool isMovingRight)
	{
		if (isMovingRight != IsFacingRight)
			Turn();
	}

	private bool CanJump()
	{
		return LastOnGroundTime > 0 && !IsJumping;
	}

	private bool CanWallJump()
	{
		return LastPressedJumpTime > 0 && LastOnWallTime > 0 && LastOnGroundTime <= 0 && (!IsWallJumping ||
			 (LastOnWallRightTime > 0 && _lastWallJumpDir == 1) || (LastOnWallLeftTime > 0 && _lastWallJumpDir == -1));
	}

	private bool CanJumpCut()
	{
		return IsJumping && RB.velocity.y > 0;
	}

	private bool CanWallJumpCut()
	{
		return IsWallJumping && RB.velocity.y > 0;
	}

	public bool CanDash()
	{
		if (!IsDashing && _dashesLeft < Data.dashAmount && LastOnGroundTime > 0 && !_dashRefilling)
		{
			StartCoroutine(nameof(RefillDash), 1);
		}

		return _dashesLeft > 0;
	}

	public bool CanSlide()
	{
		if (LastOnWallTime > 0 && !IsJumping && !IsWallJumping && !IsDashing && LastOnGroundTime <= 0)
			return true;
		else
			return false;
	}
	#endregion

	
    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize);
		Gizmos.DrawWireCube(_backWallCheckPoint.position, _wallCheckSize);
		
	}
	#endregion

//HandleDeath

	public void PlayerDie()
	{
		animHandler.isDead = true;
   
    }

	public bool isStanding()
	{
		return !IsJumping && !IsWallJumping && _moveInput.x == 0 && !isRolling && !IsDashing;
	}

	
	
    
}





#endregion
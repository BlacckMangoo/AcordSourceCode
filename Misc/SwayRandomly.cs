using System.Collections;
using UnityEngine;


public class SwayRandomly : MonoBehaviour
{
    public float maxOffset = 0.5f;
    public float swaySpeed = 1f;
    public float transitionDuration = 2f;

    private Vector3 _startLocalPosition;
    private Vector2 _currentRandomOffset;
    private Vector2 _targetRandomOffset;
    private float _timeOffset;
    private float _transitionTimer;

    void Start()
    {
        _startLocalPosition = transform.localPosition;
        _timeOffset = Random.value * 1000f;
        _currentRandomOffset = _targetRandomOffset = Random.insideUnitCircle;
        InvokeRepeating(nameof(UpdateRandomOffset), 0f, transitionDuration);
    }

    void LateUpdate()
    {
        ApplySway();
    }

    private void UpdateRandomOffset()
    {
        _currentRandomOffset = _targetRandomOffset;
        _targetRandomOffset = Random.insideUnitCircle;
        _transitionTimer = 0f;
    }

    private void ApplySway()
    {
        _transitionTimer += Time.deltaTime;
        float t = Mathf.Clamp01(_transitionTimer / transitionDuration);
        Vector2 lerpedOffset = Vector2.Lerp(_currentRandomOffset, _targetRandomOffset, t);

        float xOffset = Mathf.PerlinNoise(_timeOffset + Time.time * swaySpeed, 0) * 2 - 1;
        float yOffset = Mathf.PerlinNoise(0, _timeOffset + Time.time * swaySpeed) * 2 - 1;

        Vector3 swayOffset = new Vector3(
            xOffset * maxOffset * lerpedOffset.x,
            yOffset * maxOffset * lerpedOffset.y,
            0
        );

        // Apply sway on top of current local position
        transform.localPosition = _startLocalPosition + swayOffset;
    }



}

  

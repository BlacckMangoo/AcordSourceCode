using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] Sprite[] bulletTypeUiElement;
    [SerializeField] GameObject currentBulletTypeUiSlot;
    Image bulletImage;
    int i = 0;

    [SerializeField] GameObject standardBulletprefab;
    [SerializeField] GameObject lobberBulletprefab;
    [SerializeField] GameObject richochetBulletprefab;

    [SerializeField] AudioClip weaponSwitchSFX; 


    // Start is called before the first frame update
    void Start()
    {
       bulletImage =  currentBulletTypeUiSlot.GetComponent<Image>() ;
      
    }

    public enum BulletType
    {
        Standard,
        Lobber,
        Richochet
    }

     public BulletType currentBulletType; 
    public Dictionary<BulletType, GameObject> bulletPrefabs;

    private void Awake()
    {
        bulletPrefabs = new Dictionary<BulletType, GameObject>
        {
            { BulletType.Standard, standardBulletprefab },
            { BulletType.Lobber, lobberBulletprefab },
            { BulletType.Richochet, richochetBulletprefab }  
            
        };
    }

    // Update is called once per frame
    void Update()
    {
        ChangeBulletType();
    }

    void ChangeBulletType()
    {

        
        if (Input.GetKeyDown(KeyCode.V))
            {
            SoundManager.instance.PlaySoundFX(0.5f, weaponSwitchSFX, this.transform);
            //changes the shooting behaviour
            currentBulletType = (BulletType)(((int)currentBulletType + 1) % System.Enum.GetValues(typeof(BulletType)).Length);
            //changes the Ui
            i = (i + 1) % bulletTypeUiElement.Length;

                
                bulletImage.sprite = bulletTypeUiElement[i];
            }

    }
 }


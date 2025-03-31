using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour 
{
    public static  DataManager Instance { get; set; }
    
    [SerializeField] TextMeshPro moneyText;
    [SerializeField] int currentMoney;
 

    private void Update()
    {
        moneyText.text = currentMoney.ToString();
      
    }
      
    public void AddMoney(int money)
    {
        currentMoney += money;
    }

    public void RemoveMoney(int money) {
        currentMoney -= money;
    }
   

}

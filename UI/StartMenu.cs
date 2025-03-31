using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
   [SerializeField] Animator scenetrans; 


    public void Start()
    {scenetrans = GetComponent<Animator>();
      
    }

    public void StartGameMethod() {
        scenetrans.SetTrigger("fade");
     
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene("AreaOne");
        Debug.Log("Game Started");

    }
}

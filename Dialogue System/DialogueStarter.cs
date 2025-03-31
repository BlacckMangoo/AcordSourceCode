using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    [SerializeField] DialogueNode dialogueToStart;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.gameObject.tag == "Player")
        {
            FindObjectOfType<DialogueSystem>().StartDialogue(dialogueToStart);
            
        }
        Debug.Log("Triggered");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FindObjectOfType<DialogueSystem>().EndDialogue();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                FindObjectOfType<DialogueSystem>().NextDialogue();
            }

            if(Input.GetKeyDown(KeyCode.B))
            {
                FindObjectOfType<DialogueSystem>().PrevDialogue();
            }
        }


    }


}

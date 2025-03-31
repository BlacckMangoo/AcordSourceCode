using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] Image potraitImage;
    [SerializeField] TextMeshProUGUI dialogueText;
     DialogueNode currentDialogueNode;
    [SerializeField] GameObject DialogueUiHolder; 

    [SerializeField] int dialogueToDisplay  = 0;
    public void StartDialogue(DialogueNode dialogueNode)
    {
        DialogueUiHolder.SetActive(true);
        currentDialogueNode = dialogueNode;

        // Reset the dialogue index to 0.
        dialogueToDisplay = 0;

        // Check to ensure there's at least one dialogue line.
        if (currentDialogueNode.dialogueText.Count > 0)
        {
            dialogueText.text = currentDialogueNode.dialogueText[dialogueToDisplay];
        }
        else
        {
            Debug.LogError("Dialogue list is empty!");
        }

        potraitImage.sprite = currentDialogueNode.potraitSprite;
        Debug.Log("Starting Dialogue");
    }

    public void EndDialogue()
    {
        DialogueUiHolder.SetActive(false);
        currentDialogueNode = null;

    }

    public void NextDialogue()
    {
        dialogueToDisplay += 1;
        if (dialogueToDisplay < currentDialogueNode.dialogueText.Count)
        {
            dialogueText.text = currentDialogueNode.dialogueText[dialogueToDisplay];
        }
        else
        {
            EndDialogue();
        }
    }

    public void PrevDialogue()
    {
        dialogueToDisplay -= 1;
        if (dialogueToDisplay >= 0)
        {
            dialogueText.text = currentDialogueNode.dialogueText[dialogueToDisplay];
        }
        else
        {
            dialogueToDisplay = 0;
        }



    }

}

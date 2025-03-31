using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Dialogue System/Dialogue Node")]
public class DialogueNode : ScriptableObject
{

    [SerializeField] public  List<string> dialogueText;
    [SerializeField] public  Sprite potraitSprite;

    
}

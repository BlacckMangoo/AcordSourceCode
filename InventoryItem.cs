using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    

        public string itemName;
        public Sprite icon;
        [TextArea] public string description;
       
        
        public bool isStackable;
        public int maxStackSize = 1;
    
}

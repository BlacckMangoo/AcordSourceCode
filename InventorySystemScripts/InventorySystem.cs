using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{  
    public static InventorySystem Instance { get; private set; }
    public int gridSize =  8;
    public InventorySlot[] slots;
    public event System.Action onInventoryChanged;
    [SerializeField] GameObject[] inventoryUi; 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeInventory();
    }

    private void InitializeInventory()
    {
        slots = new InventorySlot[gridSize];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new InventorySlot();
        }
    }

    public bool AddItem(InventoryItem item)
    {
        if (item.isStackable)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null && slots[i].item == item && slots[i].quantity < item.maxStackSize)
                {
                    slots[i].quantity++;
                    onInventoryChanged?.Invoke();
                    return true;
                }
            }
        }

       
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].item = item;
                slots[i].quantity = 1;
                onInventoryChanged?.Invoke();
                return true;
            }
        }
        return false;
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
           foreach (GameObject ui in inventoryUi)
           {
               ui.SetActive(!ui.activeSelf);
           }
        }
    }
}




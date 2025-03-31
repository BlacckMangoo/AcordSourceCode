using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManger : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform gridContainer;
    private InventorySystem inventory;
    private List<InventorySlotUi> slotUIs = new List<InventorySlotUi>();

    [Header("Notification Settings")]
    public GameObject notificationPrefab;
    public Transform notificationContainer;
    public float messageSpacing = 40f;
    public int maxMessages = 3;

    public static UIManger Instance { get; private set; }
    private List<NotificationHandler> activeNotifications = new List<NotificationHandler>();

    private void Awake()
    {
        // Set up singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        
      

        inventory = InventorySystem.Instance;
        InitializeInventoryUI();
        inventory.onInventoryChanged += UpdateInventoryUI;
    }

    private void InitializeInventoryUI()
    {
        // Clear existing slots
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }
        slotUIs.Clear();

        // Create new slots
        for (int i = 0; i < inventory.gridSize; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, gridContainer);
            InventorySlotUi slotUI = slotObj.GetComponent<InventorySlotUi>();
            slotUIs.Add(slotUI);
        }
        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (i < slotUIs.Count)
            {
                slotUIs[i].UpdateSlot(inventory.slots[i]);
            }
        }
    }

    public void ShowMessage(string message)
    {
        
        
        // Clean up destroyed notifications
        activeNotifications.RemoveAll(x => x == null);

        // Remove oldest if at capacity
        if (activeNotifications.Count >= maxMessages && activeNotifications.Count > 0)
        {
            var oldest = activeNotifications[0];
            if (oldest != null && oldest.gameObject != null)
            {
                activeNotifications.RemoveAt(0);
                Destroy(oldest.gameObject);
            }
        }

        // Create new notification
        GameObject notificationObj = Instantiate(notificationPrefab, notificationContainer);
        NotificationHandler notification = notificationObj.GetComponent<NotificationHandler>();

        if (notification == null)
        {
         
            Destroy(notificationObj);
            return;
        }

        // Position the notification
        notificationObj.transform.localPosition = new Vector3(0, activeNotifications.Count * messageSpacing, 0);

        // Add to list and show
        activeNotifications.Add(notification);
        notification.DisplayMessage(message);
    }

    private void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.onInventoryChanged -= UpdateInventoryUI;
        }
    }



}
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Shop Item", menuName = "Shop/Shop Item")]
public class ShopItem : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite itemSprite;
    public int cost;

}

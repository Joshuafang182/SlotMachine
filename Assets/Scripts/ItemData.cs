using UnityEngine;

[CreateAssetMenu(menuName = "MyDataBase/ItemData", fileName = "ItemData")]
public class ItemData : ScriptableObject
{
    public int NumberOfSpinWheels;
    public Sprite[] sprites;
}

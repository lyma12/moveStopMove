using UnityEngine;

[CreateAssetMenu(fileName = "weapon item data", menuName = "Weapon Item", order = 1)]
public class WeaponItemData : ItemData
{
    [SerializeField]
    private Weapon weapon;
    [SerializeField]
    private WeaponType wpType;
    public Weapon Weapon
    {
        get { return weapon; }
    }
    public WeaponType WeaponType
    {
        get { return wpType; }
    }
}
public abstract class ItemData : ScriptableObject
{
    [SerializeField]
    protected int price;
    public int Price
    {
        get { return price; }
    }
    [SerializeField]
    private string nameItem;
    public string NameItem
    {
        get { return nameItem; }
    }
    [SerializeField]
    private Texture image;
    public Texture Image
    {
        get { return image; }
    }
}
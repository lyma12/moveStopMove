using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "weapons data", menuName = "Weapon Data", order = 1)]
public class WeaponData : ScriptableObject, IItemData
{
    [SerializeField]
    private List<WeaponItemData> wpItemDataList;
    public List<WeaponItemData> Listdata
    {
        get { return wpItemDataList; }
    }

    public int GetPriceItem(int type)
    {
        return GetWeapon((WeaponType)type).Price;
    }

    public WeaponItemData GetWeapon(WeaponType wpType)
    {
        return wpItemDataList[(int)wpType];
    }
}
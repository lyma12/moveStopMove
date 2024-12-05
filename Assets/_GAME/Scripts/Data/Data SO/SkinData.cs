using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "skin data", menuName = "skins", order = 1)]
public class SkinData : ScriptableObject, IItemData
{
    [SerializeField]
    List<SkinItemData> skins = new List<SkinItemData>();
    public List<SkinItemData> ListData
    {
        get { return skins; }
    }

    public int GetPriceItem(int type)
    {
        return GetSkin((SkinType)type).Price;
    }

    public SkinItemData GetSkin(SkinType type)
    {
        return skins[(int)type];
    }
}
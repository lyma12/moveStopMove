using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "pant data", menuName = "Pants", order = 1)]
public class PantData : ScriptableObject, IItemData
{
    [SerializeField]
    List<PantItemData> pants = new List<PantItemData>();
    public List<PantItemData> ListData
    {
        get { return pants; }
    }
    public PantItemData GetPant(PantType type)
    {
        return pants[(int)type];
    }

    public int GetPriceItem(int type)
    {
        return GetPant((PantType)type).Price;
    }
}
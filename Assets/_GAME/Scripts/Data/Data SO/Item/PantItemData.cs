using UnityEngine;
[CreateAssetMenu(fileName = "pant item data", menuName = "Pant Item", order = 1)]
public class PantItemData : ItemData
{
    [SerializeField]
    private PantType pantType;
    [SerializeField]
    public Material pant;
    public Material Pant
    {
        get { return pant; }
    }
    public PantType PantType
    {
        get { return pantType; }
    }
}
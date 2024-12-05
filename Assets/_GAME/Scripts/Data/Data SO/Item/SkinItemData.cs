using UnityEngine;
[CreateAssetMenu(fileName = "skin item data", menuName = "skin Item", order = 2)]
public class SkinItemData : ItemData
{
    [SerializeField]
    private Material skin;
    public Material Skin
    {
        get { return skin; }
    }
    [SerializeField]
    private SkinType skinType;
    public SkinType SkinType
    {
        get { return skinType; }
    }
}
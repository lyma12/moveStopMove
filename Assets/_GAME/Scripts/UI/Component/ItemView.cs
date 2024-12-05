using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    [SerializeField]
    protected Image image;
    [SerializeField]
    protected TMP_Text nameItem;
    [SerializeField]
    protected TMP_Text priceItem;
    [SerializeField]
    protected GameObject lockWeapon;

    protected int price;
    public int Price
    {
        get { return price; }
    }
    protected Player player;
    protected ItemData item;
    public virtual void OnInit<T>(T item, Player player, bool isHave) where T : ItemData
    {
        this.item = item;
        this.player = player;
        nameItem.text = item.NameItem;
        priceItem.text = $"{item.Price} $";
        if (item.Image is Texture texture)
        {
            image.sprite = TextureToSprite(texture);
        }
        lockWeapon.SetActive(!isHave);
    }
    private Sprite TextureToSprite(Texture texture)
    {
        return Sprite.Create((Texture2D)texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
    public virtual void TakeItem()
    {
        switch (item)
        {
            case WeaponItemData:
                var typeWeapon = (item as WeaponItemData).WeaponType;
                player.ChangeWeapon(typeWeapon);
                break;
            case SkinItemData:
                var typeSkin = (item as SkinItemData).SkinType;
                player.ChangeSkin(typeSkin);
                break;
            case PantItemData:
                var typePant = (item as PantItemData).PantType;
                player.ChangePant(typePant);
                break;
        }
    }
}
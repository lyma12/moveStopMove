using System;
using System.Collections.Generic;
using UnityEngine;

class Cache : Singleton<Cache>
{
    private Dictionary<Collider, Character> dictionaryCharacterFromCollision = new Dictionary<Collider, Character>();
    private Dictionary<Transform, Character> dictionaryCharacterFromTransform = new Dictionary<Transform, Character>();
    private Dictionary<GameObject, ItemView> dictionaryItemView = new Dictionary<GameObject, ItemView>();
    private Dictionary<GameObject, Bullet> dictionaryBullet = new Dictionary<GameObject, Bullet>();
    private Dictionary<GameObject, Level> dictionaryLevel = new Dictionary<GameObject, Level>();
    private Player player;
    public Player GetPlayer(GameObject gameObject)
    {
        if (player != null)
        {
            return player;
        }
        else
        {
            player = gameObject.GetComponent<Player>();
            return player;
        }
    }
    public T GetCharacter<T>(Collider collision) where T : Character
    {
        if (dictionaryCharacterFromCollision.ContainsKey(collision))
        {
            return dictionaryCharacterFromCollision[collision] as T;
        }
        else
        {
            Character character = collision.gameObject.GetComponent<T>();
            dictionaryCharacterFromCollision.Add(collision, character);
            return character as T;
        }
    }
    public T GetCharacter<T>(Transform otherTransform) where T : Character
    {
        if (dictionaryCharacterFromTransform.ContainsKey(otherTransform))
        {
            return dictionaryCharacterFromTransform[otherTransform] as T;
        }
        else
        {
            Character character = otherTransform.gameObject.GetComponent<T>();
            dictionaryCharacterFromTransform[otherTransform] = character;
            return character as T;
        }
    }

    public Bullet GetBullet(GameObject bulletObject)
    {
        if (dictionaryBullet.ContainsKey(bulletObject))
        {
            return dictionaryBullet[bulletObject];
        }
        else
        {
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            dictionaryBullet.Add(bulletObject, bullet);
            return bullet;
        }
    }
    public ItemView GetItemView(GameObject itemObject)
    {
        if (dictionaryItemView.ContainsKey(itemObject))
        {
            return dictionaryItemView[itemObject];
        }
        else
        {
            ItemView itemView = itemObject.GetComponent<ItemView>();
            dictionaryItemView.Add(itemObject, itemView);
            return itemView;
        }
    }
    public Level GetLevel(GameObject levelObject)
    {
        if (dictionaryLevel.ContainsKey(levelObject))
        {
            return dictionaryLevel[levelObject];
        }
        else
        {
            Level level = levelObject.GetComponent<Level>();
            dictionaryLevel.Add(levelObject, level);
            return level;
        }
    }
}
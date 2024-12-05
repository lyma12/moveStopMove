using System;
using UnityEngine;

public class Weapon : GameUnit
{
    [SerializeField]
    private WeaponType wpType;
    public WeaponType Type
    {
        get
        {
            return wpType;
        }
    }
    [SerializeField]
    private Bullet bullet;
    [SerializeField]
    private float mass = 100f;
    [SerializeField]
    private AudioClip audioThrowWeapon;
    public void Shoot(Character attacker, Action<Character, Character> onHit, Vector3 target)
    {

        Vector3 direction = target - transform.position;
        direction.y = 0;
        Vector3 force = (direction / 4) * mass;
        //var objectSpawn = ObjectPoolManager.Instance.SpawnObject(bullet, transform.position, Quaternion.identity);

        //Bullet bulletSpawn = Cache.Instance.GetBullet(objectSpawn);
        Bullet bulletSpawn = SimplePool.Spawn<Bullet>(bullet, transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySFX(audioThrowWeapon);
        bulletSpawn.OnInit(attacker, onHit, force);
        gameObject.SetActive(false);
        //ObjectPoolManager.Instance.ReturnObjectToPool(objectSpawn, 2f);
        Invoke(nameof(SetActiveBeforeShoot), 1.7f);
    }
    private void SetActiveBeforeShoot()
    {
        gameObject.SetActive(true);
    }

    public override void OnInit()
    {

    }

    public override void OnDespawn()
    {

    }
}
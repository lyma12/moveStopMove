using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : GameUnit
{
    ///
    [SerializeField] private Transform transformWeapon;
    [SerializeField] protected AudioClip audioOnDie;
    [SerializeField] protected ParticleSystem levelUp;
    protected Weapon weapon;
    [SerializeField]
    protected float size = 1;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    protected Transform render;
    protected string currentAnim;
    [SerializeField]
    private LayerMask characterLayer;
    private Collider[] listEnemies = new Collider[10];
    private int numOfEnemies = 0;
    [SerializeField]
    private SkinnedMeshRenderer pantMeshRender;
    [SerializeField]
    private SkinnedMeshRenderer skinMeshRender;
    ///
    [SerializeField]
    protected Transform circleFindTarget;
    [SerializeField]
    private float timeWaitAttack = 3f;
    protected float currentTime = 0f;
    protected int numOfKillVictim = 0;

    protected Character target = null;

    public virtual void ChangeAnim(string newAnim)
    {
        if (!currentAnim.Equals(newAnim))
        {
            anim.ResetTrigger(currentAnim);
            currentAnim = newAnim;
            anim.SetTrigger(newAnim);
        }
    }
    private void OnEnable()
    {
        OnInit();
    }
    protected virtual void OnCharacterInit()
    {
        currentAnim = AnimName.AnimIdle;
        ChangeAnim(AnimName.AnimIdle);
        currentTime = 0f;
        numOfEnemies = 0;
        transform.localScale = Vector3.one;
        size = 1;
    }
    public virtual void Reset()
    {
        OnInit();
    }

    protected virtual void Move()
    {
        ChangeAnim(AnimName.AnimRun);
    }
    protected virtual void Attack()
    {
        if (currentTime <= 0)
        {
            var direction = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
            render.rotation = Quaternion.LookRotation(direction);
            ChangeAnim(AnimName.AnimAttack);
            weapon.Shoot(this, OnHitVictim, target.transform.position);
            target = null;
            currentTime = timeWaitAttack;
            Invoke(nameof(ChangeAnimIdle), 1.7f);
        }
        else
        {
            currentTime -= Time.deltaTime;
        }
    }
    protected virtual void OnHitVictim(Character attacker, Character victim)
    {
        if (victim != null)
        {
            victim.Die();
            if (numOfEnemies > Contanst.NumberOfEnemyHaveKillToUpSize * size)
            {
                UpSize();
            }
        }
    }

    private void ChangeAnimIdle()
    {
        ChangeAnim(AnimName.AnimIdle);
    }

    private IEnumerator DespawnObject()
    {
        yield return new WaitForSeconds(Contanst.TimeOfAnimDie);
        try
        {
            // ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
            SimplePool.Despawn(this);
        }
        catch (Exception)
        {
            Destroy(gameObject);
        }
    }
    protected virtual void Die()
    {
        ChangeAnim(AnimName.AnimDie);
        StartCoroutine(DespawnObject());
    }
    public void ChangeWeapon(WeaponType weaponType)
    {
        if (weapon != null)
        {
            if (weapon.Type == weaponType) return;
            else
            {
                //ObjectPoolManager.Instance.ReturnObjectToPool(weapon.gameObject);
                SimplePool.Despawn(weapon);
                weapon = null;
            }
        }
        var dataWeapon = DataManager.Instance.GetWeapon(weaponType);
        weapon = SimplePool.Spawn(dataWeapon.Weapon, transformWeapon.position, Quaternion.identity, transformWeapon);
        //weapon = ObjectPoolManager.Instance.SpawnObject(dataWeapon.Weapon.gameObject, transformWeapon.position, Quaternion.identity, transformWeapon).GetComponent<Weapon>();
    }
    public void ChangeHat()
    {

    }
    public void ChangePant(PantType type)
    {
        pantMeshRender.material = DataManager.Instance.GetPant(type);
    }

    public void ChangeSkin(SkinType type)
    {
        skinMeshRender.material = DataManager.Instance.GetSkin(type);
    }

    protected void UpSize()
    {
        transform.localScale *= Contanst.UpSizeCharacterLevelUp;
        size++;
        levelUp.Play();
    }
    protected virtual void FindTarget()
    {
        float radius = circleFindTarget.localScale.x * Contanst.RadiusCircleFindTarget;
        Vector2 origin = new Vector2(circleFindTarget.position.x, circleFindTarget.position.z);
        numOfEnemies = Physics.OverlapSphereNonAlloc(origin, radius, listEnemies, characterLayer);
        Transform closestEnemy = null;
        float closestDistanceSquared = float.MaxValue;
        for (int i = 0; i < numOfEnemies; i++)
        {
            Transform enemy = listEnemies[i].transform;
            if (gameObject.transform != enemy)
            {
                float distanceSquared = (origin - new Vector2(enemy.position.x, enemy.position.z)).sqrMagnitude;
                if (distanceSquared < closestDistanceSquared)
                {
                    closestDistanceSquared = distanceSquared;
                    closestEnemy = enemy;
                }
            }

        }
        if (closestEnemy != null) target = Cache.Instance.GetCharacter<Character>(closestEnemy);
        else target = null;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag.CompareTo(Contanst.TAG_LAKE) == 0)
        {
            Die();
        }
    }
    void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }

    public override void OnInit()
    {
        OnCharacterInit();
    }

    public override void OnDespawn()
    {
        currentTime = 0f;
        numOfEnemies = 0;
        transform.localScale = Vector3.one;
        size = 1;
        render.eulerAngles = Vector3.one;
    }
}

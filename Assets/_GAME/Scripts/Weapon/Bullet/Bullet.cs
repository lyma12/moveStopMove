using System;
using System.Collections;
using UnityEngine;

public class Bullet : GameUnit
{
    public float rotationSpeed = 30f;
    protected Character attacker;
    protected Action<Character, Character> onHit;
    [SerializeField] private AudioClip soundOnCollision;
    [SerializeField]
    private Transform render;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private LayerMask characterLayer;
    private void FixedUpdate()
    {
        render.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
    }
    public void OnInit(Character attacker, Action<Character, Character> onHit, Vector3 force)
    {
        rb.AddForce(force);
        this.onHit = onHit;
        this.attacker = attacker;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & characterLayer) != 0)
        {
            if (attacker == null)
            {
                onHit.Invoke(attacker, Cache.Instance.GetCharacter<Character>(other));
                //ObjectPoolManager.Instance.ReturnObjectToPool(gameObject, 0.05f);
                SimplePool.Despawn(this);
                return;
            }
            if (other.gameObject != attacker.gameObject)
            {
                onHit.Invoke(attacker, Cache.Instance.GetCharacter<Character>(other));
                SimplePool.Despawn(this);
            }
        }
        SoundManager.Instance.PlaySFX(soundOnCollision);
    }
    private IEnumerator DespawnBullet()
    {
        yield return new WaitForSeconds(Contanst.TimeDespawnBullet);
        SimplePool.Despawn(this);
    }
    public override void OnInit()
    {
        StartCoroutine(DespawnBullet());
    }

    public override void OnDespawn()
    {
        attacker = null;
        rb.velocity = Vector3.zero;
        onHit = null;
    }
}
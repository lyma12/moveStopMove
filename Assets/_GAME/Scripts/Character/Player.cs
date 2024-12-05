using System.Collections;
using UnityEngine;

public class Player : Character
{
    [SerializeField]
    private Joystick joystick;
    public Joystick Joystick
    {
        set
        {
            joystick = value;
        }
    }
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private CircleTarget circleBelowTarget;
    private GameUnit circleTargetSpawn;
    private Transform currentTarget;
    protected override void Move()
    {
        base.Move();
        Vector3 direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        rb.velocity = direction;
        if (direction != Vector3.zero) render.rotation = Quaternion.LookRotation(direction);
    }
    protected override void OnHitVictim(Character attacker, Character victim)
    {
        base.OnHitVictim(attacker, victim);
        SoundManager.Instance.PlaySFX(audioOnDie);
    }
    protected void FixedUpdate()
    {
        if (rb.velocity == Vector3.zero)
        {
            if (target != null)
            {
                Attack();
            }
        }
        else
        {
            if (currentTime >= 0)
            {
                currentTime -= Time.deltaTime;
            }
        }
        StartCoroutine(FindTargetConroutine());
    }
    private IEnumerator FindTargetConroutine()
    {
        FindTarget();
        yield return null;
    }

    private void Update()
    {
        if (joystick != null)
        {
            if (joystick.Direction != Vector2.zero)
            {
                Move();
            }
            else if (joystick.Direction == Vector2.zero)
            {
                rb.velocity = Vector3.zero;
                if (target == null) ChangeAnim(AnimName.AnimIdle);
                else if (currentAnim == AnimName.AnimRun)
                {
                    ChangeAnim(AnimName.AnimIdle);
                }
            }
        }
    }
    public override void OnDespawn()
    {
        rb.velocity = Vector3.zero;
        target = null;
        base.OnDespawn();
    }
    protected override void OnCharacterInit()
    {
        base.OnCharacterInit();
        currentTarget = null;
    }

    protected override void FindTarget()
    {
        base.FindTarget();
        if (target == null)
        {
            if (circleTargetSpawn != null)
            {
                //ObjectPoolManager.Instance.ReturnObjectToPool(circleTargetSpawn.gameObject);
                SimplePool.Despawn(circleTargetSpawn);
            }
            return;
        }
        if (currentTarget == target) return;
        else if (circleTargetSpawn != null)
        {
            //ObjectPoolManager.Instance.ReturnObjectToPool(circleTargetSpawn.gameObject);
            SimplePool.Despawn(circleTargetSpawn);
            circleTargetSpawn = null;
        }
        if (circleTargetSpawn == null && target != null)
            //circleTargetSpawn = ObjectPoolManager.Instance.SpawnObject(circleBelowTarget, target.transform.position, Quaternion.identity, target.transform).transform;
            circleTargetSpawn = SimplePool.Spawn(circleBelowTarget, target.transform.position, Quaternion.identity, target.transform);
        if (target == null && circleTargetSpawn != null)
        {
            //ObjectPoolManager.Instance.ReturnObjectToPool(circleTargetSpawn.gameObject);
            SimplePool.Despawn(circleTargetSpawn);
            circleTargetSpawn = null;
        }
        currentTarget = target.transform;
    }
    protected override void Die()
    {
        if (LevelManager.Instance.LevelState == LevelState.PLAY)
        {
            LevelManager.Instance.LevelState = LevelState.ON_ENDGAME;
        }
        if (circleTargetSpawn != null)
        {
            SimplePool.Despawn(circleTargetSpawn);
        }
        ChangeAnim(AnimName.AnimDie);
    }
}

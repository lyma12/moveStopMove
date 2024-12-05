using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character, ISubject
{
    private List<IObserver> _observer = new List<IObserver>();
    protected IState<Bot> currentState;
    [SerializeField]
    private NavMeshAgent agent;
    private Vector3 destionation;
    private float maxDistance = 4f;
    public float MaxDistance
    {
        get
        {
            return maxDistance;
        }
    }
    public Character Target
    {
        get
        {
            return target;
        }
    }
    public bool IsDestination => Vector3.Distance(destionation, Vector3.right * transform.position.x + Vector3.forward * transform.position.z) < 0.1f;
    public void ResetCurrentTime()
    {
        currentTime = 0f;
    }
    public override void OnDespawn()
    {
        ChangeState(new IdleState());
        base.OnDespawn();
    }
    protected override void OnCharacterInit()
    {
        base.OnCharacterInit();
        ChangeState(new IdleState());
        target = null;
    }
    protected void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
    }
    public void ResetRenderRotation()
    {
        render.rotation = Quaternion.LookRotation(transform.forward);
    }
    public void ChangeState(IState<Bot> newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        currentState.OnEnter(this);
    }
    public void SetDestination(Vector3 position)
    {
        try
        {
            agent.enabled = true;
            destionation = position;
            destionation.y = 0;
            agent.SetDestination(position);
            ChangeAnim(AnimName.AnimRun);
        }
        catch
        {
            //ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
            SimplePool.Despawn(this);
            Notify();
        }
    }
    public void FindTargetAttack()
    {
        base.FindTarget();
    }
    public void OnAttack()
    {
        base.Attack();
    }
    protected override void Die()
    {
        base.Die();
        ChangeState(new DieState());
        agent.enabled = false;
        Notify();
    }
    internal void MoveStop()
    {
        ChangeAnim(AnimName.AnimIdle);
        agent.enabled = false;
    }

    public void Attach(IObserver observer)
    {
        if (_observer.Contains(observer)) return;
        _observer.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        if (_observer.Contains(observer))
        {
            _observer.Remove(observer);
        }
    }

    public void Notify()
    {
        foreach (IObserver observer in _observer)
        {
            observer.UpdateState(this);
        }
    }
}
using UnityEngine;
using UnityEngine.AI;

public class FindState : IState<Bot>
{
    public void OnEnter(Bot t)
    {
        t.ResetRenderRotation();
        SeekTarget(t);
    }

    public void OnExecute(Bot t)
    {
        t.FindTargetAttack();
        if(t.Target != null){
            t.ChangeState(new AttackState());
        }else{
            if(t.IsDestination){
                t.ChangeAnim(AnimName.AnimIdle);
                SeekTarget(t);
            }
        }
    }

    public void OnExit(Bot t)
    {
    }
    private void SeekTarget(Bot t){
        var playerTransform = LevelManager.Instance.Player;
        if(playerTransform != null)
        {
            Vector3 randomPos = Random.insideUnitSphere * t.MaxDistance + playerTransform.gameObject.transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, t.MaxDistance, NavMesh.AllAreas);
            if(hit.position != null){
                t.SetDestination(hit.position);
            }
            else{
                t.SetDestination(t.transform.position);
            }
        }
    }
}
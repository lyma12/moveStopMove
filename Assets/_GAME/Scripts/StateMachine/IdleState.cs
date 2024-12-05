using UnityEngine;

public class IdleState : IState<Bot>
{
    private float currentTime = 0f;
    private float maxTime = 2f;

    public void OnEnter(Bot t)
    {
        currentTime = 0;
        t.ChangeAnim(AnimName.AnimIdle);
        t.MoveStop();
    }

    public void OnExecute(Bot t)
    {
        if(currentTime > maxTime){
            t.ChangeState(new FindState());
        }
        else{
            currentTime += Time.deltaTime;
        }
    }

    public void OnExit(Bot t)
    {
        // 
    }
}
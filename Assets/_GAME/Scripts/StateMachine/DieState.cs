using UnityEngine;

public class DieState : IState<Bot>
{

    public void OnEnter(Bot t)
    {
        t.ChangeAnim(AnimName.AnimDie);
    }

    public void OnExecute(Bot t)
    {
    }

    public void OnExit(Bot t)
    {
        // 
    }
}
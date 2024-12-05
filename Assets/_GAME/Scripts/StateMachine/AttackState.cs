public class AttackState : IState<Bot>
{
    public void OnEnter(Bot t)
    {
        t.MoveStop();
    }

    public void OnExecute(Bot t)
    {
        if(t.Target != null){
            t.OnAttack();
        }else{
            t.ChangeState(new IdleState());
        }
    }

    public void OnExit(Bot t)
    {
        
    }
}
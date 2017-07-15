using UnityEngine;

public sealed class WaitState : State <Bob>
{
    static readonly WaitState instance = new WaitState();

    public static WaitState Instance
    {
        get
        {
            return instance;
        }
    }

    static WaitState() { }
    private WaitState() { }

    public override void Enter(Bob agent)
    {
        Debug.Log("Starting to wait...");
    }

    public override void Execute(Bob agent)
    {
        agent.IncreaseWaitedTime(1);
        Debug.Log("...waiting for " + agent.waitedTime + " cycle" + (agent.waitedTime > 1 ? "s" : "") + " so far...");
        if (agent.WaitedLongEnough()) agent.ChangeState(WaitState.Instance);
    }

    public override void Exit(Bob agent)
    {
        agent.waitedTime = 0;
        Debug.Log("...waited long enough!");
    }
}
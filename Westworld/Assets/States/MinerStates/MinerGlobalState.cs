
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinerGlobalState : State<Bob>
{
    static readonly MinerGlobalState instance = new MinerGlobalState();

    public static MinerGlobalState Instance
    {
        get
        {
            return instance;
        }
    }

    static MinerGlobalState() { }
    private MinerGlobalState() { }
    
    public override void Enter(Bob agent)
    {
        agent.thirst = 0;
        agent.tired = 0;

    }


    public override void Execute(Bob agent)
    {
        agent.thirst++;
        agent.tired++;
        

        if (agent.thirst == 200.0f)  
            agent.ChangeState(QuenchThirstState.Instance);

        if(agent.tired==250)
            agent.ChangeState(GoHomeState.Instance);

    }

    public override void Exit(Bob agent)
    {

    }
}

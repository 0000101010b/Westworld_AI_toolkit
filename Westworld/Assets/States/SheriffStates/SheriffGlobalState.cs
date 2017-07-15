using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheriffGlobalState : State<Sheriff>
{
    static readonly SheriffGlobalState instance = new SheriffGlobalState();

    public static SheriffGlobalState Instance
    {
        get
        {
            return instance;
        }
    }

    static SheriffGlobalState() { }
    private SheriffGlobalState() { }

    public override void Enter(Sheriff agent)
    {
        agent.thirst = 0;
    }


    public override void Execute(Sheriff agent)
    {

    }

    public override void Exit(Sheriff agent)
    {

    }
}

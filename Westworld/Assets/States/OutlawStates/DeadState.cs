using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State<Outlaw>
{
    static readonly DeadState instance = new DeadState();

    public static DeadState Instance
    {
        get
        {
            return instance;
        }
    }

    static DeadState() { }
    private DeadState() { }

    public override void Enter(Outlaw agent)
    {
        agent.CreateSpeechBubble("DEAD");
        agent.Dead();
    }


    public override void Execute(Outlaw agent)
    {


    }

    public override void Exit(Outlaw agent)
    {

    }
}

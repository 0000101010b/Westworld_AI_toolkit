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

    Vector2 uPos;
    public override void Enter(Outlaw agent)
    {
        agent.CreateSpeechBubble("DEAD");
        agent.Dead();
        GameObject g = GameObject.Find(eLocation.Undertakers.ToString());
        uPos = new Vector2(g.transform.position.x, g.transform.position.z);

    }


    public override void Execute(Outlaw agent)
    { 
        if(agent.pos == uPos)
            agent.Awake();
        
    }

    public override void Exit(Outlaw agent)
    {

    }
}

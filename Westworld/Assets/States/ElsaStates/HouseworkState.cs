using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseworkState : State<Elsa>
{
    static readonly HouseworkState instance = new HouseworkState();

    public static HouseworkState Instance
    {
        get
        {
            return instance;
        }
    }

    static HouseworkState() { }
    private HouseworkState() { }

    int index = 0;
    public override void Enter(Elsa agent)
    {
        agent.CreateSpeechBubble("Doin' the Housework");
        agent.enableCooking();
    }

   
    public override void Execute(Elsa agent)
    {
        if (agent.isStart)
        {
            agent.isStart = false;
            agent.CreateSpeechBubble("Doin' the Housework");
            agent.enableCooking();
        }
    }

    public override void Exit(Elsa agent)
    {
        agent.disableCooking();
    }
}

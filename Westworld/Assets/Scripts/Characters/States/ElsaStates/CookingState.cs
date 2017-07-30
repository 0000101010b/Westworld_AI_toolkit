using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingState : State<Elsa>
{
    static readonly CookingState instance = new CookingState();

    public static CookingState Instance
    {
        get
        {
            return instance;
        }
    }

    static CookingState() { }
    private CookingState() { }

    int index = 0;
    public override void Enter(Elsa agent)
    {
        agent.waitedTime = 0;
        agent.CreateSpeechBubble("Doin' the Cooking");
    }


    public override void Execute(Elsa agent)
    {
        agent.IncreaseWaitedTime(1);
        if (agent.WaitedLongEnough())
        {
            agent.Cooked();
            agent.ChangeState(HouseworkState.Instance);
        }
    }
    public override void Exit(Elsa agent)
    {

    }
}

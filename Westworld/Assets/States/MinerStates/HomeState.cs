using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeState : State<Bob>
{
    static readonly HomeState instance = new HomeState();


 

    public static HomeState Instance
    {
        get
        {
            return instance;
        }
    }

    static HomeState() { }
    private HomeState() { }

    int index = 0;
    public override void Enter(Bob agent)
    {
        agent.waitedTime = 0;

        agent.eatenStew = false;

        agent.CreateSpeechBubble("Honey' im Home");

        //agent.Arrive(eAgent.Bob, eLocation.Shack);

        GameObject g=GameObject.Find(eLocation.Shack.ToString());
        g.GetComponent<Location>().agents.Add((int)eAgent.Bob);

        agent.enableEat();
        agent.ArrivingHome();

    }
    public override void Execute(Bob agent)
    {
        if(agent.eatenStew)
        {
            agent.IncreaseWaitedTime(1);
            if (agent.WaitedLongEnough())
                agent.ChangeState(GoMineState.Instance);
        }
    
    }

    public override void Exit(Bob agent)
    {
        GameObject g = GameObject.Find(eLocation.Shack.ToString());
        g.GetComponent<Location>().agents.Remove((int)eAgent.Bob);
        agent.disableEat();
        agent.thirst = 0;
        agent.tired = 0;
        agent.CreateSpeechBubble("No longer tired");
    }

}

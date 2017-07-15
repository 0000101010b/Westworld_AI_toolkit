using UnityEngine;
using System.Collections;

public class QuenchThirstState : State<Bob>
{
    static readonly QuenchThirstState instance = new QuenchThirstState();

    public static QuenchThirstState Instance
    {
        get
        {
            return instance;
        }
    }

    static QuenchThirstState() { }
    private QuenchThirstState() { }

    bool atSaloon = false;
    int index = 0;
    public override void Enter(Bob agent)
    {
        atSaloon = false;
        index = 0;
        agent.waitedTime = 0;
        GameObject g = GameObject.Find("Saloon");
        agent.toLoc = new Vector2(g.transform.position.x, g.transform.position.z);

        agent.path = agent.aStar();
        //agent.Arrive(eAgent.Bob, eLocation.other);

        agent.CreateSpeechBubble("Goin' to the Saloon");
    }


    public override void Execute(Bob agent)
    {
        if (agent.path.Count == index)//at saloon
        {
            agent.IncreaseWaitedTime(1);

            if (!atSaloon)
            {
                agent.CreateSpeechBubble("Drink!!");
                GameObject g = GameObject.Find(eLocation.Saloon.ToString());
                g.GetComponent<Location>().agents.Add((int)eAgent.Bob);
            }

            if (agent.WaitedLongEnough())
            {
                if (agent.tired >= 200)
                    agent.ChangeState(GoHomeState.Instance);
                else
                    agent.ChangeState(GoMineState.Instance);
            }
        }
        else//move to saloon
        {
            Point newPos = agent.path[index];
            index++;
            agent.pos = new Vector2(newPos.x, newPos.y);
        }
    }

    public override void Exit(Bob agent)
    {
        agent.thirst = 0;
        GameObject g = GameObject.Find(eLocation.Saloon.ToString());
        g.GetComponent<Location>().agents.Remove((int)eAgent.Bob);
        agent.CreateSpeechBubble("No longer thirsty");
    }
}

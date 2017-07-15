
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoMineState : State<Bob>
{
    static readonly GoMineState instance = new GoMineState();

    public static GoMineState Instance
    {
        get
        {
            return instance;
        }
    }

    static GoMineState() { }
    private GoMineState() { }

    int index = 0;
    public override void Enter(Bob agent)
    {
        index = 0;
        GameObject mine = GameObject.Find("Mine");
        agent.toLoc = new Vector2(mine.transform.position.x, mine.transform.position.z);

        agent.path = agent.aStar();
        if (agent.path == null)
            agent.path = new List<Point>();

        agent.CreateSpeechBubble("Goin' to the Mine");
    }


    public override void Execute(Bob agent)
    {
        if (agent != null)
        {
            if (agent.path != null)
            {
                if (agent.path.Count == index)
                    agent.ChangeState(MiningState.Instance);
                else
                {
                    Point newPos = agent.path[index];
                    index++;
                    agent.pos = new Vector2(newPos.x, newPos.y);
                }
            }
            else
            {
                this.Enter(agent);
            }
        }
    }

    public override void Exit(Bob agent)
    {
        agent.CreateSpeechBubble("Leavin' the Mine");
    }
}

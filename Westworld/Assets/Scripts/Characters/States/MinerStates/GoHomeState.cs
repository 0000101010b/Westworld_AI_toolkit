using UnityEngine;
using System.Collections;

public class GoHomeState : State<Bob>
{
    static readonly GoHomeState instance = new GoHomeState();

    public static GoHomeState Instance
    {
        get
        {
            return instance;
        }
    }

    static GoHomeState() { }
    private GoHomeState() { }

    int index = 0;
    public override void Enter(Bob agent)
    {
        index = 0;
        GameObject g = GameObject.Find(eLocation.Shack.ToString());
        agent.toLoc = new Vector2(g.transform.position.x, g.transform.position.z);
        agent.path = agent.aStar();

        agent.CreateSpeechBubble("Goin' Home");
    }


    public override void Execute(Bob agent)
    {
        if (agent.path.Count == index)
        {
            agent.ChangeState(HomeState.Instance);
        }
        else
        {
            Point newPos = agent.path[index];
            index++;
            agent.pos = new Vector2(newPos.x, newPos.y);
        }
    }

    public override void Exit(Bob agent)
    {
        agent.thirst = 0;
        agent.CreateSpeechBubble("No longer thirsty");
    }
}

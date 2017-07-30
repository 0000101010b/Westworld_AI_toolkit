using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectBodyState : State<Undertaker>
{
    static readonly CollectBodyState instance = new CollectBodyState();

    public static CollectBodyState Instance
    {
        get
        {
            return instance;
        }
    }

    static CollectBodyState() { }
    private CollectBodyState() { }


    int index = 0;
    public override void Enter(Undertaker agent)
    {
        agent.waitedTime = 0;

        Debug.Log(agent.deadBody);
        index = 0;
        getBody(agent);
    }

    public void getBody(Undertaker agent)
    {

        GameObject g = agent.deadBody;
        agent.toLoc = new Vector2(g.transform.position.x, g.transform.position.z);
        agent.CreateSpeechBubble("Goin' to Dead Body");
        agent.path = agent.aStar();
    }



    public override void Execute(Undertaker agent)
    {
        if (agent.path.Count > index)
        {
            //go to next point
            Point newPos = agent.path[index];
            index++;
            agent.pos = new Vector2(newPos.x, newPos.y);

        }
        else //go to other location
        {
            agent.pickedUpBody = true;
            agent.ChangeState(GoToUndertakers.Instance);
        }

    }

    public override void Exit(Undertaker agent)
    {
        agent.CreateSpeechBubble("Leavin' the Undertaker s");

        GameObject g = GameObject.Find(eLocation.Undertakers.ToString());
        g.GetComponent<Location>().agents.Remove((int)eAgent.Undertaker);

    }
}

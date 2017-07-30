using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToUndertakers : State<Undertaker>
{
    static readonly GoToUndertakers instance = new GoToUndertakers();

    public static GoToUndertakers Instance
    {
        get
        {
            return instance;
        }
    }

    static GoToUndertakers() { }
    private GoToUndertakers() { }


    int index = 0;
    bool atUndertakers = false;
    public override void Enter(Undertaker agent)
    {

        agent.waitedTime = 0;
        atUndertakers = false;
        index = 0;
        goUndertakers(agent);
    }

    public void goUndertakers(Undertaker agent)
    {

        GameObject g = GameObject.Find(eLocation.Undertakers.ToString());
        agent.toLoc = new Vector2(g.transform.position.x, g.transform.position.z);
        agent.CreateSpeechBubble("Goin' to Undertakers");
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
            if(agent.deadBody!=null)
                agent.deadBody.GetComponent<Agent>().pos= new Vector2(newPos.x, newPos.y);



        }
        else //go to other location
        {
            agent.pickedUpBody = false;
            agent.ChangeState(AtUndertakersState.Instance);

        }

    }

    public override void Exit(Undertaker agent)
    {

    }
}

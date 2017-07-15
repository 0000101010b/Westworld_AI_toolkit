using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoToOutlawCamp : State<Outlaw>
{
    static readonly GoToOutlawCamp instance = new GoToOutlawCamp();

    public static GoToOutlawCamp Instance
    {
        get
        {
            return instance;
        }
    }

    static GoToOutlawCamp() { }
    private GoToOutlawCamp() { }

    
    int index = 0;
    bool atOutlawCamp = false;
    public override void Enter(Outlaw agent)
    {

        agent.waitedTime = 0;
        atOutlawCamp = false;
        index = 0;
        goOutlawCamp(agent);
    }

    public void goOutlawCamp(Outlaw agent)
    {
    
        GameObject g = GameObject.Find(eLocation.OutlawCamp.ToString());
        agent.toLoc = new Vector2(g.transform.position.x, g.transform.position.z);
        agent.CreateSpeechBubble("Goin' to Outlaw Camp");
        agent.path = agent.aStar();
    }



    public override void Execute(Outlaw agent)
    {
        if (agent.path.Count > index)
        {
            //go to next point
            Point newPos = agent.path[index];
            index++;
            agent.pos = new Vector2(newPos.x, newPos.y);
       
        }else //go to other location
        {
            if(!atOutlawCamp)
            {
                GameObject g = GameObject.Find(eLocation.OutlawCamp.ToString());
                g.GetComponent<Location>().agents.Add((int)eAgent.OutlawJesse);

                atOutlawCamp = true;
            }

            agent.IncreaseWaitedTime(1);

            if (agent.WaitedLongEnough())
                agent.ChangeState(LurkState.Instance);
        }
        
    }

    public override void Exit(Outlaw agent)
    {
        agent.CreateSpeechBubble("Leavin' the Outlaw Camp");

        GameObject g = GameObject.Find(eLocation.OutlawCamp.ToString());
        g.GetComponent<Location>().agents.Remove((int)eAgent.OutlawJesse);

    }
}

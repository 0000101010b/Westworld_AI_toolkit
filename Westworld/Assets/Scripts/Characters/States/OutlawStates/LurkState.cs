using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LurkState : State<Outlaw>
{
    static readonly LurkState instance = new LurkState();

    public static LurkState Instance
    {
        get
        {
            return instance;
        }
    }

    static LurkState() { }
    private LurkState() { }

    bool goinOutlawCamp = false;

    int cycles;
    int index = 0;
    bool arrived = false;
    public override void Enter(Outlaw agent)
    {
        cycles=Random.Range(1, 5);
        go(agent,eLocation.Cemetery);
    }

    void go(Outlaw agent,eLocation loc)
    {
        arrived = false;
        goinOutlawCamp = (loc == eLocation.OutlawCamp) ? true : false;
        index = 0;
        GameObject g = GameObject.Find(loc.ToString());

        agent.toLoc = new Vector2(g.transform.position.x, g.transform.position.z);
        agent.CreateSpeechBubble("Goin' to "+ loc.ToString());
        agent.path = agent.aStar();
    }


    public override void Execute(Outlaw agent)
    {

        if (Random.Range(0, 20000) > 19900)//go rob the bank
        {
            agent.ChangeState(RobBankState.Instance);
        }
        else
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

                if(!arrived)
                {
                    arrived = true;
                    if (goinOutlawCamp)
                    {
                        GameObject g = GameObject.Find(eLocation.OutlawCamp.ToString());
                        g.GetComponent<Location>().agents.Add((int)eAgent.OutlawJesse);
                        Debug.Log("+outlaw camp");
                    }
                    else
                    {
                        GameObject g = GameObject.Find(eLocation.Cemetery.ToString());
                        g.GetComponent<Location>().agents.Add((int)eAgent.OutlawJesse);
                        Debug.Log("+cemetery");
                    }
                }
                agent.IncreaseWaitedTime(1);

                if (agent.WaitedLongEnough())
                {
                   
                    if (goinOutlawCamp)
                    {
                        GameObject g = GameObject.Find(eLocation.OutlawCamp.ToString());
                        g.GetComponent<Location>().agents.Remove((int)eAgent.OutlawJesse);
                        go(agent, eLocation.Cemetery);
                        Debug.Log("-outlaw camp");
                    }
                    else
                    {
                        GameObject g = GameObject.Find(eLocation.Cemetery.ToString());
                        g.GetComponent<Location>().agents.Remove((int)eAgent.OutlawJesse);
                        go(agent, eLocation.OutlawCamp);
                        Debug.Log("-cemetery");
                    }
                    arrived = false;
                    agent.waitedTime = 0;
                }
            }
        }

    }

    public override void Exit(Outlaw agent)
    {
        if (arrived)
        {
            if (goinOutlawCamp)
            {
                GameObject g = GameObject.Find(eLocation.OutlawCamp.ToString());
                g.GetComponent<Location>().agents.Remove((int)eAgent.OutlawJesse);
            }
            else
            {
                GameObject g = GameObject.Find(eLocation.Cemetery.ToString());
                g.GetComponent<Location>().agents.Remove((int)eAgent.OutlawJesse);
            }
        }

    }
}

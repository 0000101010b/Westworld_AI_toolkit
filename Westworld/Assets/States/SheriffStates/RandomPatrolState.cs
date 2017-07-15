using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPatrolState :State<Sheriff>
{
    static readonly RandomPatrolState instance = new RandomPatrolState();

    public static RandomPatrolState Instance
    {
        get
        {
            return instance;
        }
    }

    static RandomPatrolState() { }
    private RandomPatrolState() { }



    bool arrived = false;
    int index = 0;
    void go(Sheriff agent, eLocation loc)
    { 
        GameObject g = GameObject.Find(loc.ToString());
     
        agent.toLoc = new Vector2(g.transform.position.x, g.transform.position.z);
        agent.CreateSpeechBubble("Goin' to " + loc.ToString());
        agent.path = agent.aStar();
    }
    int to;
    public override void Enter(Sheriff agent)
    {
        index = 0;

        to = agent.vLoc[Random.Range(0, agent.vLoc.Length )];

        go(agent, (eLocation)to);
 
    }


    public override void Execute(Sheriff agent)
    {
        if (agent.path != null)
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
                
                GameObject g = GameObject.Find(((eLocation)to).ToString());
                List<int> greetings = g.GetComponent<Location>().agents;
                for (int i = 0; i < greetings.Count; i++)
                {
                    if ((eAgent)greetings[i] == eAgent.OutlawJesse)
                        ShootOutlaw(agent);
                    else if ((eAgent)greetings[i] != eAgent.Sheriff && !arrived)
                    {
                        agent.CreateSpeechBubble("Hey' there " + (eAgent)greetings[i]);
                    }
                }
                arrived = true;

                agent.IncreaseWaitedTime(1);

                if (agent.WaitedLongEnough())
                {
                    agent.waitedTime = 0;
                    arrived = false;
                    index = 0;
                    to = agent.vLoc[Random.Range(0, agent.vLoc.Length)];
                    go(agent, (eLocation)to);
                }
            }
        }

       

  
    }

    public void ShootOutlaw(Sheriff agent)
    {
        GameObject g=GameObject.Find(eAgent.OutlawJesse.ToString()+"(Clone)");
        Outlaw outlaw=g.GetComponent<Outlaw>();
        outlaw.ChangeState(DeadState.Instance);
        
    }

    public void sayHello()
    {

    }

    public override void Exit(Sheriff agent)
    {

    }
}

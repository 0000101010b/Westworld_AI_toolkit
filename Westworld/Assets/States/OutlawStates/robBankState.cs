using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RobBankState : State<Outlaw>
{
    static readonly RobBankState instance = new RobBankState();


    public static RobBankState Instance
    {
        get
        {
            return instance;
        }
    }

    static RobBankState() { }
    private RobBankState() { }



    int index = 0;
    bool atBank = false;
    public override void Enter(Outlaw agent)
    {
        atBank = false;
        agent.waitedTime = 0;
        robBank(agent);
    }
    public void robBank(Outlaw agent)
    {
        GameObject g = GameObject.Find(eLocation.Bank.ToString());
        agent.toLoc = new Vector2(g.transform.position.x, g.transform.position.z);

        agent.CreateSpeechBubble("Robin' d' Bank");
        agent.path = agent.aStar();
        index = 0;
    }


    public override void Execute(Outlaw agent)
    {
        if (agent.path.Count == index)
        {
            if (!atBank)
            {
                GameObject g = GameObject.Find(eLocation.Bank.ToString());
                g.GetComponent<Location>().agents.Add((int)eAgent.OutlawJesse);
                atBank = true; 
            }
           
            agent.IncreaseWaitedTime(1);
            if(agent.WaitedLongEnough())
                agent.ChangeState(GoToOutlawCamp.Instance);
        }
        else
        {
            Point newPos = agent.path[index];
            index++;
            agent.pos = new Vector2(newPos.x, newPos.y);
        }

    }

    public override void Exit(Outlaw agent)
    { 
        GameObject g = GameObject.Find(eLocation.Bank.ToString());
        g.GetComponent<Location>().agents.Remove((int)eAgent.OutlawJesse);
        Bank bank=g.GetComponent<Bank>();
        agent.currentGold +=bank.Rob();
        Debug.Log("JESSE GOLD: " + agent.currentGold);
    }
}
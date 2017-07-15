using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoBankState : State<Bob> {

    

    static readonly GoBankState instance = new GoBankState();

    public static GoBankState Instance
    {
        get
        {
            return instance;
        }
    }

    static GoBankState() { }
    private GoBankState() { }



    int index = 0;
    bool atBank = false;
    public override void Enter(Bob agent)
    {
        atBank = false;
        index=0;

        GameObject bank = GameObject.Find("Bank");
        agent.toLoc = new Vector2(bank.transform.position.x, bank.transform.position.z);
      
        agent.path = agent.aStar();

        agent.waitedTime = 0;
        agent.Arrive(eAgent.Bob, eLocation.other);

        agent.CreateSpeechBubble("Goin' to the bank. Yes siree");
    }

   
    public override void Execute(Bob agent)
    {
        if (agent.path.Count == index )
        {
            agent.IncreaseWaitedTime(1);

            if (!atBank)
            {
                GameObject bank = GameObject.Find(eLocation.Bank.ToString());
                bank.GetComponent<Location>().agents.Add((int)eAgent.Bob);

                Bank bankScript = bank.GetComponent<Bank>();
                bankScript.Deposite(agent.id, agent.currentGold);

                agent.CreateSpeechBubble("Depositin’ gold. Total savings now: " + bankScript.CheckBalance(agent.id));
                agent.currentGold = 0;

                atBank = true;
            }

            if (agent.WaitedLongEnough()) 
                agent.ChangeState(GoMineState.Instance);
            
       }
        else
        {
            Point newPos = agent.path[index];
            index++;
            agent.pos = new Vector2(newPos.x,newPos.y);
          
        }
    }

    public override void Exit(Bob agent)
    {
        GameObject bank = GameObject.Find(eLocation.Bank.ToString());
        bank.GetComponent<Location>().agents.Remove((int)eAgent.Bob);
        agent.CreateSpeechBubble("Leavin' the bank");
    }
}

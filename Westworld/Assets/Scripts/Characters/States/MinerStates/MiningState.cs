using UnityEngine;
using System.Collections;

public class MiningState : State<Bob> {

    static readonly MiningState instance = new MiningState();

    public static MiningState Instance
    {
        get
        {
            return instance;
        }
    }

    static MiningState() { }
    private MiningState() { }

    public override void Enter(Bob agent)
    {
        //agent.Arrive(eAgent.Bob, eLocation.Mine);

        agent.waitedTime = 0;

        GameObject mine = GameObject.Find(eLocation.Mine.ToString());
        mine.GetComponent<Location>().agents.Add((int)eAgent.Bob);

        agent.CreateSpeechBubble("Arrived at the gold mine...");
    }

    public override void Execute(Bob agent)
    {

        agent.IncreaseWaitedTime(1);

        agent.CreateSpeechBubble("pickin' up a nugget");
        agent.currentGold++;


        if (agent.WaitedLongEnough())
        {
            int action = Random.Range(0, 2);
            switch (action)
            {
                case 0:
                    agent.ChangeState(GoBankState.Instance);
                    break;
                case 1:
                    agent.ChangeState(GoHomeState.Instance);
                    break;
            }
        }
    }

    public override void Exit(Bob agent)
    {
        GameObject mine = GameObject.Find(eLocation.Mine.ToString());
        mine.GetComponent<Location>().agents.Remove((int)eAgent.Bob);
        agent.CreateSpeechBubble("Ah'm leavin' the gold mine with mah pockets full o' sweet gold");
    }
}

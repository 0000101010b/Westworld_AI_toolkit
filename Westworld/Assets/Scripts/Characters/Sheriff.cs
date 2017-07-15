using System;
using UnityEngine;

public class Sheriff : Agent
{

    public TextMesh text;
    public scaleSpeech ss;

    private StateMachine<Sheriff> stateMachine;

    //global states
    public int thirst;

    public int loc = (int)eLocation.Mine;
    public int currentGold = 0;

    public int[] vLoc;

    public static int WAIT_TIME = 20;
    public int waitedTime = 0;
    public int createdTime = 0;

    public override void Awake()
    {
        vLoc = new int[]{
        (int)eLocation.Shack,
        (int)eLocation.Bank,
        (int)eLocation.Mine,
        (int)eLocation.Cemetery,
        (int)eLocation.Saloon,
        (int)eLocation.SheriffsOffice};

        GameObject g = GameObject.Find(eLocation.SheriffsOffice.ToString());
    
        pos.x = g.transform.position.x;
        pos.y = g.transform.position.z;
        g.GetComponent<Location>().agents.Add((int)eAgent.Sheriff);


        transform.position = new Vector3(pos.x, 2.0f, pos.y);
       
        this.stateMachine = new StateMachine<Sheriff>();
        this.stateMachine.Init(this, RandomPatrolState.Instance,SheriffGlobalState.Instance);
    }

    public void IncreaseWaitedTime(int amount)
    {
        this.waitedTime += amount;
    }

    public bool WaitedLongEnough()
    {
        return this.waitedTime >= WAIT_TIME;
    }

    public void CreateTime()
    {
        this.createdTime++;
        this.waitedTime = 0;
    }

    public void ChangeState(State<Sheriff> state)
    {
        this.stateMachine.ChangeState(state);
    }

    public int count = 0;
    public override void Update()
    {
        if (count < 1)//movement and update
            count++;
        else
        {
            transform.position = new Vector3(pos.x, 1.0f, pos.y);
            this.stateMachine.Update();
            count = 0;
        }
    }

    public void CreateSpeechBubble(string str)
    {
        text.text = str;
        ss.FixSpeechBubble();
    }
}

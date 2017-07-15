using System;
using UnityEngine;

public class Bob : Agent {

    
    public delegate void arriveHome();
    public static event arriveHome onArrivedHome;

    public void ArrivingHome()
    {
        if (onArrivedHome != null)
        {
            onArrivedHome();
        }
    }

    public bool eatenStew;
    public void EatStew()
    {
        hunger = 0;
        eatenStew = true;
        CreateSpeechBubble("Great stew");
    }

    public void enableEat()
    {
        Elsa.onCookedStew += EatStew;
    }
    public void disableEat()
    {
        Elsa.onCookedStew -= EatStew;
    }

    public TextMesh text;
    public scaleSpeech ss;

    private StateMachine<Bob> stateMachine;

    //global states
    public int thirst;
    public int tired;
    public int hunger;

    public int loc = (int)eLocation.Mine;
    public int currentGold = 0;

    public bool goinHome=false;
    public bool goinSaloon = false;
    

    public static int WAIT_TIME = 20;
    public int waitedTime = 0;
    public int createdTime = 0;

    public override void Awake()
    {
        GameObject g = GameObject.Find(eLocation.Mine.ToString());

        pos.x = g.transform.position.x;
        pos.y = g.transform.position.z;

        transform.position = new Vector3(pos.x, 2.0f, pos.y);

        this.stateMachine = new StateMachine<Bob>();
        this.stateMachine.Init(this, MiningState.Instance,MinerGlobalState.Instance);
       
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

    public void ChangeState(State<Bob> state)
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

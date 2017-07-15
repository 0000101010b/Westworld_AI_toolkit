using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elsa : Agent
{
    public delegate void CookedStew();
    public static event CookedStew onCookedStew;

    public void Cooked()
    {
        if (onCookedStew != null)
        {
            onCookedStew();
        }
    }
    public TextMesh text;
    public scaleSpeech ss;

    private StateMachine<Elsa> stateMachine;
    public bool isStart;

    //global states
    public int thirst;
    public int tired;

    public int loc = (int)eLocation.Mine;
    public int currentGold = 0;

    public bool goinHome = false;
    public bool goinSaloon = false;


    public static int WAIT_TIME = 30;
    public int waitedTime = 0;
    public int createdTime = 0;

    public override void Awake()
    {
        isStart = true;
        GameObject g = GameObject.Find(eLocation.Shack.ToString());

        pos.x = g.transform.position.x;
        pos.y = g.transform.position.z;
        g.GetComponent<Location>().agents.Add((int)eAgent.Elsa);

        transform.position = new Vector3(pos.x, 2.0f, pos.y);
        this.stateMachine = new StateMachine<Elsa>();
        this.stateMachine.Init(this, HouseworkState.Instance, ElsaGlobalState.Instance);
    }


    public void startCooking()
    {
        ChangeState(CookingState.Instance);
    }

    public void enableCooking()
    {
        Bob.onArrivedHome += startCooking;
    }
    public void disableCooking()
    {
       Bob.onArrivedHome -= startCooking;
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

    public void ChangeState(State<Elsa> state)
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

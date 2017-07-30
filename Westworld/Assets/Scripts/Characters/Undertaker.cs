using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undertaker : Agent
{
    public TextMesh text;
    public scaleSpeech ss;

    private StateMachine<Undertaker> stateMachine;
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

    public GameObject deadBody;
    public bool pickedUpBody;

    public override void Awake()
    {
        pickedUpBody = false;

        isStart = true;
        GameObject g = GameObject.Find(eLocation.Undertakers.ToString());

        pos.x = g.transform.position.x;
        pos.y = g.transform.position.z;
        g.GetComponent<Location>().agents.Add((int)eAgent.Undertaker);

        transform.position = new Vector3(pos.x, 2.0f, pos.y);

        EnableCollectDead();

        this.stateMachine = new StateMachine<Undertaker>();
        this.stateMachine.Init(this, AtUndertakersState.Instance, UndertakerGlobalState.Instance);
    }

    void EnableCollectDead()
    {
      
        Agent.onDead += CollectDeadBody;
    }
    void DisableCollectDead()
    {
        Agent.onDead -= CollectDeadBody;
    }

    public void CollectDeadBody(GameObject _deadBody)
    {
        this.deadBody = _deadBody;
        this.ChangeState(CollectBodyState.Instance);
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

    public void ChangeState(State<Undertaker> state)
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

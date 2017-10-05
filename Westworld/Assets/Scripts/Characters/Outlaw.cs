using UnityEngine;
using System.Collections;


public class Outlaw : Agent
{

    public TextMesh text;
    public scaleSpeech ss;

    private StateMachine<Outlaw> stateMachine;


    public int loc = (int)eLocation.Mine;
    public int currentGold = 0;

    public static int WAIT_TIME = 20;
    public int waitedTime = 0;
    public int createdTime = 0;

    public override void Awake()
    {
        GameObject g = GameObject.Find(eLocation.OutlawCamp.ToString());

        pos.x = g.transform.position.x;
        pos.y = g.transform.position.z;
      

        transform.position = new Vector3(pos.x, 2.0f, pos.y);

        this.stateMachine = new StateMachine<Outlaw>();
        this.stateMachine.Init(this, LurkState.Instance,OutlawGlobalState.Instance);
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

    public void ChangeState(State<Outlaw> state)
    {
        this.stateMachine.ChangeState(state);
    }


    public float count = 0;
    public Vector3 newPos;
    public Vector3 oldPos;
    public override void Update()
    {
        if (count > 1)
        {
            transform.position = newPos;
            this.stateMachine.Update();
            oldPos = newPos;
            newPos = new Vector3(pos.x, 0.5f, pos.y);
            count = 0;
        }

        transform.position = Vector3.Lerp(oldPos, newPos, count);
        count += 0.1f;
    }

    /*
    public int count = 0;
    public override void Update()
    {
        if (count < 5)
            count++;
        else
        {
            transform.position = new Vector3(pos.x, 1.0f, pos.y);
            this.stateMachine.Update();
            count = 0;
        }
    }*/

    public void CreateSpeechBubble(string str)
    {
        text.text = str;
        ss.FixSpeechBubble();
    }
}

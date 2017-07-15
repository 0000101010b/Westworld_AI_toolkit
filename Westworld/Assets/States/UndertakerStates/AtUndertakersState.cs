using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtUndertakersState : State<Undertaker>
{
    static readonly AtUndertakersState instance = new AtUndertakersState();

    public static AtUndertakersState Instance
    {
        get
        {
            return instance;
        }
    }

    static AtUndertakersState() { }
    private AtUndertakersState() { }


    int index = 0;

    public override void Enter(Undertaker agent)
    {
        GameObject g = GameObject.Find(eLocation.Undertakers.ToString());
        g.GetComponent<Location>().agents.Add((int)eAgent.Undertaker);
        agent.CreateSpeechBubble("At' the Undertaker s");
    }

  
    public override void Execute(Undertaker agent)
    {
    
    }

    public override void Exit(Undertaker agent)
    {
        agent.CreateSpeechBubble("Leavin' the Undertaker s");

        GameObject g = GameObject.Find(eLocation.Undertakers.ToString());
        g.GetComponent<Location>().agents.Remove((int)eAgent.Undertaker);

    }
}
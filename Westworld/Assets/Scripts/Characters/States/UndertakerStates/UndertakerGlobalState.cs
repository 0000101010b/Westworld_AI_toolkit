using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndertakerGlobalState : State<Undertaker>
{
    static readonly UndertakerGlobalState instance = new UndertakerGlobalState();

    public static UndertakerGlobalState Instance
    {
        get
        {
            return instance;
        }
    }

    static UndertakerGlobalState() { }
    private UndertakerGlobalState() { }

    public override void Enter(Undertaker agent)
    {
        agent.thirst = 0;
    }


    public override void Execute(Undertaker agent)
    {

    }

    public override void Exit(Undertaker agent)
    {

    }
}

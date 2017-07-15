using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElsaGlobalState : State<Elsa>
{
    static readonly ElsaGlobalState instance = new ElsaGlobalState();

    public static ElsaGlobalState Instance
    {
        get
        {
            return instance;
        }
    }

    static ElsaGlobalState() { }
    private ElsaGlobalState() { }

    int index = 0;
    public override void Enter(Elsa agent)
    {

    }


    public override void Execute(Elsa agent)
    {

    }

    public override void Exit(Elsa agent)
    {

    }
}

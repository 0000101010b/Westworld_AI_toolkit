using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlawGlobalState : State<Outlaw>
{
    static readonly OutlawGlobalState instance = new OutlawGlobalState();

    public static OutlawGlobalState Instance
    {
        get
        {
            return instance;
        }
    }

    static OutlawGlobalState() { }
    private OutlawGlobalState() { }

    public override void Enter(Outlaw agent)
    {
     
    }


    public override void Execute(Outlaw agent)
    {
      

    }

    public override void Exit(Outlaw agent)
    {

    }
}

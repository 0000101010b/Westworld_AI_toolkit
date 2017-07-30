using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheriffGlobalState : State<Sheriff>
{
    static readonly SheriffGlobalState instance = new SheriffGlobalState();

    public static SheriffGlobalState Instance
    {
        get
        {
            return instance;
        }
    }

    static SheriffGlobalState() { }
    private SheriffGlobalState() { }

    public override void Enter(Sheriff agent)
    {
        //Sense properties
        agent.sightDist = 10.0f;
        //other agent properties
        agent.thirst = 0;
    }

    bool chaseOutlaw = false;
    public override void Execute(Sheriff agent)
    {
        
        if (CheckForOutlaw(agent))
            agent.ChangeState(RandomPatrolState.Instance);//change to chase *not implemented
        else
            chaseOutlaw = false;


    }

    public bool CheckForOutlaw(Sheriff agent)
    {
        RaycastHit[][] hits = agent.Sight();

        for (int i = 0; i < hits.Length; i++)
        {
            for (int j = 0;j<hits[i].Length; j++)
            {
                
                RaycastHit hit = hits[i][j];
                if (hit.transform.gameObject.name == eAgent.OutlawJesse.ToString()+"(Clone)")
                {
                    agent.CreateSpeechBubble("I See You !!!!!!!Jesse James...");
                    Debug.Log("I See You !!!!!!! Jesse James..." );

                }
                Debug.Log("I See " + hit.transform.gameObject.name);
            }
        }
        
        return false;
    }


    public override void Exit(Sheriff agent)
    {

    }
}

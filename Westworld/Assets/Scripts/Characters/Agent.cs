using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;



abstract public class Agent :MonoBehaviour {


    public delegate void dead(GameObject agent);
    public static event dead onDead;

    public void Dead()
    {
        if (onDead != null)
        {
            Debug.Log(this.gameObject);
            onDead(this.gameObject);
        }
    }
    //sense parameters
    public float sightDist;


    public int id;
    public bool pathFound;
    public Vector2 pos;
   
    abstract public void Awake();
	abstract public void Update ();

    public Vector2 toLoc;
    public List<Point> path;

    public delegate void ArrivedAt(eAgent agent,eLocation loc);
    public static event ArrivedAt onArrivedAt;

    public void Arrive(eAgent agent,eLocation loc)
    {
        if (onArrivedAt != null)
        {
            onArrivedAt(agent,loc);
        }
    }

    public RaycastHit[][] Sight()
    {
        Transform t = transform.gameObject.transform;
        Vector3 pos = t.position;
        RaycastHit[][] hits = new RaycastHit[4][];

        Debug.DrawRay(pos, -t.forward*10.0f);
        hits[0] = Physics.RaycastAll(pos, t.forward, sightDist);
        hits[1] = Physics.RaycastAll(pos, -t.forward, sightDist);
        hits[2] = Physics.RaycastAll(pos, -t.right, sightDist);
        hits[3] = Physics.RaycastAll(pos, t.right, sightDist);

        return hits;
    }


    public List<Point> aStar()
    {
        GameObject g=GameObject.Find("Westworld");

        WestWorld script = g.GetComponent<WestWorld>();
        int width = script.width;
        int height = script.height;
        bool[,] grid = script.grid;

        Point from = new Point(pos.x, pos.y);
        Point to = new Point(toLoc.x, toLoc.y);
        
        SearchParameters searchParameters = new SearchParameters(from,to,grid);

        PathFinder pathFinder = new PathFinder(searchParameters);
        
        return pathFinder.FindPath();
    }

  

}
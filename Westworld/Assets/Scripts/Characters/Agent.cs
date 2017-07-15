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
            onDead(this.gameObject);
        }
    }
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

    public List<Point> aStar()
    {
        GameObject g=GameObject.Find("Westworld");

        WestWorld1 script = g.GetComponent<WestWorld1>();
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
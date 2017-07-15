using UnityEngine;
using System.Collections.Generic;


public enum eLocation
{
    Shack,
    Bank,
    Mine,
    OutlawCamp,
    Cemetery,
    Saloon,
    SheriffsOffice,
    Undertakers,
    Mountain,
    other
}
public enum eAgent
{
    Bob,
    OutlawJesse,
    Elsa,
    Sheriff,
    Undertaker
}
enum eTile
{
    plains,
    mountains,
    shack
}




public class WestWorld1 : MonoBehaviour {

    public List<GameObject> locationPrefabs;
    public List<GameObject> agentPrefabs;
    public List<Vector2> loc;


    [Header ("Grid")]
    public bool[,] grid;

    [Header("Tile Color")]
    public List<Color> Colors;

    [Header("Tile Material")]
    public List<Material> tiles;

    List<Point> path;

    //map
    public int width;
    public int height;

    //ground
    public GameObject Prefab;
    private List<GameObject> clones = new List<GameObject>();
    private string name;
    public GameObject parent;


    // Use this for initialization
    void Start()
    {
        SetActiveEvents();

        SetUpGrid();

        loc = new List<Vector2>();

        //set locations
        for (int i = 0; i < (int)eLocation.Undertakers + 1; i++)
        {
            Vector2 v = new Vector2();
            do
            {
                v.x = Random.Range(0, 100);
                v.y = Random.Range(0, 75);
            } while (!grid[(int)v.x, (int)v.y] && !loc.Contains(v));
            
            loc.Add(v);
        }

        //Create locations
        GameObject locParent = new GameObject("locations");

        for (int i = 0; i < locationPrefabs.Count; i++)
        {
            GameObject g = Instantiate(locationPrefabs[i], new Vector3(loc[i].x, 1, loc[i].y), Quaternion.identity) as GameObject;
            g.transform.parent = locParent.transform;
            g.name = ((eLocation)i).ToString();
            g.AddComponent<Location>();
        }



        GameObject agentParent = new GameObject("Characters");

        //Create Agents
        for (int i = 0; i < agentPrefabs.Count; i++)
        {
            GameObject g = Instantiate(agentPrefabs[i], new Vector3( 0,1,0), Quaternion.identity) as GameObject;
            g.transform.parent = agentParent.transform;
            //g.GetComponent<Agent>().pos = loc[(int)agentLoc[i]];
        }
    }


    /*
     * Unused: working method for sending agent locations to westworld
     * */
    //List<eLocation> agentLoc;
    void SetActiveEvents()
    {
        //Agent.onArrivedAt += UpdateAgentLoc;
    }


    /*
    void UpdateAgentLoc(eAgent agent,eLocation loc)
    {
        agentLoc[(int)agent] = loc;
    }*/





    void SetUpGrid()
    {
        parent = new GameObject("Tiles");

        grid = new bool[width, height];

        #region Initialize and Spawn grid
        //Set offset position
        Vector3 start = transform.position;
        start.x = 0;
        start.y = 0;
        start.z = 0;

        //Initialize and Spawn Grid
        grid = new bool[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                SetMaterial(x, z);

                Vector3 pos = new Vector3(0, 0, 0);
                pos.x = x + start.x;
                pos.z = z + start.z;

                SpawnPrefab(pos);
            }
        }
        #endregion
    }


    private void SetMaterial(int x, int y)
    {
        MeshRenderer gRenderer = Prefab.GetComponent<MeshRenderer>();

        float walkable = Random.Range(0.0f, 1.0f);

        Material m;
        if (walkable < 0.98f)
        {
            m = tiles[0];
            name = "walk";
            grid[x, y] = true;
        }
        else
        {
            grid[x, y] = false;
            m = tiles[1];
            name = "unwalkable";
        }
        gRenderer.material = m;

    }

    public void SpawnPrefab(Vector3 pos)
    {
        var clone = Lean.LeanPool.Spawn(Prefab, pos, Prefab.transform.rotation, null);
        clone.name = name;

        clone.transform.parent = parent.transform;
        clones.Add(clone);
    }

    public void DespawnPrefab()
    {
        if (clones.Count > 0)
        {
            // Get the last clone
            var index = clones.Count - 1;
            var clone = clones[index];

            // Remove it
            clones.RemoveAt(index);

            // Despawn it
            Lean.LeanPool.Despawn(clone);
        }
    }
    void OnDrawGizmos()
    {
        if (path != null)
        {
            Debug.Log(path.Count + " Drawing path");
            foreach (var point in path)
            {
                Gizmos.DrawWireSphere(new Vector3(point.x, 0, point.y), 0.5f);
                Gizmos.DrawWireSphere(new Vector3(point.x, 0, point.y), 0.5f);
            }
        }
    }

    void Update()
    {


        if (Input.GetKey(KeyCode.Space))
        {
            SearchParameters searchParameters = new SearchParameters(

                new Point(
                    Random.Range(0, width),
                    Random.Range(0, height)),

                new Point(
                    Random.Range(0, width),
                    Random.Range(0, height)),

                    grid

                    );

            PathFinder pathFinder = new PathFinder(searchParameters);

            path = pathFinder.FindPath();

        }
    }
}



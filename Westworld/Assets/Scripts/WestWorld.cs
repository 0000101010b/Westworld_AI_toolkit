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


public class WestWorld : MonoBehaviour {

    [Header("Locations")]
    public List<GameObject> locationPrefabs;

    [Header("Agents")]
    public List<GameObject> agentPrefabs;

    [Header ("Grid")]
    public bool[,] grid;
    public int width;
    public int height;

    [Header("Tiles")]
    public GameObject tilePrefab;
    public List<Color> tileColors;
    public List<Material> tileMaterials;
    private GameObject tiles;

    //leanpool
    private List<GameObject> clones = new List<GameObject>();
    private string name;
    //Debuging
    List<Point> path;

    // Use this for initialization
    void Start()
    {
        //SetActiveEvents();

        SetUpGrid();

        #region Create Locations
        List<Vector2>loc = new List<Vector2>();

        //set location location
        for (int i = 0; i < (int)eLocation.Undertakers + 1; i++)
        {
            Vector2 v = new Vector2();
            do
            {
                v.x = Random.Range(0, width);
                v.y = Random.Range(0, height);
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
        #endregion

        #region Create Characters
        GameObject agentParent = new GameObject("Characters");

        //Create Agents
        for (int i = 0; i < agentPrefabs.Count; i++)
        {
            GameObject g = Instantiate(agentPrefabs[i], new Vector3( 0,1,0), Quaternion.identity) as GameObject;
            g.transform.parent = agentParent.transform;
            //g.GetComponent<Agent>().pos = loc[(int)agentLoc[i]];
        }
        #endregion
    }


    /*
     * Unused: method for sending agent locations to westworld
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
        tiles = new GameObject("Tiles");

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
        MeshRenderer gRenderer = tilePrefab.GetComponent<MeshRenderer>();

        float walkable = Random.Range(0.0f, 1.0f);

        Material m;
        if (walkable < 0.98f)
        {
            m = tileMaterials[0];
            name = "walk";
            grid[x, y] = true;
        }
        else
        {
            grid[x, y] = false;
            m = tileMaterials[1];
            name = "unwalkable";
        }
        gRenderer.material = m;
    }

    #region leanpool
    public void SpawnPrefab(Vector3 pos)
    {
        var clone = Lean.LeanPool.Spawn(tilePrefab, pos, tilePrefab.transform.rotation, null);
        clone.name = name;

        clone.transform.parent = tiles.transform;
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
    #endregion

    #region A* Debuging
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
    #endregion
}

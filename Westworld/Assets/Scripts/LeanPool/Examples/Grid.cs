using UnityEngine;
using System.Collections.Generic;
using System.Linq;




// This script shows you how you can easily spawn and despawn a prefab
public class SimplePooling : MonoBehaviour
{
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

    void Awake()
    {
        parent = new GameObject("Tiles");

        grid = new bool[width, height];

        #region Initialize and Spawn grid
        //Set offset position
        Vector3 start = transform.position;
        start.x  = 0;
        start.y  = 0;
        start.z  = 0;

        //Initialize and Spawn Grid
        grid = new bool[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                SetMaterial(x, z);

                Vector3 pos = new Vector3(0, 0, 0);
                pos.x = x + start.x ;
                pos.z = z + start.z ;

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
            grid[x,y] = false;
            m = tiles[1];
            name = "unwalkable";
        }        
        gRenderer.material = m;
  
    }

	public void SpawnPrefab(Vector3 pos)
	{
        var clone    = Lean.LeanPool.Spawn(Prefab, pos, Prefab.transform.rotation, null);
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

            PathFinder pathFinder=new PathFinder(searchParameters);

            path=pathFinder.FindPath();
        
        }
    }
}
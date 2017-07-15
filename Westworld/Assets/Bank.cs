using UnityEngine;
using System.Collections.Generic;

public class Bank : MonoBehaviour {

    public Dictionary<int,int> records;

    public int totalGold = 0;


	// Use this for initialization
	void Start () {
        records = new Dictionary<int, int>();
	}
	
	// Update is called once per frame
	public void Deposite (int id,int gold) {
        if(records.ContainsKey(id))
        {
            records[id] += gold;
        }
        else
        {
            records.Add(id, gold);
        }

        totalGold = 0;
        foreach (KeyValuePair<int,int> entry in records)
        {
            totalGold += entry.Value;
        }
	}

    public int CheckBalance(int id)
    {
        if (records.ContainsKey(id))
        {
            return records[id];
        }
        else
        {
            return 0;
        }
    }


    public int Rob()
    {
        return Random.Range(1, 11);
    }
}

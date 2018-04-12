using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinAnimation : MonoBehaviour
{

    public GameObject ShardPrefab;
    List<WinShard> shardList;

    public RectTransform EndShard;


    int shardsLeft;
    public float increment = 0.5f;
    float Timer;

    public float Speed;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (shardsLeft > 0)
        {
            if (Timer > increment)
            {
                Timer = 0;
                GameObject shard = Instantiate(ShardPrefab, transform.position, transform.rotation, this.transform);

                WinShard winShard = shard.GetComponent<WinShard>();
                winShard.FireShard(EndShard, Speed);
                shardList.Add(winShard);
                shardsLeft--;
            }
            else
            {
                Timer += Time.deltaTime;
            }
        }
    }


    internal void GiveShards(int number)
    {
        if (shardList != null)
        {
            if (shardList.Count > 0)
                shardList.Clear();
        }
        else
        {
            shardList = new List<WinShard>();
        }


        shardsLeft = number;
    }

}

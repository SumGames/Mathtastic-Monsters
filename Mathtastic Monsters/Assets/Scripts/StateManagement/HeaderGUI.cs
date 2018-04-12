using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeaderGUI : MonoBehaviour
{
    public Text playernameUI; // Shows the player thir name in the UI
    public Text shardsUI; //Shows the player the amount of shards they have in the UI
    public Text starsUI; // Shows the player the amount of stars they have in the UI

    equipmentList list;


    internal bool UINeedsUpdate;

    TalismanManager talismans;

    int shardCount;

    // Use this for initialization
    void Start()
    {        
    }

    private void Update()
    {
        if (!list)
        {
            list = FindObjectOfType<equipmentList>();
            UINeedsUpdate = true;
        }

        if (shardsUI && UINeedsUpdate)
        {
            if (list && list.equip != null)
            {
                UpdateUI(true);
            }
        }
    }

    internal void UpdateUI(bool andShards)
    {
        if (andShards)
            shardCount = list.equip.shards;

        shardsUI.text = list.getShards();
        starsUI.text = list.equip.GetTotalStars().ToString();
        playernameUI.text = list.playerName;
        UINeedsUpdate = false;

        if (!talismans)
            talismans = GetComponent<TalismanManager>();

        if (talismans)
        {
            talismans.SetStaticTalismans();

        }
    }

    internal void incrementShards()
    {
        shardCount++;
        shardsUI.text = shardCount.ToString();
    }
}
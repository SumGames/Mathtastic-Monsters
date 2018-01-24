using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeaderGUI : MonoBehaviour
{
    public Text playernameUI; // Shows the player thir name in the UI
    public Text shardsUI; //Shows the player the amount of shards they have in the UI
    public Text starsUI; // Shows the player the amount of stars they have in the UI
    public Image[] medals;

    equipmentList list;


    internal bool UINeedsUpdate;


    // Use this for initialization
    void Start () {
		
	}

    private void Update()
    {
        if(!list)
        {
            list = FindObjectOfType<equipmentList>();
        }

        if (shardsUI && UINeedsUpdate)
        {
            if (list && list.equip != null)
            {
                UpdateUI();
            }
        }
    }

    void UpdateUI()
    {
        shardsUI.text = list.getShards();
        starsUI.text = list.equip.GetTotalStars().ToString();
        playernameUI.text = list.playerName;
        UINeedsUpdate = false;

        for (int i = 0; i < 4; i++)
        {
            if (list.equip.completedLevels[i] > 9)
            {
                medals[i].color = new Color(1, 1, 1, 1);
            }
            else
            {
                medals[i].color = new Color(.2f, .2f, .2f, 1);
            }
        }
    }
}

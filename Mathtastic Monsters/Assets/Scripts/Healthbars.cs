using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Healthbars : MonoBehaviour
{
    float playerHealth;
    float enemyHealth;

    float playerFill;
    float enemyFill;

    public Slider playerBar;

    public Slider enemyBar;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (playerBar != null)
        {
            playerFill = Mathf.Lerp(playerFill, playerHealth, Time.deltaTime);
            playerBar.value = playerFill;
        }
        if (enemyBar != null)
        {
            enemyFill = Mathf.Lerp(enemyFill, enemyHealth, Time.deltaTime);
            enemyBar.value = enemyFill;
        }
    }



    public void setMaxHealth(float Max, bool player)
    {
        if (player)
        {
            playerBar.maxValue = playerHealth = playerFill = Max;           
        }
        else
        {
            enemyBar.maxValue = enemyHealth = enemyFill = Max;
        }


    }

    internal void changeHealth(bool player, float current)
    {
        if (player)
            playerHealth = current;
        else
            enemyHealth = current;

    }

}

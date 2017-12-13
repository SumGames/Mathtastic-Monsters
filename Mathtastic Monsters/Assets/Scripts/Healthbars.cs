using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Healthbars : MonoBehaviour
{
    float playerHealth; //what the character's health is actually at.
    float enemyHealth;

    float playerFill; //Where the player's health bar level is at.
    float enemyFill;

    public Slider playerBar;

    public Slider enemyBar;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    //For each bar it slowly empties out as they take damage, using the difference between the two to determine speed.
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



    //Called at the start of the a fight, setting the character's healthbar to max.
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

    //After a character is damaged, set health so we can lerp down to that level.
    internal void changeHealth(bool player, float current)
    {
        if (player)
            playerHealth = current;
        else
            enemyHealth = current;

    }

}

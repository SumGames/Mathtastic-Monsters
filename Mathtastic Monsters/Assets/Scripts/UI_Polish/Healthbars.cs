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

    public Slider SecondBar;
    float secondFill;
    float secondHealth;
    public Slider ThirdBar;
    float thirdFill;
    float thirdHealth;

    bool multiActive;

    public bool wait;

	// Use this for initialization
	void Start ()
    {
		
	}


    internal void skipRefill()
    {
        playerFill = playerHealth;
        enemyFill = enemyHealth;
        secondFill = secondHealth;
        thirdFill = thirdHealth;
    }


	
    // Update is called once per frame
    //For each bar it slowly empties out as they take damage, using the difference between the two to determine speed.
	void Update ()
    {
        if (wait)
            return;

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

        if(multiActive)
        {
            if (SecondBar != null)
            {
                secondFill = Mathf.Lerp(secondFill, secondHealth, Time.deltaTime);
                SecondBar.value = secondFill;
            }
            if (ThirdBar != null)
            {
                thirdFill = Mathf.Lerp(thirdFill, thirdHealth, Time.deltaTime);
                ThirdBar.value = thirdFill;
            }
        }

    }



    //Called at the start of the a fight, setting the character's healthbar to max.
    public void setMaxHealth(float Max, bool player, bool Three = false)
    {
        wait = true;

        if (Three)
        {
            Debug.Log("Start");

            multiActive = true;

            SecondBar.gameObject.SetActive(true);

            ThirdBar.gameObject.SetActive(true);
            enemyBar.maxValue = enemyHealth = 4;
            SecondBar.maxValue = secondHealth = 4;
            ThirdBar.maxValue = secondHealth = 4;

            return;
        }
        if (player)
        {
            playerBar.maxValue = playerHealth = Max;
        }
        else
        {
            multiActive = false;

            enemyBar.maxValue = enemyHealth = Max;
            if (SecondBar)
            {
                SecondBar.gameObject.SetActive(false);
                ThirdBar.gameObject.SetActive(false);
            }
        }
    }

    //After a character is damaged, set health so we can lerp down to that level.
    internal void changeHealth(bool player, float current)
    {

        Debug.Log(current);

        if (player)
            playerHealth = current;
        else
            enemyHealth = current;

    }
    internal void SetEnemyBars(int index, float health)
    {
        switch (index)
        {
            case 3:
                thirdHealth = health;
                break;
            case 2:
                secondHealth = health;
                break;
            case 1:
                enemyHealth = health;
                break;
            default:
                break;
        }

    }

}

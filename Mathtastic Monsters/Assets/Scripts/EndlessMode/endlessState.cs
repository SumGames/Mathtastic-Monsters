using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class endlessState : ParentsStateManager
{
    public GameObject homeScreen; //Scene found at startup.

    public GameObject startScreen; //State after clicking on subject.


    public GameObject continueScreen;

    public GameObject lossScreen;

    public GameObject battleScreen;


    public endlessMonsterManager monsterManager;

    public GameObject leaderboard;


    public Player player;
    public EndlessMonster enemy;

    public Button[] buttons;
    public Button additionTwo;

    // Use this for initialization
    void Start()
    {
        Find();

        changeState(playStatus.ArenaHome);

        backgrounds.startBack(playStatus.subjectSelect);

        CheckAvailable();
    }

    //Change the game's state, closing/opening containers and changing text.
    public override void changeState(playStatus newState)
    {
        base.changeState(newState);

        disableObjects();

        gameState = newState;

        monsterManager.DisplayScore();

        switch (gameState)
        {
            case playStatus.ArenaHome:
                homeScreen.SetActive(true);
                break;
            case playStatus.ArenaStart:
                startScreen.SetActive(true);
                break;
            case playStatus.ArenaCombat:
                battleScreen.SetActive(true);
                break;
            case playStatus.ArenaContinue:
                monsterManager.PlayerWon();
                continueScreen.SetActive(true);
                break;
            case playStatus.ArenaLost:
                lossScreen.SetActive(true);
                monsterManager.PlayerLost();
                break;
            case playStatus.subjectSelect:
                SceneManager.LoadScene(1);
                break;

            case playStatus.ArenaLeaderBoard:
                leaderboard.SetActive(true);
                break;
            default:
                break;
        }

    }

    //Disables all objects by default so it doesn't have to be done manually.
    void disableObjects()
    {
        homeScreen.SetActive(false);
        startScreen.SetActive(false);
        continueScreen.SetActive(false);
        lossScreen.SetActive(false);
        battleScreen.SetActive(false);
        leaderboard.SetActive(false);
    }

    public void EndlessBack()
    {
        if (gameState == playStatus.ArenaHome)
        {
            changeState(playStatus.subjectSelect);
        }
        else if (gameState == playStatus.ArenaCombat)
        {
            changeState(playStatus.ArenaLost);
        }
        else
        {
            changeState(playStatus.ArenaHome);
        }
    }


    void CheckAvailable()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            bool activate = list.equip.completedLevels[i] >= 5;
            buttons[i].interactable = activate;
        }
        additionTwo.interactable = buttons[0].interactable;
    }

    public void Unlock()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }
        additionTwo.interactable = true;
    }
}
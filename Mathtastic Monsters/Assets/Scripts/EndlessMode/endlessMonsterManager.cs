using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endlessMonsterManager : MonsterManager
{
    float Modifier;
    float Score;
    int levels;

    public endlessState endlessState;

    public Text Welcome;

    public endlessButton running;

    public Text currentScoreText;


    public GameObject[] modifierButtons;
    public GameObject[] spots;
    GameObject[] buttons;


    // Use this for initialization
    void Start()
    {
        buttons = new GameObject[3];

        player = endlessState.player;
        enemy = endlessState.enemy;

        player.enemy = enemy;
        player.parent = this;
        enemy.player = player;
        enemy.manager = endlessState;
        enemy.parent = this;

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ToSubjectScreen(endlessButton button)
    {
        Welcome.text = SubjectTitle();
        endlessState.changeState(playStatus.ArenaStart);

    }


    public string SubjectTitle()
    {
        return "Welcome to the " + quizRunning.Operator + " Arena!";
    }

    public void newGame()
    {

        running.resetToBasic();
        Modifier = 10;
        Score = 0;
        levels = 1;
        NextLevel(null);
    }


    internal void NextLevel(EndlessModifierButton button = null)
    {
        if (button != null)
        {
            Modifier += button.modifierChange;
            running.BoostStats(button);
            levels++;

            foreach (GameObject item in buttons)
            {
                Destroy(item);
            }
        }

        endlessState.changeState(playStatus.ArenaCombat);

        enemy.loadMonster();

        player.ResetPlayer();
    }

    internal void PlayerWon()
    {
        Score += Modifier;

        for (int i = 0; i < 3; i++)
        {
            int rand = Random.Range(0, modifierButtons.Length);
            buttons[i] = Instantiate(modifierButtons[rand], spots[i].transform, false);
            buttons[i].transform.localScale = new Vector3(1, 1, 1);

        }
        DisplayScore();
    }

    internal void PlayerLost()
    {

    }

    public void DisplayScore()
    {

        switch (endlessState.getGameState())
        {
            case playStatus.ArenaHome:
                currentScoreText.text = "";
                break;
            case playStatus.ArenaStart:
                currentScoreText.text = "";
                break;
            case playStatus.ArenaCombat:
                Welcome.text = "Level " + levels.ToString() + " of " + running.Operator.ToString();
                currentScoreText.text = "";
                break;
            case playStatus.ArenaContinue:
                Welcome.text = "You reached level " + levels.ToString() + " of " + quizRunning.Operator + " Arena!";

                currentScoreText.text = "Currently running: " + running.Operator;
                currentScoreText.text += "\nLevel : " + levels.ToString() + ". Score: " + Score.ToString();
                currentScoreText.text += "\nMultiplier: " + Modifier;
                break;
            case playStatus.ArenaLost:
                Welcome.text = "Your run of " + quizRunning.Operator + " Arena is over...";

                currentScoreText.text = "This Run of.... " + running.Operator;
                currentScoreText.text += "\nLevel : " + levels.ToString() + ". Score: " + Score.ToString();
                currentScoreText.text += "\bMultiplier: " + Modifier;
                break;
            default:
                currentScoreText.text = "";
                break;
        }

    }

}

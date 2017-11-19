
using UnityEngine;
using UnityEngine.UI;


public class endlessMonsterManager : MonsterManager
{


    internal float Modifier;
    public float Score;
    public int levels;

    public int crit;
    public int counter;


    public int fightsBetweenBreaks = 1;
    int fightsSinceBreak = 0;
    bool skipHeal = false;

    public endlessState endlessState;

    public Text Welcome;

    public endlessButton running;

    public Text currentScoreText;


    public EndlessModifierButton[] modifierButtons;
    public GameObject[] spots;
    EndlessModifierButton[] buttons;

    bool[] removedLimbs;

    equipmentList list;

    public HighSaveScore highScores;




    public Text highestLevel;
    public Text highestScore;

    public Text lostText;


    // Use this for initialization
    void Start()
    {

        list = FindObjectOfType<equipmentList>();

        removedLimbs = new bool[6];

        buttons = new EndlessModifierButton[3];

        player = endlessState.player;
        enemy = endlessState.enemy;

        player.enemy = enemy;
        player.parent = this;
        enemy.player = player;
        enemy.manager = endlessState;
        enemy.parent = this;


        highScores = GetComponent<HighSaveScore>();


        highScores.setHighZero();


        highScores.Load();
                    


    }

    // Update is called once per frame
    void Update()
    {
    }


    public void ToSubjectScreen(endlessButton button)
    {
        endlessState.changeState(playStatus.ArenaStart);

        highestScore.text = highScores.returnBest(button.Operator, true);
        highestLevel.text = highScores.returnBest(button.Operator, false);


    }

    public void newGame()
    {
        for (int i = 0; i < 6; i++)
        {
            removedLimbs[i] = false;
        }

        running.resetToBasic();
        Modifier = 10;
        Score = 0;
        levels = 1;
        NextLevel(null);

        fightsBetweenBreaks = 1;
        fightsSinceBreak = 0;
    }


    internal void NextLevel(EndlessModifierButton button = null)
    {
        if (button != null)
        {
            Modifier += button.modifierChange;
            running.BoostStats(button);

            foreach (EndlessModifierButton item in buttons)
            {
                Destroy(item.gameObject);
            }
        }

        endlessState.changeState(playStatus.ArenaCombat);

        enemy.loadMonster();

        RemoveLimbs();

        if(skipHeal)
        {
            skipHeal = false;
            return;
        }

        player.ResetPlayer();
    }

    internal void PlayerWon()
    {
        levels++;
        Score += Modifier;
        Score += (crit * .5f);
        Score += (counter * .25f);


        crit = 0;
        counter = 0;

        fightsSinceBreak++;
        if (fightsSinceBreak < fightsBetweenBreaks)
        {
            skipHeal = true;
            NextLevel(null);
            return;
        }
        else
        {
            fightsSinceBreak = 0;
        }

        int[] locked = new int[3];

        for (int i = 0; i < 3; i++)
        {
            EndlessModifierButton button = null;

            while (button == null)
            {
                int rand = Random.Range(0, modifierButtons.Length);
                EndlessModifierButton modifierButton = modifierButtons[rand];

                if (!modifierButton.locked)
                {
                    button = modifierButtons[rand];
                    button.locked = true;
                    locked[i] = rand;
                }

            }
            buttons[i] = Instantiate(button, spots[i].transform, false);
            buttons[i].transform.localScale = new Vector3(1, 1, 1);

        }
        for (int i = 0; i < 3; i++)
        {
            modifierButtons[locked[i]].locked = false;
        }

        DisplayScore();
    }

    public void quitButton()
    {
        endlessState.changeState(playStatus.Lost);        
    }

    internal void PlayerLost()
    {
        Score += (crit * .5f);
        Score += (counter * .25f);
        crit = 0;
        counter = 0;


        Welcome.text = "Your run of " + quizRunning.Operator + " Arena is over...";
        int highScore = highScores.checkLevel(running.Operator, levels, Score, list.playerName);
        int highLevel = highScores.checkScore(running.Operator, Score, levels, list.playerName);


        save();


        if (highScore == 0 && highLevel == 0)
        {
            lostText.text = "You reached the Highest Level AND score! WOW!";
        }
        else if(highScore==0)
        {
            lostText.text = "You got the highest Score. Grats!";

        }
        else if(highLevel==0)
        {
            lostText.text = "You got the furthest in. Grats!";
        }
        else if (highScore >= 0 || highLevel >= 0)
        {
            lostText.text = "You're on the leaderboard!";
        }
        else
        {
            lostText.text = "Keep trying. You'll do better next time!";
        }
    }

    void RemoveLimbs()
    {
        list = FindObjectOfType<equipmentList>();


        for (int i = 0; i < 6; i++)
        {
            if (removedLimbs[i])
            {
                list.ChangeEquip(null, (partType)3, -1);
            }
        }

    }

    public void DisplayScore()
    {

        switch (endlessState.getGameState())
        {
            case playStatus.ArenaHome:
                if (quizRunning != null)
                    quizRunning = null;
                Welcome.text = "Welcome to the Endless Arena!!";
                currentScoreText.text = "";
                break;
            case playStatus.ArenaStart:
                Welcome.text = "Welcome to the " + quizRunning.Operator + " Arena!";
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
                currentScoreText.text = "This Run of.... " + running.Operator;
                currentScoreText.text += "\nLevel : " + levels.ToString() + ". Score: " + Score.ToString();
                currentScoreText.text += "\bMultiplier: " + Modifier;
                break;

            case playStatus.ArenaLeaderBoard:
                Welcome.text = "The Endless Arena Leaderboard!";

                break;
            default:
                currentScoreText.text = "";
                break;
        }
    }

    internal void OtherModifiers(modifierType mod, float intensity)
    {
        switch (mod)
        {
            case modifierType.LessBreaks:
                fightsBetweenBreaks += (int)intensity;
                break;
            case modifierType.RemoveLimb:
                removedLimbs[(int)intensity] = true;
                break;
            default:
                break;
        }
    }

    public void save()
    {
        list = FindObjectOfType<equipmentList>();
        highScores.Save(list.playerName);
    }


}

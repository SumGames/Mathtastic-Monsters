
using UnityEngine;
using UnityEngine.UI;


public class endlessMonsterManager : MonsterManager
{


    internal float Modifier; //Points we're earning every round.
    public float Score; //Total Points we've earned
    public int levels; //How many monsters we've beaten.


    public int fightsBetweenBreaks = 1; //We usually stop after every fight, but a mod will skip that phase
    int fightsSinceBreak = 0;
    bool skipHeal = false; //if we aren't taking a break, we aren't getting healed.

    public endlessState endlessState;

    public Text Welcome; //Changes based off state.

    public endlessButton running;

    public Text currentScoreText;


    public EndlessModifierButton[] modifierButtons; //A list of potential buttons that can appear.
    public GameObject[] spots; //Places for buttons.
    EndlessModifierButton[] buttons; //Buttons that have appeared.

    public bool[] removedLimbs; //Parts we can't use anymore.

    equipmentList list;

    public HighSaveScore highScores;




    public Text highestLevel;
    public Text highestScore;

    public Text lostText;

    public playerAbilities m_playerAbilities;


    public override void Start()
    {

        list = FindObjectOfType<equipmentList>();

        removedLimbs = new bool[6];

        buttons = new EndlessModifierButton[3];

        player = endlessState.player;
        monster = endlessState.enemy;

        player.enemy = monster;
        player.parent = this;
        monster.player = player;
        monster.manager = endlessState;
        monster.parent = this;


        highScores = GetComponent<HighSaveScore>();


        highScores.setHighZero();


        highScores.Load();

        currentEnemy = monster;

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


    //Reset all stats to initial.
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

    //Start the next level in the arena.
    //If the button wasn't null, it's because we started a new level by giving ourselves a penalty.
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

        RemoveLimbs();

        if(skipHeal)
        {
            monster.loadMonster();
            skipHeal = false;
            return;
        }

        player.ResetPlayer(false);
    }

    internal void PlayerWon()
    {       
        levels++;
        Score += (Modifier * m_playerAbilities.ReturnExpBoost());

        fightsSinceBreak++;
        if (fightsSinceBreak < fightsBetweenBreaks) //Not picking a button or healing. We're just starting the next round now!
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

        for (int i = 0; i < 3; i++) //Run through the buttons, picking out three that aren't locked.
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
        for (int i = 0; i < 3; i++) //Unlock all buttons.
        {
            modifierButtons[locked[i]].locked = false;
        }

        DisplayScore();
    }

    public void quitButton() //Surrender and go to lost screen.
    {
        endlessState.changeState(playStatus.Lost);        
    }

    internal void PlayerLost() //Player lost. Now we tell them how they did, and if they got any high scores.
    {
        playerAbilities abilities = player.GetComponent<playerAbilities>();

        Score += (Modifier * abilities.ReturnExpBoost());


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

    //If we bought the penalties, we're going to lose our limbs.
    void RemoveLimbs()
    {
        list = FindObjectOfType<equipmentList>();

        for (int i = 0; i < 6; i++)
        {
            if (removedLimbs[i])
            {
                list.ChangeEquip(null, (partType)i, -1);
            }
        }
    }
    //Changes the text as we change states.
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
                currentScoreText.text = "You scored: " + running.Operator;
                currentScoreText.text += "\nLevel : " + levels.ToString() + ". Score: " + Score.ToString();
                currentScoreText.text += "\nMultiplier: " + Modifier;
                break;

            case playStatus.ArenaLeaderBoard:
                Welcome.text = "The Endless Arena Leaderboard!";

                break;
            default:
                currentScoreText.text = "";
                break;
        }
    }

    //For non-standard mmodifiers, they're passed to this class and funcion.
    internal void OtherModifiers(modifierType mod, int intensity)
    {
        switch (mod)
        {
            case modifierType.LessBreaks:
                fightsBetweenBreaks += intensity;
                break;
            case modifierType.RemoveLimb:
                removedLimbs[intensity] = true;
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

public class endlessButton : QuizButton
{
    public endlessMonsterManager endlessMonster;


    // Use this for initialization
    public override void Start ()
    {
		
	}
	
	// Update is called once per frame
	public override void Update ()
    {
		
	}

    public override void buttonUsed()
    {
        endlessMonster.running = this;
        endlessMonster.quizRunning = this;

        endlessMonster.ToSubjectScreen(this);
    }


    public void resetToBasic()
    {
        MonsterHealth = 6;
        MonsterAttack = 1;
        levelTime = 5;
        enemPhaseTime = 10;

        minNumber = 1;
        maxNumber = 6;

        minAnswer = 1;
        maxAnswer = 5;
    }

    internal void BoostStats(EndlessModifierButton a_button)
    {
        
        maxNumber += 5;
        minAnswer += 4;
        maxAnswer += 9;

        parseModifier(a_button.modOne, a_button.modOneIntensity);
        parseModifier(a_button.modTwo, a_button.modTwoIntensity);


    }

    public void parseModifier(modifierType type, float intensity)
    {
        switch (type)
        {
            case modifierType.none:
                return;
            case modifierType.monsterHealth:
                MonsterHealth += (int)intensity;
                break;
            case modifierType.monsterAttack:
                MonsterAttack += (int)intensity;
                break;
            case modifierType.AttackTime:
                levelTime += intensity;
                break;
            case modifierType.counterTime:
                enemPhaseTime += (int)intensity;
                break;
            case modifierType.boostGenerated:
                minNumber = maxNumber * (0.1f * intensity);
                break;
            default:
                break;
        }
    }
}
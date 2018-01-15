public class endlessButton : QuizButton
{
    public endlessMonsterManager endlessMonster;

    public void Used()
    {
        endlessMonster.running = this;
        endlessMonster.quizRunning = this;

        endlessMonster.ToSubjectScreen(this);
    }

    //Starting from scratch. enemy has basic stats.
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

        preventRounding = false;

        secondNumberMin = 0;
        secondNumberMax = 1;

        enemyChoices = 3;
        enemyAnswerRange = 2;
    }

    //Enemy is going to get stronger now.
    internal void BoostStats(EndlessModifierButton a_button)
    {
        
        maxNumber += 5;
        minAnswer += 4;
        maxAnswer += 9;

        parseModifier(a_button.modOne, a_button.modOneIntensity);
        parseModifier(a_button.modTwo, a_button.modTwoIntensity);


    }

    //Called in from a button we clicked on.
    //Will take one stat, and affect it, but give us more points in future.
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
            case modifierType.YourAttackTime:
                levelTime += intensity;
                break;
            case modifierType.MonsterAttackTime:
                enemPhaseTime += (int)intensity;
                break;
            case modifierType.numberofCounterAnswers:
                enemyChoices++;
                break;
            case modifierType.difficultyJump:
                maxNumber += 5 * intensity;
                minAnswer += 4 * (int)intensity;
                maxAnswer += 9 * (int)intensity;
                break;

            case modifierType.RemoveLimb:
                endlessMonster.OtherModifiers(type, intensity);
                break;
            case modifierType.LessBreaks:
                endlessMonster.OtherModifiers(type, intensity);
                break;
            default:
                break;
        }
    }
}
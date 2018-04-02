using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionContainer : MonoBehaviour
{
    public int enemyAnswerNeeded; //What answer is correct.

    public AdditionAnswer[] answers; //Array of answers, specifically ordered so first elements are at start of list.

    public Player player;


    Dictionary<int, int> abilities; //A list of abilities with charges >1, and their charge count.

    List<int> answersList;

    internal void MultipleAnswers(BossMonster boss, BossButton a_running)
    {

        //Spacing prevents answers from being spread out at first, but slowly expands them as we continue.
        int spacing = boss.bossSpacing;
        if (spacing > answers.Length)//If we have a range that is over our threshold, we'll have to clamp it back down.
        {
            spacing = answers.Length;
        }


        DisableMultiple();//Turn off all buttons.




        enemyAnswerNeeded = boss.answerNeeded;

        player.SetTime(true, a_running.levelTime);

        foreach (AdditionAnswer item in answers) //Set answers to negative. Easier to find if they're inactive now.
        {
            item.SetAnswer(-1);
        }
        int index = Random.Range(0, spacing);


        //Our list of duplicates. If you're the same, you're invalid.
        answersList = new List<int>();

        //One Answer is set to be correct.
        answers[index].gameObject.SetActive(true);
        answers[index].SetAnswer(enemyAnswerNeeded);
        answersList.Add(enemyAnswerNeeded);

        for (int i = 1; i < a_running.enemyChoices; i++)
        {
            int wrongAnswer = -3;
            while (wrongAnswer < a_running.minNumber || CheckMultiple(a_running,wrongAnswer)) //Continue looping if we have a dupe, or the number is under 0, essentially.
            {
                int range = Random.Range(-a_running.enemyAnswerRange, a_running.enemyAnswerRange);

                wrongAnswer = enemyAnswerNeeded + range;
            }
            index = Random.Range(0, spacing);
            while (answers[index].GetAnswer() != -1) //continue looking for buttons until we find an empty one.
            {
                index = Random.Range(0, spacing);
            }
            answers[index].gameObject.SetActive(true);
            answers[index].SetAnswer(wrongAnswer);

        }
    }

    //Loop if we return true.
    bool CheckMultiple(QuizButton button, int result)
    {
        bool dupes=false;

        foreach (AdditionAnswer item in answers)
        {
            if (result == item.GetAnswer())
                dupes = true;
        }

        //No duplicates.
        if (dupes==false)
        {
            answersList.Add(result);
            return false;
        }

        //Duplicates, but too many to avoid getting more :(
        if (answersList.Count >= button.enemyAnswerRange * 2)
        {
            return false;
        }

        return true;
    }


    void DisableMultiple()
    {

        foreach (AdditionAnswer item in answers)
        {
            item.gameObject.SetActive(false);
        }
    }
}
//Builds a list from every button in the subject, then assigns attributes to them.

using UnityEngine;

public class questionContainer : MonoBehaviour
{
    public QuizButton[] buttons;

    public int completedQuestions;

    public classType type = classType.None; //Used for when we want to ask/tell the manager how many levels are completed.

    equipmentList list;

    bool alreadyTurnedOn;

    public LevelSelection levelSelection;

    public void Awaken()
    {
        if (!alreadyTurnedOn)
        {
            if (list == null)
            {
                list = FindObjectOfType<equipmentList>();
            }
            int i = 0;
            foreach (QuizButton item in buttons)
            {
                item.quizIndex = i;

                item.parent = this;
                i++;
            }
            alreadyTurnedOn = true;
        }
        checkCompleted();
        levelSelection.setContainer(this);
        gameObject.SetActive(false);
    }

    public void checkCompleted()
    {
        completedQuestions = list.equip.completedLevels[(int)type];
    }


    //Return number completed, when checking requirements.
    public int getCompleted()
    {
        return completedQuestions;
    }

    public void incrementCompleted()
    {
        completedQuestions++;
        list.equip.setCompletedLevels(completedQuestions, (int)type);
    }

}
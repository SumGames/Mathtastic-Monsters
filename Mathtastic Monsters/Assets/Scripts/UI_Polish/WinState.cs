using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinState : MonoBehaviour
{
    public Button[] stars;

    public Text shardsEarned;

    public MonsterManager monsterM;

    equipmentList list;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void End(int earned)
    {
        list = FindObjectOfType<equipmentList>();

        QuizButton button = monsterM.quizRunning;

        int subject;
        int index = button.quizIndex;


        if ((int)button.Operator > (int)classType.Calculi)
            subject = (int)classType.Calculi;
        else
            subject = (int)button.Operator;

        int starsUnlocked = list.equip.StarsAcquired[((subject * 10) + index)];



        if (button.boss)
        {
            foreach (Button item in stars)
            {
                item.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (Button item in stars)
            {
                item.gameObject.SetActive(true);
            }
        }

        stars[0].interactable = (starsUnlocked >= 1);
        stars[1].interactable = (starsUnlocked >= 2);
        stars[2].interactable = (starsUnlocked >= 3);


        if (shardsEarned)
            shardsEarned.text = earned.ToString();
    }
}
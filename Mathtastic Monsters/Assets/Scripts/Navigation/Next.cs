using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Next : MonoBehaviour
{
    MonsterManager parent;
    questionContainer container;
    QuizButton button;

    internal void ready()
    {
        if (parent == null)
            parent = FindObjectOfType<MonsterManager>();
        button = parent.quizRunning;
        container = button.parent;

        if (!button.Hard && button.quizIndex < (container.buttons.Length - 1))
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void go()
    {
        container.buttons[(button.quizIndex + 1)].buttonUsed(phases.previous);
    }
}
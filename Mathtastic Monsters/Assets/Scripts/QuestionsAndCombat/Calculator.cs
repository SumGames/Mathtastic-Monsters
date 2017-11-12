﻿using UnityEngine;
using UnityEngine.UI;
//using System.Collections;

public class Calculator : MonoBehaviour
{
    public Text input; //The textbox where charactera are added.

    public Monster monster; //Reference to the opponent. Used to call a check command.

    public ParentsStateManager manager;

    internal string answerNeeded; //The number, as a string, that a player must input.

    public Button ok;
    public Button cancel;
    public AudioSource[] sounds;
    public AudioSource attack;
    public AudioSource hurt;


    // Use this for initialization
    void Start()
    {
        sounds = GetComponents<AudioSource>();
        attack = sounds[0];
        hurt = sounds[1];

        if (manager == null)
            manager = GameObject.Find("Manager").GetComponent<StateManager>();

        if (monster == null)
            monster = GameObject.Find("Monster").GetComponent<Monster>();

        if (input == null)
            input = GetComponentInChildren<Text>();
        ok.interactable = cancel.interactable = false;
    }

    //Adds the input character to the input text, or calls a command.
    //a_name is the letter that will be added
    public void AddInput(string a_name)
    {
        if (!manager || !manager.isPlaying())
            return;

        switch (a_name)
        {
            case "Ok":
                checkAnswer(input.text);
                input.text = "";
                ok.interactable = cancel.interactable = false;
                break;

            case "Cancel":
                input.text = "";
                ok.interactable = cancel.interactable = false;
                break;

            default:
                input.text += a_name;
                ok.interactable = cancel.interactable = true;
                break;
        }
    }

    //This function is called hits the Ok button on their calculator.
    //It then either attacks the player or is attacked.
    internal void checkAnswer(string answer)
    {
        if (!manager.isPlaying())
        {
            return;
        }

        if (answer == answerNeeded)
        {
            monster.MonsterHurt();
 //           attack.Play();
        }
        else
        {
            monster.EnemyAttack();
 //           hurt.Play();
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
//using System.Collections;

public class Calculator : MonoBehaviour
{
    Text input; //The textbox where charactera are added.

    Monster monster; //Reference to the opponent. Used to call a check command.

    StateManager manager;

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

        manager = GameObject.Find("Manager").GetComponent<StateManager>();

        monster = GameObject.Find("Monster").GetComponent<Monster>();

        input = GetComponentInChildren<Text>();
        ok.interactable = cancel.interactable = false;
    }

    //Adds the input character to the input text, or calls a command.
    //a_name is the letter that will be added
    public void AddInput(string a_name)
    {
        if (!manager || manager.getGameState() != playStatus.playing)
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
        if (manager.getGameState() != playStatus.playing)
        {
            return;
        }

        if (answer == answerNeeded)
        {
            monster.MonsterHurt();
//            attack.Play();
        }
        else
        {
            monster.EnemyAttack();
//            hurt.Play();
        }
    }
}

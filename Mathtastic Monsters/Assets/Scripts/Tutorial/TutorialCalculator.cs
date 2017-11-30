using UnityEngine;
using UnityEngine.UI;

public class TutorialCalculator : MonoBehaviour
{
    Text input; //The textbox where charactera are added.

    public TutorialMonster monster; //Reference to the opponent. Used to call a check command.

    string answerNeeded; //The number, as a string, that a player must input.



    public Button ok;
    public Button cancel;


    public Button []buttonsAll;


    public Text text;

    public TutmultipleContainer tutmultiple;

    // Use this for initialization
    void Start()
    {

        input = GetComponentInChildren<Text>();
        ok.interactable = cancel.interactable = false;
    }

    //Adds the input character to the input text, or calls a command.
    //a_name is the letter that will be added
    public void AddInput(string a_name)
    {
        switch (a_name)
        {
            case "Ok":
                CheckAnswer(input.text);
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
    internal void CheckAnswer(string answer)
    {
        if (answer == answerNeeded)
        {
            monster.MonsterHurt();
        }
        else
        {
            monster.EnemyAttack();
        }
    }

    public void ButtonsActive(bool a_on)
    {
        tutmultiple.interactableButtons(a_on);

        foreach (Button item in buttonsAll)
        {
            item.interactable = a_on;
        }

    }

    public void SetAnswer(string answer, string question)
    {
        answerNeeded = answer;
        text.text = question;
    }
}

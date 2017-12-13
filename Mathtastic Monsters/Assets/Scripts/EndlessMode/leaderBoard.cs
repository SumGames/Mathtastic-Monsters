using UnityEngine;
using UnityEngine.UI;

public class leaderBoard : MonoBehaviour
{
    public Text bestBox; //Where we displ our scores.
    public Text buttonText; //Button where we pick levels or score.

    endlessMonsterManager monsterManager;



    bool showScores; //We either want to display our best scores, or our best levels.

    int currentOperator; //We have scoreboards for every operator type.    

    // Use this for initialization
    void Start ()
    {
        monsterManager = FindObjectOfType<endlessMonsterManager>();

        changeType();
        displayHighScores(0);
	}

    public void changeType() //Changing from showing levels to scores.
    {
        showScores = !showScores;
        displayHighScores(currentOperator);

        if(showScores)
        {
            buttonText.text = "Show High Levels";
        }
        else
        {
            buttonText.text = "Show High Scores";
        }
    }

    public void displayHighScores(int ops) //Displaying best after changing our operator.
    {
        currentOperator = ops;

        operators op = (operators)ops;


        bestBox.text = monsterManager.highScores.returnRanking(op, showScores);
    }       
}

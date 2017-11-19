using UnityEngine;
using UnityEngine.UI;

public class leaderBoard : MonoBehaviour
{
    public Text bestBox;

    endlessMonsterManager monsterManager;



    bool showScores;


    int currentOperator;

    public Text buttonText;

    // Use this for initialization
    void Start ()
    {
        monsterManager = FindObjectOfType<endlessMonsterManager>();

        changeType();
        displayHighScores(0);
	}

    public void changeType()
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

    public void displayHighScores(int ops)
    {
        currentOperator = ops;

        operators op = (operators)ops;


        bestBox.text = monsterManager.highScores.returnRanking(op, showScores);
    }       
}

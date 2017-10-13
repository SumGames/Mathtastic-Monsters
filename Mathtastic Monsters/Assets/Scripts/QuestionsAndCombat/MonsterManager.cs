//Main purpose is combat startup and storage of current level we're playing.
using UnityEngine;

public class MonsterManager : MonoBehaviour
{

    internal QuizButton quizRunning; //The current quiz that has been running. This object passes in number range and other question specific variables.

    internal StateManager stateManager;

    internal Player player;
    internal Monster enemy;

    void start()
    {        
        player = stateManager.player;
        enemy = stateManager.enemy;
    }

    //Called by clicking on a button. Creates questions and loads them.
    internal void startLevel(QuizButton a_button)
    {
        quizRunning = a_button; //The button selected will be used as the basis for calculation and creation.

        stateManager.changeState(playStatus.playing);

        enemy.loadMonster();

        player.ResetPlayer();

    }

    //Called by the retry button. Reloads questions in the scene so another game can be played.
    public void restartLevel()
    {
        if (quizRunning != null)
        {
            startLevel(quizRunning);
        }
    }
}
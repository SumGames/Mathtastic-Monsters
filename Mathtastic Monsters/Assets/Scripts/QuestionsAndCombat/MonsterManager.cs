//Main purpose is combat startup and storage of current level we're playing.
using UnityEngine;

public class MonsterManager : MonoBehaviour
{

    public QuizButton quizRunning; //The current quiz that has been running. This object passes in number range and other question specific variables.

    internal StateManager stateManager;

    internal Player player;
    public Monster enemy;
    public BossMonster boss;

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

        if (!quizRunning.boss)
        {

            enemy.loadMonster();
            boss.additionContainer.gameObject.SetActive(false);
            boss.DestroyMonster();
        }
        else
        {
            boss.player = player;
            boss.parent = this;
            boss.loadMonster();
            enemy.DestroyMonster();
        }


        player.ResetPlayer(quizRunning.boss);

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
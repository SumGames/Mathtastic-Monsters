//Main purpose is combat startup and storage of current level we're playing.
using UnityEngine;

public class MonsterManager : MonoBehaviour
{

    public QuizButton quizRunning; //The current quiz that has been running. This object passes in number range and other question specific variables.

    public StateManager stateManager;

    internal Player player;
    public Monster monster;
    public BossMonster boss;

    public bool fightingBoss;

    internal Monster currentEnemy;

    public Vector3 initialPosition;

    public virtual void Start()
    {
        player = stateManager.player;
        monster = stateManager.enemy;

        initialPosition = monster.transform.position;
    }

    //Called by clicking on a button. Creates questions and loads them.
    internal void StartLevel(QuizButton a_button)
    {
        monster.transform.position = initialPosition;

        quizRunning = a_button; //The button selected will be used as the basis for calculation and creation.

        stateManager.changeState(playStatus.playing);

        fightingBoss = quizRunning.boss;

        if (!quizRunning.boss)
        {
            currentEnemy = monster;

            boss.additionContainer.gameObject.SetActive(false);
            boss.DestroyMonster();
        }
        else
        {
            currentEnemy = boss;

            boss.player = player;
            boss.parent = this;

            monster.DestroyMonster();
        }


        player.ResetPlayer(quizRunning.boss);

    }

    //Called by the retry button. Reloads questions in the scene so another game can be played.
    public void restartLevel()
    {
        if (quizRunning != null)
        {
            StartLevel(quizRunning);
        }
    }
}
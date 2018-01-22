using UnityEngine;

public class BossButton : QuizButton
{
    StoryManager storyManager;

    public int maxDepth = -3; //The level the Subtraction boss will run to when hurt.


    public override void buttonUsed(phases phase)
    {
        p_manager = GameObject.Find("MonsterManager").GetComponent<MonsterManager>();


        if (!storyManager)
            storyManager = FindObjectOfType<StoryManager>();

        storyManager.StartTransition(this, phase);

        boss = true;
        p_manager.StartLevel(this);
    }
}

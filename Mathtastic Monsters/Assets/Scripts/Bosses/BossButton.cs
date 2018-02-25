using UnityEngine;

public class BossButton : QuizButton
{
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

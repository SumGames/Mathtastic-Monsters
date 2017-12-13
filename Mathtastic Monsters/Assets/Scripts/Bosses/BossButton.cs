using UnityEngine;

public class BossButton : QuizButton
{
    public int maxDepth = -3; //The level the Subtraction boss will run to when hurt.


    // Use this for initialization
    public override void Start()
    {
        p_manager = GameObject.Find("MonsterManager").GetComponent<MonsterManager>();
    }

    public override void buttonUsed()
    {
        boss = true;
        p_manager.StartLevel(this);
    }
}

using UnityEngine;

public class BossButton : QuizButton
{

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

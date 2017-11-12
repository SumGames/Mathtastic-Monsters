using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessMonster : Monster
{

	// Use this for initialization
	void Start () {
		
	}


    public override void CheckDeath()
    {
        if (health < 0)
        {
            manager.changeState(playStatus.ArenaContinue);
            return;
        }

        if (player.getPlayerHealth() <= 0)
        {
            manager.changeState(playStatus.ArenaLost);
        }
    }
}

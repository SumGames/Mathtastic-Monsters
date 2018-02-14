using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessMonster : Monster
{

    //The arena uses different states
    public override void CheckDeath(bool boss=false)
    {
        bar.changeHealth(false, health);

        if (health <= 0)
        {
            manager.changeState(playStatus.ArenaContinue);
            return;
        }

        if (player.GetPlayerHealth() <= 0)
        {
            manager.changeState(playStatus.ArenaLost);
        }
    }
}

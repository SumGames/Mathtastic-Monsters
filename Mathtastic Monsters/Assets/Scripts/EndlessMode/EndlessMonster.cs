using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessMonster : Monster
{


    public override void CheckDeath()
    {
        bar.changeHealth(false, health);

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

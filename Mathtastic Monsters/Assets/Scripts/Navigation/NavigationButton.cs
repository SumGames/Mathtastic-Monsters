using UnityEngine;

public class NavigationButton : MonoBehaviour
{
    public playStatus stateTarget; //Where we want the state to go to.


    ParentsStateManager manager; //Parent encompassing every stateManager type. Can use to move states.

    MusicManager musicManager;

    public void used()
    {
        if (!musicManager)
        {
            musicManager = FindObjectOfType<MusicManager>();
        }
        if (musicManager)
            musicManager.click.Play();

        if (manager == null)
        {
            manager = FindObjectOfType<ParentsStateManager>();
        }

        manager.changeState(stateTarget);
    }
}

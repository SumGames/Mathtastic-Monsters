using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationButton : MonoBehaviour
{
    public playStatus stateTarget; //Where we want the state to go to.


    ParentsStateManager manager; //Parent encompassing every stateManager type. Can use to move states.

    MusicManager musicManager;

    public GameObject optionsPrefab;

    public void OptionsMenu()
    {
        if (optionsPrefab)
        {
            Instantiate(optionsPrefab, FindObjectOfType<Canvas>().transform, false);
            return;
        }
        Debug.Log("Prefab not set");
    }


    public void startTutorial()
    {
        SceneManager.LoadScene(4);
    }


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


    public void OptionUsed()
    {
        if (!musicManager)
        {
            musicManager = FindObjectOfType<MusicManager>();
        }
        if (musicManager)
            musicManager.click.Play();
        FindObjectOfType<OptionsManager>().changeState(stateTarget);
    }

    public void Quit()
    {
        Application.Quit();
    }

}

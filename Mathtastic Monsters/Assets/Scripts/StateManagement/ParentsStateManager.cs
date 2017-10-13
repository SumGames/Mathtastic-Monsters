//Class every button inherits. Primary purpose is making changeState universal across all managers.
using UnityEngine;

public enum playStatus //Enumerator to prevent states being changed manually, and to keep them in order.
{
    Start,
    subjectSelect,
    Addition,
    Subtraction,
    Multiplication,
    Division,
    MathFortress,
    playing,
    Won,
    Lost,
    MyMonster,
    MonsterCustomisation,
    LillyHome,
    Options,
    Instructions,
    Parents,
    Credits,
    Login,
    Splash
}

public class ParentsStateManager : MonoBehaviour
{
    internal playStatus gameState; //A statemanager. Different objects are made active/inactive as this variable changes.

    public GameObject backPrefab;
    GameObject backs;

    internal backgroundManager backgrounds;

    MusicManager manager;

    public GameObject prefabbedList;
    internal equipmentList list;


    public void Find ()
    {
        list = FindObjectOfType<equipmentList>();
        if (list == null)
        {
            GameObject adding = Instantiate(prefabbedList, null, false);
            list = adding.GetComponent<equipmentList>();
            adding.name = "List";

            StateManager check = gameObject.GetComponent<StateManager>();

            if (!check)
                list.startGame("Guest", true);
        }

        GameObject can = GameObject.Find("Canvas");
        backs = Instantiate(backPrefab, can.transform, false);

        backgrounds = backs.GetComponent<backgroundManager>();
    }

    
    //Change the game's state, closing/opening containers and changing text.
    public virtual void changeState(playStatus newState)
    {
        backgrounds.changeBack(newState);

        if (manager == null)
        {
            manager = FindObjectOfType<MusicManager>();
            return;
        }
        manager.setMusic(newState);

    }

    public playStatus getGameState()
    {
        return gameState;
    }

    public void quitGame()
    {
        Application.Quit();
    }


}

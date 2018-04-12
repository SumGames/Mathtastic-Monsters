//Class every button inherits. Primary purpose is making changeState universal across all managers.
using UnityEngine;
using UnityEngine.UI;

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
    SoundControl,
    OtherOptions,
    Credits,
    Login,
    Splash,
    ArenaHome,
    ArenaStart,
    ArenaCombat,
    ArenaContinue,
    ArenaLost,
    ArenaLeaderBoard
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

    public HeaderGUI header;

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

    public bool isPlaying()
    {
        if (gameState == playStatus.playing || gameState == playStatus.ArenaCombat)
            return true;
        return false;
    }

    //Change the game's state, closing/opening containers and changing text.
    public virtual void changeState(playStatus newState)
    {

        if (header)
        {
            if (newState == playStatus.Won)
            {
                header.UpdateUI(false);
            }
            else
            {
                header.UINeedsUpdate = true;
            }

        }
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


    public void CheatMode(UnityEngine.UI.Button clickedOn)
    {
        if (list.playerName != "Guest")
        {
            Destroy(clickedOn.gameObject);
            return;
        }
        list.ByPassRestrictions();
        Destroy(clickedOn.gameObject);

        changeState(gameState);

        list.skipping = true;

    }


}

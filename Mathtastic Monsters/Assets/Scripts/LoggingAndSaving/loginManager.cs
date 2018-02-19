using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loginManager : MonoBehaviour
{
    //Three possible players. We'll store these as playerPrefs instead of a speerate file.
    string nameOne;
    string nameTwo;
    string nameThree;

    //Display our player names.
    public Text[] nameTags;


    public Text[] inputNames;


    //Display if our player's name is blank.
    public GameObject[] MakeProfile;
    //otherwise display this.
    public GameObject[] loadProfile;

    public equipmentList list;

    StateManager states; //Tell this to continue onto main menu.

    public GameObject tutorialPopUp;

    // Use this for initialization
    internal void Begin(StateManager a_state, equipmentList a_list)
    {
        states = a_state;
        list = a_list;

        nameOne = PlayerPrefs.GetString("NameOne", "");
        nameTwo = PlayerPrefs.GetString("NameTwo", "");
        nameThree = PlayerPrefs.GetString("NameThree", "");


        nameTags[0].text = nameOne;
        nameTags[1].text = nameTwo;
        nameTags[2].text = nameThree;
        inputNames[0].text = "";
        inputNames[1].text = "";
        inputNames[2].text = "";

        EnableProfiles();
    }


    //Display making or loading profiles based on if one already exists
    void EnableProfiles()
    {
        tutorialPopUp.SetActive(false);

        if (nameOne == "")
        {
            MakeProfile[0].SetActive(true);
            loadProfile[0].SetActive(false);
        }
        else
        {
            MakeProfile[0].SetActive(false);
            loadProfile[0].SetActive(true);
        }

        if (nameTwo == "")
        {
            MakeProfile[1].SetActive(true);
            loadProfile[1].SetActive(false);
        }
        else
        {
            MakeProfile[1].SetActive(false);
            loadProfile[1].SetActive(true);
        }

        if (nameThree == "")
        {
            MakeProfile[2].SetActive(true);
            loadProfile[2].SetActive(false);
        }
        else
        {
            MakeProfile[2].SetActive(false);
            loadProfile[2].SetActive(true);
        }
    }

    //Called from a button in the loginscreen, under MakeProfile. Builds a profile based on the name typed into the box.
    public void createFromLinked(int index)
    {
        if (index == 4)
        {
            list.startGame("Guest", true);
            ContinueToStartScreen();
            return;
        }

        bool lilly = String.Equals(inputNames[(index - 1)].text, "Lilly", StringComparison.OrdinalIgnoreCase);

        if (lilly)
        {
            SetNameUsingIndex(index, inputNames[(index - 1)].text);
            list.startGame("Lilly", true);
            ContinueToStartScreen();
            return;
        }

        if (inputNames[(index - 1)].text == "")
            return;

        SetNameUsingIndex(index, inputNames[(index - 1)].text);
        list.startGame(inputNames[(index - 1)].text, true);

        tutorialPopUp.SetActive(true);

        transform.parent.gameObject.SetActive(false);
    }

    //Called from clicking of the main button of LoadProfile. Loads the profile.
    public void loadLinked(int index)
    {
        string profileNname = LoadUsingIndex(index);
        list.startGame(profileNname, false);

        ContinueToStartScreen();
    }

    //Called from all start screens. Jump to the main menu.
    void ContinueToStartScreen()
    {
        SceneManager.LoadScene(1);

        states.changeState(playStatus.subjectSelect);
        EnableProfiles();
    }


    //Called from clicking of the main button of LoadProfile. Deletes the profile.
    public void deleteLinked(int index)
    {
        string delName = Application.persistentDataPath + "/" + LoadUsingIndex(index) + ".gd";

        if (File.Exists(delName))
        {
            File.Delete(delName);
        }
        SetNameUsingIndex(index, "");

        EnableProfiles();
    }

    //Use the index of the button clicked on to return the associated name.
    string LoadUsingIndex(int index)
    {
        switch (index)
        {
            case 1:
                return nameOne;
            case 2:
                return nameTwo;
            case 3:
                return nameThree;
            default:
                return "";
        }
    }
    //When a name is created, set its name, set its playerpref, update its name.
    void SetNameUsingIndex(int index, string newName)
    {
        switch (index)
        {
            case 1:
                nameOne = newName;
                PlayerPrefs.SetString("NameOne", newName);
                nameTags[0].text = newName;
                break;
            case 2:
                nameTwo = newName;
                PlayerPrefs.SetString("NameTwo", newName);
                nameTags[1].text = newName;
                break;
            case 3:
                nameThree = newName;
                PlayerPrefs.SetString("NameThree", newName);
                nameTags[2].text = newName;
                break;
            default:
                break;
        }
    }

    public void AskTutorial(bool startTutorial)
    {
        if (startTutorial)
        {
            SceneManager.LoadScene(3);

        }
        else
        {
            ContinueToStartScreen();
        }
    }
}
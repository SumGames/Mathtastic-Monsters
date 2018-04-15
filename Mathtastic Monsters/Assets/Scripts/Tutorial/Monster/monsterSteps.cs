using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class monsterSteps : MonoBehaviour
{

    public Text lillyText;
    internal int tutorialStage;


    public Button headButton;

    public Button nextPart;
    public Button prevPart;


    public Button[] otherSlots;

    public Button myButton;


    //Links to part and shop managers. Links them to list and tells them to start.
    public CombinedShop combinedShop;


    equipmentList list;


    public GameObject prefabbedList;

    public bool EnableEquip;


    public GameObject[] outlinesAndArrows;


    // Use this for initialization
    void Start()
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

        combinedShop.Begin(list);

        headButton.interactable = false;
        nextPart.interactable = false;
        prevPart.interactable = false;
        EnableEquip = false;

        foreach (Button item in otherSlots)
        {
            item.interactable = false;
        }

        SetStep(1);

        enableOutlines(-1);
    }

    void Update()
    {
    }

    public void ProgressTutorial()
    {
        tutorialStage++;

        SetStep(tutorialStage);
    }

    void SetStep(int a_step)
    {
        myButton.interactable = true;

        tutorialStage = a_step;

        switch (a_step)
        {
            case 1:
                lillyText.text = "Welcome to my workshop! Isn't it nice?";
                myButton.interactable = true;
                break;

            case 2:
                lillyText.text = "This is where you can bring your monster shards to me and I'll make new parts out of them that you can equip! Some parts have special abilities that will help you in battle!";
                break;

            case 3:
                lillyText.text = "You can choose part types by clicking these buttons!";
                enableOutlines(0);
                break;

            case 4:
                lillyText.text = "Here you can tap the arrows to have a look at available parts!";
                enableOutlines(1);
                myButton.interactable = true;
                break;

            case 5:
                lillyText.text = "This area shows the part you've selected, along with it's ability!";
                enableOutlines(2);
                break;

            case 6:
                lillyText.text = "Use this button to use shards and create parts, then equip them!";
                enableOutlines(3);
                break;

            case 7:
                lillyText.text = "And over here you can see what your monster looks like, along with any abilities it has! You now have everything you need going forward! Good luck and make sure to check back in to see what I can make for you!";
                enableOutlines(4);
                break;

            /*
                        case 8:
                            lillyText.text = "You can look at parts by tapping the arrows.";
                            myButton.interactable = true;
                            headButton.interactable = false;
                            break;

                        case 9:
                            lillyText.text = "Pick out a nice one and tap on its name to Buy It!";
                            prevPart.interactable = true;
                            nextPart.interactable = true;
                            myButton.interactable = false;
                            buyPart.interactable = true;

                            break;

                        case 10:
                            lillyText.text = "Oooh, a fine choice! Now to put it on! Now if we could just go back...";
                            headButton.interactable = false;
                            nextPart.interactable = false;
                            prevPart.interactable = false;
                            buyPart.interactable = false;
                            myButton.interactable = false;
                            break;

                        case 11:
                            lillyText.text = "Your room is over here, Hero!";
                            myButton.interactable = false;

                            break;

                        case 12:
                            lillyText.text = "This is your room! I'll chuck your extra pieces on the pile when they're not in use.";
                            myButton.interactable = true;
                            break;
                        case 13:
                            lillyText.text = "Your wardrobe works much like my shop.";            
                            break;

                        case 14:
                            lillyText.text = "Click the arrows limb names to change types, and the arrows to switch between parts you own";

                            break;
                        case 15:
                            lillyText.text = "Clicking on the name will remove or replace a part.\n But keep in mind that changing or remove the chest will-";
                            myButton.interactable = false;
                            break;

                        case 16:
                            lillyText.text = "-Remove everything else!\nOh... Why don't you build your monster yourself? Let me know when you're done!";
                            myButton.interactable = true;
                            foreach (Button item in otherSlots)
                            {
                                item.interactable = true;
                            }
                            break;
                        //https://docs.google.com/document/d/155cqU4X-KrRZv3BxWMFZFX1zls94GhRntpJP6tYW5Sk/edit#heading=h.gjdgxs
                        */

            default:
                Destroy(gameObject);
                SceneManager.LoadScene(1);
                break;
        }
    }

    void enableOutlines(int index)
    {
        for (int i = 0; i < outlinesAndArrows.Length; i++)
        {
            if (i == index)
                outlinesAndArrows[i].SetActive(true);
            else
                outlinesAndArrows[i].SetActive(false);
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class monsterSteps : MonoBehaviour
{

    public Text lillyText;
    internal int tutorialStage;

    public GameObject selectionContainer;
    public GameObject shopContainer;
    public GameObject customiseContainer;

    public Button toShop;
    public Button headButton;

    public Button nextPart;
    public Button prevPart;
    public Button buyPart;

    public Button shopback;
    public Button toCustom;

    public Button equipButton;
    public Button[] equipment;


    public GameObject[] OutlineGlows;
    public Button myButton;


    // Use this for initialization
    void Start()
    {
        selectionContainer.SetActive(true);
        shopContainer.SetActive(false);
        customiseContainer.SetActive(false);

        headButton.interactable = false;
        nextPart.interactable = false;
        prevPart.interactable = false;
        buyPart.interactable = false;
        shopback.interactable = false;

        foreach (Button item in equipment)
        {
            item.interactable = false;
        }

        SetStep(1);
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
                lillyText.text = "Hello, Hero! This is my little home away from evil.\n Isn't it nice?";
                myButton.interactable = true;
                toShop.interactable = false;
                break;

            case 2:
                lillyText.text = "It's here that I can help you by making monster parts!";
                break;

            case 3:
                lillyText.text = "Let me show you. Let's go to my shop!";
                myButton.interactable = false;
                toShop.interactable = true;
                break;

            case 4:
                lillyText.text = "Home, sweet home!";
                myButton.interactable = true;
                toShop.interactable = false;
                selectionContainer.SetActive(false);
                shopContainer.SetActive(true);
                break;

            case 5:
                lillyText.text = "Did you notice those shards you got from beating that beast?";
                break;

            case 6:
                lillyText.text = "You can use those to purchase new heads, Torsos, arms and legs!";
                break;

            case 7:
                lillyText.text = "I noticed your monster is lacking a head. Why don't we look at them?";
                myButton.interactable = false;
                headButton.interactable = true;
                break;


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

                shopback.interactable = true;
                break;

            case 11:
                lillyText.text = "Your room is over here, Hero!";
                shopback.interactable = false;
                toCustom.interactable = true;
                myButton.interactable = false;

                break;

            case 12:
                lillyText.text = "This is your room! I'll chuck your extra pieces on the pile when they're not in use.";
                toCustom.interactable = false;
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
                equipButton.interactable = true;
                break;

            case 16:
                lillyText.text = "-Remove everything else!\nOh... Why don't you build your monster yourself? Let me know when you're done!";
                myButton.interactable = true;
                foreach (Button item in equipment)
                {
                    item.interactable = true;
                }
                break;
            //https://docs.google.com/document/d/155cqU4X-KrRZv3BxWMFZFX1zls94GhRntpJP6tYW5Sk/edit#heading=h.gjdgxs
            default:
                Destroy(gameObject);
                SceneManager.LoadScene(0);
                break;
        }
    }
}
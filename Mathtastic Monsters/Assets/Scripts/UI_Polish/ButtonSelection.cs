using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelection : MonoBehaviour
{
    public Button[] buttons;//An array of buttons we can jump between.

    public Button Normal; //Clicked on to start normal mode.
    public Button Hard; //If we have two stars, click on to start the normal mode's "hard mode" button.    
}

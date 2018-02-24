using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FortressButton : MonoBehaviour {

    public FortressDragger draggedIn;

    public Fortress parent;

    // Use this for initialization
    void Start()
    {
        GetComponent<Image>().color = Color.white;
    }

    internal void ResetButton()
    {
        GetComponent<Image>().color = Color.white;
        draggedIn = null;
    }

    internal void ChangeDragger(FortressDragger collided, bool entered)
    {
        if (entered)
        {
            if (draggedIn != null)
            {
                return;
            }

            draggedIn = collided;
            draggedIn.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            if (draggedIn != null && collided == draggedIn)
            {

                if (draggedIn != null)
                    draggedIn.GetComponent<Image>().color = Color.white;
                draggedIn = null;

            }
        }
        parent.CheckDefenceReady();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public Slider CalculatorSlider;
    public Slider MultipleSlider;

    public Slider AdditionSlider;

    public Slider MultSlider;
    public Slider DivSlider;

    public Slider FortSliderDefend;
    public Slider FortSliderAbacus;


    public Slider[] SlidersList;
    public Image[] SliderFill;



	// Use this for initialization
	void Start ()
    {
		
	}


    internal void SetTimeLeft(float time)
    {
        foreach (Slider item in SlidersList)
        {
            item.value = time;
        }

    }

	
	// Update is called once per frame
	void Update ()
    {
		
	}

    internal void sliderColour(Color setColour)
    {
        foreach (Image item in SliderFill)
        {
            item.color = setColour;

        }
    }

    internal void SetMaxTime(bool disable, float TimeGiven)// bool enemyPhase, float enemyTime, bool Disable = false)
    {
        if (disable)
        {
            foreach (Slider item in SlidersList)
            {
                item.gameObject.SetActive(false);
                item.maxValue = 10000;
            }

            return;
        }


        foreach (Slider item in SlidersList)
        {
            item.gameObject.SetActive(true);
            item.value = item.maxValue = TimeGiven;
        }
    }
}

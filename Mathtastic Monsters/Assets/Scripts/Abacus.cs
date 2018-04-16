using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abacus : MonoBehaviour
{
    int OneValue;
    int TwoValue;
    int ThreeValue;
    int FourValue;

    int onesDefault = 1;
    int twosDefault = 2;
    int threesDefault = 10;
    int foursDefault = 20;


    public GameObject[] OneBlocks;
    public GameObject[] oneSpotNegatives;
    public GameObject[] oneSpotPositives;
    bool[] shiftedOnes = new bool[6];


    public GameObject[] TwoBlocks;
    public GameObject[] TwoSpotNegatives;
    public GameObject[] TwoSpotPositives;
    bool[] shiftedTwos = new bool[6];


    public GameObject[] ThreeBlocks;
    public GameObject[] ThreeSpotNegatives;
    public GameObject[] ThreeSpotPositives;
    bool[] shiftedThrees = new bool[6];

       
    public GameObject[] FourBlocks;
    public GameObject[] FourSpotNegatives;
    public GameObject[] FourSpotPositives;
    bool[] shiftedFours = new bool[6];



    public Text OneText;
    public Text TwoText;
    public Text ThreeText;
    public Text FourText;

    int Total;


    public bool testing = false;


    public Text CurrentText;


    public void ResetAbacus(bool random, float Goal)
    {
        ResetBars();

        if (random)
        {
            SetRandom((int)Goal);

        }
        else
        {
            OneValue = onesDefault;
            TwoValue = twosDefault;
            ThreeValue = threesDefault;
            FourValue = foursDefault;

        }
        setText();

    }

    void setText()
    {
        OneText.text = OneValue.ToString();
        TwoText.text = TwoValue.ToString();
        ThreeText.text = ThreeValue.ToString();
        FourText.text = FourValue.ToString();
    }

    internal void SetRandom(int goal=0)
    {
        Total = goal;
        

        int remaining = Total;

        int value = Random.Range(4, 12);
        int bars = Random.Range(1, 5);

        OneValue = value / bars;
        remaining -= OneValue*bars;


        value = Random.Range(8, 18);
        bars = Random.Range(1, 4);

        TwoValue = value / bars;
        remaining -= TwoValue * bars;



        value = Random.Range(10, 30);
        bars = Random.Range(1, 4);

        ThreeValue = value / bars;
        remaining -= ThreeValue * bars;

        if (remaining < 5)
        {
            FourValue = 1;
        }
        else
        {
            value = remaining;
            Random.Range(1, 4);

            FourValue = value / bars;
        }
    }


    void ResetBars()
    {
        for (int i = (OneBlocks.Length-1); i >= 0; i--)
        {
            OneBlocks[i].transform.position = oneSpotNegatives[i].transform.position;
            shiftedOnes[i] = false;
            TwoBlocks[i].transform.position = TwoSpotNegatives[i].transform.position;
            shiftedTwos[i] = false;
            ThreeBlocks[i].transform.position = ThreeSpotNegatives[i].transform.position;
            shiftedThrees[i] = false;
            FourBlocks[i].transform.position = FourSpotNegatives[i].transform.position;
            shiftedFours[i] = false;
        }
        checkTotal();
    }


    public void ClickBarOne(int index)
    {
        if (!shiftedOnes[index])
        {
            for (int i = index; i < OneBlocks.Length; i++)
            {
                OneBlocks[i].transform.position = oneSpotPositives[i].transform.position;
                shiftedOnes[i] = true;
            }
        }
        else
        {
            for (int i = index; i >= 0; i--)
            {
                OneBlocks[i].transform.position = oneSpotNegatives[i].transform.position;
                shiftedOnes[i] = false;
            }
        }
        checkTotal();
    }

    public void ClickBarTwo(int index)
    {
        if (!shiftedTwos[index])
        {
            for (int i = index; i < TwoBlocks.Length; i++)
            {
                TwoBlocks[i].transform.position = TwoSpotPositives[i].transform.position;
                shiftedTwos[i] = true;
            }
        }
        else
        {
            for (int i = index; i >= 0; i--)
            {
                TwoBlocks[i].transform.position = TwoSpotNegatives[i].transform.position;
                shiftedTwos[i] = false;
            }
        }
        checkTotal();
    }


    public void ClickBarThree(int index)
    {
        if (!shiftedThrees[index])
        {
            for (int i = index; i < ThreeBlocks.Length; i++)
            {
                ThreeBlocks[i].transform.position = ThreeSpotPositives[i].transform.position;
                shiftedThrees[i] = true;
            }
        }
        else
        {
            for (int i = index; i >= 0; i--)
            {
                ThreeBlocks[i].transform.position = ThreeSpotNegatives[i].transform.position;
                shiftedThrees[i] = false;
            }
        }
        checkTotal();
    }

    public void ClickBarFour(int index)
    {
        if (!shiftedFours[index])
        {
            for (int i = index; i < FourBlocks.Length; i++)
            {
                FourBlocks[i].transform.position = FourSpotPositives[i].transform.position;
                shiftedFours[i] = true;
            }
        }
        else
        {
            for (int i = index; i >= 0; i--)
            {
                FourBlocks[i].transform.position = FourSpotNegatives[i].transform.position;
                shiftedFours[i] = false;
            }
        }
        checkTotal();
    }

    public int CalculateTotal()
    {
        int total = 0;


        for (int i = 1; i < 5; i++)
        {
            total += ReturnTrueBools(i);
        }

        return total;
    }

    int ReturnTrueBools(int row)
    {
        int index = 0;
        switch (row)
        {
            case 1:
                for (int i = 0; i < shiftedOnes.Length; i++)
                {
                    if (shiftedOnes[i])
                        index++;
                }
                return OneValue * index;
            case 2:
                for (int i = 0; i < shiftedTwos.Length; i++)
                {
                    if (shiftedTwos[i])
                        index++;
                }
                return TwoValue * index;
            case 3:
                for (int i = 0; i < shiftedThrees.Length; i++)
                {
                    if (shiftedThrees[i])
                        index++;
                }
                return ThreeValue * index;
            case 4:
                for (int i = 0; i < shiftedFours.Length; i++)
                {
                    if (shiftedFours[i])
                        index++;
                }
                return FourValue * index;
            default:
                break;
        }

        return index;
    }

    void checkTotal()
    {
        CurrentText.text = CalculateTotal().ToString();

    }
}
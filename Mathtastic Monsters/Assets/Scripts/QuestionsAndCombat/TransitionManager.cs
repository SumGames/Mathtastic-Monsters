using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransitionState
{
    CalculatorShrink,
    CalculatorGrow,
    MultShrink,
    MultGrow,
    None
}

public class TransitionManager : MonoBehaviour
{
    public playerAbilities abilities;

    public float transitionSpeed = 1;

    public float transitionMin = 0.1f;

    public GameObject Calculator;
    float calSize;

    public GameObject MultipleChoice;
    float choSize;

    public TransitionState transitionState;

    public StoryManager storyManager;

    // Use this for initialization
    void Start()
    {
        calSize = 1;
        choSize = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (transitionState == TransitionState.None)
            return;

        if (storyManager && storyManager.phase != phases.None)
        {
            Calculator.SetActive(false);
            MultipleChoice.SetActive(false);
            return;
        }

        if (transitionState == TransitionState.CalculatorGrow)
        {
            if (calSize >= 1)
            {
                Calculator.SetActive(true);
                calSize = 1;
                Calculator.transform.localScale = new Vector3(1, 1, 1);
                transitionState = TransitionState.None;
                abilities.AbleAbilities(true);
                return;
            }
            else
            {
                Calculator.SetActive(true);
                calSize += Time.deltaTime * transitionSpeed;
                Calculator.transform.localScale = new Vector3(calSize, calSize, 1);
                abilities.AbleAbilities(false);
            }
        }
        if (transitionState == TransitionState.MultGrow)
        {
            if (choSize >= 1)
            {
                MultipleChoice.SetActive(true);
                choSize = 1;
                MultipleChoice.transform.localScale = new Vector3(1, 1, 1);
                transitionState = TransitionState.None;
                abilities.AbleAbilities(true);
                return;
            }
            else
            {
                MultipleChoice.SetActive(true);
                choSize += Time.deltaTime * transitionSpeed;
                MultipleChoice.transform.localScale = new Vector3(choSize, choSize, 1);
            }
        }

        abilities.AbleAbilities(false);

        if (transitionState == TransitionState.CalculatorShrink)
        {
            if (calSize <= transitionMin)
            {
                Calculator.SetActive(true);
                calSize = transitionMin;
                Calculator.transform.localScale = new Vector3(0, 0, 1);
                transitionState = TransitionState.MultGrow;
            }
            else
            {
                Calculator.SetActive(true);
                calSize -= Time.deltaTime * transitionSpeed;
                Calculator.transform.localScale = new Vector3(calSize, calSize, 1);
            }
        }
        if (transitionState == TransitionState.MultShrink)
        {
            if (choSize <= transitionMin)
            {
                MultipleChoice.SetActive(true);
                choSize = transitionMin;
                MultipleChoice.transform.localScale = new Vector3(0, 0, 1);
                transitionState = TransitionState.CalculatorGrow;
            }
            else
            {
                MultipleChoice.SetActive(true);
                choSize -= Time.deltaTime * transitionSpeed;
                MultipleChoice.transform.localScale = new Vector3(choSize, choSize, 1);
            }
        }

    }

    internal void TransitionContainers(TransitioningObjects a_mode)
    {
        switch (a_mode)
        {
            case TransitioningObjects.SwapToMultiple:
                transitionState = TransitionState.CalculatorShrink;
                break;
            case TransitioningObjects.SwapToCalculator:
                transitionState = TransitionState.MultShrink;
                break;
            case TransitioningObjects.JustSwap:
                choSize = 1;
                calSize = 1;
                MultipleChoice.transform.localScale = new Vector3(1, 1, 1);
                Calculator.transform.localScale = new Vector3(1, 1, 1);
                transitionState = TransitionState.None;
                break;
            default:
                break;
        }
    }
}

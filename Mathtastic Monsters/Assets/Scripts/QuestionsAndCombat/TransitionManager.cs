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
    public float transitionSpeed = 1;

    public float transitionMin = 0.1f;

    public GameObject Calculator;
    float calSize;

    public GameObject MultipleChoice;
    float choSize;

    public TransitionState transitionState;

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

        if (transitionState == TransitionState.CalculatorGrow)
        {
            if (calSize >= 1)
            {
                calSize = 1;
                Calculator.transform.localScale = new Vector3(1, 1, 1);
                transitionState = TransitionState.None;
            }
            else
            {
                calSize += Time.deltaTime * transitionSpeed;
                Calculator.transform.localScale = new Vector3(calSize, calSize);
            }
        }
        if (transitionState == TransitionState.MultGrow)
        {
            if (choSize >= 1)
            {
                choSize = 1;
                MultipleChoice.transform.localScale = new Vector3(1, 1, 1);
                transitionState = TransitionState.None;
            }
            else
            {
                choSize += Time.deltaTime * transitionSpeed;
                MultipleChoice.transform.localScale = new Vector3(choSize, choSize);
            }
        }

        if (transitionState == TransitionState.CalculatorShrink)
        {
            if (calSize <= transitionMin)
            {
                calSize = transitionMin;
                Calculator.transform.localScale = new Vector3(0, 0, 0);
                transitionState = TransitionState.MultGrow;
            }
            else
            {
                calSize -= Time.deltaTime * transitionSpeed;
                Calculator.transform.localScale = new Vector3(calSize, calSize);
            }
        }
        if (transitionState == TransitionState.MultShrink)
        {
            if (choSize <= transitionMin)
            {
                choSize = transitionMin;
                MultipleChoice.transform.localScale = new Vector3(0, 0, 0);
                transitionState = TransitionState.CalculatorGrow;
            }
            else
            {
                choSize -= Time.deltaTime * transitionSpeed;
                MultipleChoice.transform.localScale = new Vector3(choSize, choSize);
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

using UnityEngine;
using System;

[Serializable]
public class HighSaveData
{


    [SerializeField]
    public int[][] Highest_LevelLevels;
    [SerializeField]
    public float[][] highest_LevelScore;


    [SerializeField]
    public string[][] highest_LevelNames;

    [SerializeField]
    public int[][] highest_ScoreLevel;
    [SerializeField]
    public float[][] highest_ScoreScore;

    [SerializeField]
    public string[][] highest_ScoreNames;

}
using UnityEngine;
using System;


using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class HighSaveScore : MonoBehaviour
{

    public int[,] HighLevelLevels;

    public float[,] highLevelScore;

    public string[,] highLevelNames;


    public int[,] highScoreLevel;

    public float[,] highScoreScore;

    public string[,] highScoreNames;



    HighSaveData data;


    //Check if we got a level higher than startingIndex
    //As an example, the external check would be first place, so 0.
    //If that was true, we'd continually check the score we're replacing is better than any other score.
    public int checkLevel(operators op, int newLevel, float andScore, string playerName, int startingIndex = 0) 
    {
        for (int i = startingIndex; i < 5; i++)
        {
            if (newLevel > HighLevelLevels[i, (int)op])
            {
                checkLevel(op, HighLevelLevels[i, (int)op], highLevelScore[i, (int)op], highLevelNames[i, (int)op], (i + 1));

                HighLevelLevels[i, (int)op] = newLevel;
                highLevelScore[i, (int)op] = andScore;
                highLevelNames[i, (int)op] = playerName;

                return i;
            }

        }

        return -1; //We got no high score.
    }

    //Above, but for scores.
    public int checkScore(operators op, float newScore, int andLevel, string playerName, int startingI = 0)
    {
        for (int i = startingI; i < 5; i++)
        {
            if (newScore > highScoreScore[i, (int)op])
            {
                checkScore(op, highScoreScore[i, (int)op], highScoreLevel[i, (int)op], highScoreNames[i, (int)op], (i + 1));

                highScoreScore[i, (int)op] = newScore;
                highScoreLevel[i, (int)op] = andLevel;
                highScoreNames[i, (int)op] = playerName;

                return i;
            }
        }

        return -1;
    }

    //Return the highest score's stats,
    public string returnBest(operators op, bool Score)
    {
        string returning = "";
        if (Score)
        {

            returning += "Best " + op.ToString() + " Score run by " + highScoreNames[0, (int)op] + "\n";
            returning += "Points: " + highScoreScore[0, (int)op].ToString() + ".  Levels: " + highScoreLevel[0, (int)op].ToString();
        }
        else
        {
            returning += "Best " + op.ToString() + " Level run by " + highLevelNames[0, (int)op] + "\n";
            returning += "Levels: " + HighLevelLevels[0, (int)op].ToString() + ".  Points: " + highLevelScore[0, (int)op].ToString();
        }


        return returning;
    }

    //Return the Ranking scores.
    //No for loop because 1st, 2nd is different for each, and we'd need another array for that.
    public string returnRanking(operators op, bool Score)
    {
        int index = 0;
        string returning = "";
        if (Score)
        {
            returning = op.ToString() + " Score Rankings:\n";

            returning += "1st is " + highScoreNames[index, (int)op] + " at " + highScoreScore[index, (int)op].ToString() + " Points\n";
            index++;
            returning += "2nd is " + highScoreNames[index, (int)op] + " at " + highScoreScore[index, (int)op].ToString() + " Points\n";
            index++;
            returning += "3rd is " + highScoreNames[index, (int)op] + " at " + highScoreScore[index, (int)op].ToString() + " Points\n";
            index++;
            returning += "4th is " + highScoreNames[index, (int)op] + " at " + highScoreScore[index, (int)op].ToString() + " Points\n";
            index++;
            returning += "5th is " + highScoreNames[index, (int)op] + " at " + highScoreScore[index, (int)op].ToString() + " Points\n";
        }
        else
        {
            returning = op.ToString() + " Level Rankings:\n";

            returning += "1st is " + highLevelNames[index, (int)op] + " at " + HighLevelLevels[index, (int)op].ToString() + " Levels\n";
            index++;
            returning += "2nd is " + highLevelNames[index, (int)op] + " at " + HighLevelLevels[index, (int)op].ToString() + " Levels\n";
            index++;
            returning += "3rd is " + highLevelNames[index, (int)op] + " at " + HighLevelLevels[index, (int)op].ToString() + " Levels\n";
            index++;
            returning += "4th is " + highLevelNames[index, (int)op] + " at " + HighLevelLevels[index, (int)op].ToString() + " Levels\n";
            index++;
            returning += "5th is " + highLevelNames[index, (int)op] + " at " + HighLevelLevels[index, (int)op].ToString() + " Levels\n";
        }


        return returning;
    }


    //Set all scores to zero.
    public void setHighZero()
    {
        HighLevelLevels = new int[5, 6];
        highLevelScore = new float[5, 6];
        highLevelNames = new string[5, 6];

        highScoreLevel = new int[5, 6];
        highScoreScore = new float[5, 6];
        highScoreNames = new string[5, 6];
    }


    //Save our high scores to the data class, then serialise it.
    public void Save(string playerName)
    {
        data = new HighSaveData();

        saveToData();

        string fileName = Application.persistentDataPath + "/HighScores.gd";

        Debug.Log(fileName);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(fileName);
        bf.Serialize(file, data);
        file.Close();
    }


    //Deserialise and load the file.
    public bool Load()
    {
        data = new HighSaveData();

        string fileName = Application.persistentDataPath + "/HighScores.gd";
        if (File.Exists(fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(fileName, FileMode.Open);
            data = (HighSaveData)bf.Deserialize(file);
            file.Close();
            loadFromData();
            return true;
        }
        return false;
    }

    //Convert our 2D arrays to 1D serialisable arrays.
    public void saveToData()
    {
        data.Highest_LevelLevels = new int[5][];
        data.highest_LevelNames = new string[5][];
        data.highest_LevelScore = new float[5][];

        data.highest_ScoreLevel = new int[5][];
        data.highest_ScoreNames = new string[5][];
        data.highest_ScoreScore = new float[5][];

        for (int i = 0; i < HighLevelLevels.GetLength(0); i++)
        {
            data.Highest_LevelLevels[i] = new int[HighLevelLevels.GetLength(1)];
            data.highest_LevelNames[i] = new string[HighLevelLevels.GetLength(1)];
            data.highest_LevelScore[i] = new float[HighLevelLevels.GetLength(1)];

            data.highest_ScoreLevel[i] = new int[HighLevelLevels.GetLength(1)];
            data.highest_ScoreNames[i] = new string[HighLevelLevels.GetLength(1)];
            data.highest_ScoreScore[i] = new float[HighLevelLevels.GetLength(1)];

            for (int j = 0; j < HighLevelLevels.GetLength(1); j++)
            {
                data.Highest_LevelLevels[i][j] = HighLevelLevels[i, j];
                data.highest_LevelNames[i][j] = highLevelNames[i, j];
                data.highest_LevelScore[i][j] = highLevelScore[i, j];

                data.highest_ScoreLevel[i][j] = highScoreLevel[i, j];
                data.highest_ScoreNames[i][j] = highScoreNames[i, j];
                data.highest_ScoreScore[i][j] = highScoreScore[i, j];
            }
        }
    }

    //Converting from 1D arrays to 2D.
    public void loadFromData()
    {
        for (int i = 0; i < HighLevelLevels.GetLength(0); i++)
        {
            for (int j = 0; j < HighLevelLevels.GetLength(1); j++)
            {
                HighLevelLevels[i, j] = data.Highest_LevelLevels[i][j];
                highLevelNames[i, j] = data.highest_LevelNames[i][j];
                highLevelScore[i, j] = data.highest_LevelScore[i][j];

                highScoreLevel[i, j] = data.highest_ScoreLevel[i][j];
                highScoreNames[i, j] = data.highest_ScoreNames[i][j];
                highScoreScore[i, j] = data.highest_ScoreScore[i][j];
            }
        }
    }
}
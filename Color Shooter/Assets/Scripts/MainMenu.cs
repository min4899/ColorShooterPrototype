using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using BayatGames.SaveGameFree;
using System;

public class Level
{
    public bool unlocked = false;
    public bool cleared = false;
    public int bestScore = 0;
    public int bestStars = 0;
}

public class MainMenu : MonoBehaviour {

    public int numberOfLevels;
    public TextMeshProUGUI areaNumber;
    public TextMeshProUGUI levelNumber;

    public TextMeshProUGUI bestScore;
    public GameObject stars;

    private static int area = 0;
    private static int selectedLevel = 0;
    private int sceneIndex;

    public static Level[] levels;

    void Awake()
    {
        //SaveGame.Clear();
        //Debug.Log("CLEARING ALL SAVE FILES");

        if (SaveGame.Exists("levels")) // save file exists, load this file 
        {
            Debug.Log("Save found. Loading save.");
            levels = SaveGame.Load<Level[]>("levels");

            if (levels.Length != numberOfLevels) // new levels have been added, update level data array to include new levels
            {
                Debug.Log("Save file size is not same as number of levels, updating save file to include new levels.");
                Array.Resize(ref levels, numberOfLevels);
                for(int i = 0; i < levels.Length; i++)
                {
                    if(levels[i] == null)
                    {
                        levels[i] = new Level();
                        //Debug.Log("No level object found, creating new level object");
                    }
                }
                Debug.Log("New save file size now includes " + levels.Length + " levels.");
                SaveGame.Save<Level[]>("levels", levels);
            }
        }
        else // save file does not exist, create new file
        {
            Debug.Log("No save found. Creating new save.");
            levels = new Level[numberOfLevels];
            for(int i = 0; i < levels.Length; i++)
            {
                levels[i] = new Level();
            }
        }
    }

	// Use this for initialization
	void Start () {
        if(area == 0 || selectedLevel == 0)
        {
            area = 1;
            selectedLevel = 1;
        }
        //area = 1;
        //selectedLevel = 1;
        areaNumber.text = area.ToString();
        SelectLevel(selectedLevel);

    }

    public void Left()
    {
        if(area > 1)
        {
            area--;
            areaNumber.text = area.ToString();
            selectedLevel = 1;
            SelectLevel(1);
        }
    }

    public void Right()
    {
        //if (area < 2)
        if(area < numberOfLevels/4) // each area has 4 levels
        {
            area++;
            areaNumber.text = area.ToString();
            selectedLevel = 1;
            SelectLevel(1);
        }
    }

    public void SelectLevel(int i)
    {
        AudioManager.instance.PlaySound("Select_Sound");
        selectedLevel = i;
        sceneIndex = selectedLevel + (area - 1) * 4;
        levelNumber.text = "Level " + selectedLevel.ToString();
        Debug.Log("Area " + area.ToString() + ", Level " + i.ToString() + " (Scene: " + sceneIndex + ") selected.");
        loadStats();
    }

    void loadStats()
    {
        // Display stats for this level
        Debug.Log("Displaying stats for Scene " + sceneIndex.ToString() + " (levels array index:" + (sceneIndex - 1).ToString() + ")");
        if (levels[sceneIndex - 1].cleared) // level was cleared, can display proper stats
        {
            bestScore.text = levels[sceneIndex - 1].bestScore.ToString(); // displaying best scores
            switch (levels[sceneIndex - 1].bestStars) // displaying stars
            {
                case 1:
                    stars.transform.GetChild(0).gameObject.SetActive(true);
                    stars.transform.GetChild(1).gameObject.SetActive(false);
                    stars.transform.GetChild(2).gameObject.SetActive(false);
                    stars.transform.GetChild(3).gameObject.SetActive(false);
                    stars.transform.GetChild(4).gameObject.SetActive(false);
                    break;
                case 2:
                    stars.transform.GetChild(0).gameObject.SetActive(false);
                    stars.transform.GetChild(1).gameObject.SetActive(true);
                    stars.transform.GetChild(2).gameObject.SetActive(false);
                    stars.transform.GetChild(3).gameObject.SetActive(false);
                    stars.transform.GetChild(4).gameObject.SetActive(false);
                    break;
                case 3:
                    stars.transform.GetChild(0).gameObject.SetActive(false);
                    stars.transform.GetChild(1).gameObject.SetActive(false);
                    stars.transform.GetChild(2).gameObject.SetActive(true);
                    stars.transform.GetChild(3).gameObject.SetActive(false);
                    stars.transform.GetChild(4).gameObject.SetActive(false);
                    break;
                case 4:
                    stars.transform.GetChild(0).gameObject.SetActive(false);
                    stars.transform.GetChild(1).gameObject.SetActive(false);
                    stars.transform.GetChild(2).gameObject.SetActive(false);
                    stars.transform.GetChild(3).gameObject.SetActive(true);
                    stars.transform.GetChild(4).gameObject.SetActive(false);
                    break;
                default:
                    stars.transform.GetChild(0).gameObject.SetActive(false);
                    stars.transform.GetChild(1).gameObject.SetActive(false);
                    stars.transform.GetChild(2).gameObject.SetActive(false);
                    stars.transform.GetChild(3).gameObject.SetActive(false);
                    stars.transform.GetChild(4).gameObject.SetActive(true);
                    break;
            }
        }
        else // not cleared, setting default values
        {
            bestScore.text = "---";
            stars.transform.GetChild(0).gameObject.SetActive(false);
            stars.transform.GetChild(1).gameObject.SetActive(false);
            stars.transform.GetChild(2).gameObject.SetActive(false);
            stars.transform.GetChild(3).gameObject.SetActive(false);
            stars.transform.GetChild(4).gameObject.SetActive(true);
        }
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(sceneIndex);
        Debug.Log("Playing Area " + area.ToString() + ", Level " + selectedLevel.ToString() + " (Scene: " + sceneIndex + ").");
    }
}

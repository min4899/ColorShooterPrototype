using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using BayatGames.SaveGameFree;
using System;
using UnityEngine.Advertisements;
using UnityEngine.EventSystems;

public class Level
{
    public bool unlocked = false;
    public bool cleared = false;
    public int bestScore = 0;
    public int bestStars = 0;
}

public class MainMenu : MonoBehaviour {

    public bool devMode = false;
    public int numberOfLevels;
    public TextMeshProUGUI areaNumber;
    public TextMeshProUGUI levelNumber;
    [SerializeField] private string bannerPlacementId;

    public TextMeshProUGUI bestScore;
    public GameObject stars;

    public GameObject level1Lock;
    public GameObject level2Lock;
    public GameObject level3Lock;
    public GameObject level4Lock;
    public GameObject playLocked;

    public GameObject Sound;
    public GameObject Music;
    public GameObject SoundController;
    public GameObject MusicController;

    private static int area = 0;
    private static int selectedLevel = 0;
    private int sceneIndex;
    private int numLevelsCleared;
    private GameObject currentMenu;

    public static Level[] levels;

    void Awake()
    {
        currentMenu = gameObject.transform.GetChild(0).gameObject;

        // If settings aren't set up, set it up now.
        if(!SaveGame.Exists("SoundOn"))
        {
            SaveGame.Save<bool>("SoundOn", true);
        }
        if (!SaveGame.Exists("MusicOn"))
        {
            SaveGame.Save<bool>("MusicOn", true);
        }
        LoadSettingsOnScreen();

        // Load Save File
        if (SaveGame.Exists("levels")) // save file exists, load this file 
        {
            Debug.Log("Save found. Loading save.");
            levels = SaveGame.Load<Level[]>("levels");

            if (levels.Length != numberOfLevels) // new levels have been added, update level data array to include new levels
            {
                Debug.Log("Save file size is not same as number of levels, updating save file to include new levels.");
                Array.Resize(ref levels, numberOfLevels);
                for (int i = 0; i < levels.Length; i++)
                {
                    if (levels[i] == null)
                    {
                        levels[i] = new Level();
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
            for (int i = 0; i < levels.Length; i++)
            {
                levels[i] = new Level();
            }
        }

        // Get number of cleared levels
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].cleared == true)
                numLevelsCleared++;
        }
        Debug.Log("Number of cleared levels: " + numLevelsCleared);

        // Unlock the next level, or unlock none if all levels are cleared.
        if (numLevelsCleared < numberOfLevels)
        {
            levels[numLevelsCleared].unlocked = true;
            // just use numLevelsCleared since it corresponds to the index in the levels array.
        }
    }

	// Use this for initialization
	void Start () {
        if(area == 0 || selectedLevel == 0) // when game launches
        {
            area = 1;
            selectedLevel = 1;
        }
        areaNumber.text = area.ToString();
        SelectLevel(selectedLevel);
        UnlockLevels();

        StartCoroutine(ShowBannerWhenReady()); // show banner ad at the bottom
    }

    public void Left()
    {
        if(area > 1)
        {
            area--;
            areaNumber.text = area.ToString();
            UnlockLevels();
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
            UnlockLevels();
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

        if(!devMode)
        {
            if(levels[sceneIndex-1].unlocked) // if the game is unlocked, do not lock the play button
            {
                playLocked.SetActive(false);
            }
            else
            {
                playLocked.SetActive(true);
            }
        }
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
        Advertisement.Banner.Hide(false);
    }

    void UnlockLevels()
    {
        if (devMode) // unlock all levels automatically in dev mode
        {
            level1Lock.SetActive(false);
            level2Lock.SetActive(false);
            level3Lock.SetActive(false);
            level4Lock.SetActive(false);
        }
        else
        {
            // for Level 1 of the area
            if (levels[(area - 1)*4].unlocked)
            {
                level1Lock.SetActive(false);
            }
            else
            {
                level1Lock.SetActive(true);
            }

            // for Level 2 of the area
            if (levels[(area - 1)*4 + 1].unlocked)
            {
                level2Lock.SetActive(false);
            }
            else
            {
                level2Lock.SetActive(true);
            }

            // for Level 3 of the area
            if (levels[(area - 1)*4 + 2].unlocked)
            {
                level3Lock.SetActive(false);
            }
            else
            {
                level3Lock.SetActive(true);
            }

            // for Level 4 of the area
            if (levels[(area - 1)*4 + 3].unlocked)
            {
                level4Lock.SetActive(false);
            }
            else
            {
                level4Lock.SetActive(true);
            }
        }
    }

    public void GotoOption(GameObject selection)
    {
        AudioManager.instance.PlaySound("Select_Sound");
        selection.SetActive(true); // set selected menu active
        currentMenu.SetActive(false);
        currentMenu = selection;
    }

    IEnumerator ShowBannerWhenReady()
    {
        while (!Advertisement.IsReady(bannerPlacementId))
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.Show(bannerPlacementId);
    }

    // on/off button for specified option setting
    public void OnOffOption(string option)
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        //Debug.Log(button.name);
        if (SaveGame.Exists(option))
        {
            bool choice = SaveGame.Load<bool>(option);
            choice = !choice; // set opposite choice
            SaveGame.Save<bool>(option, choice);
            Debug.Log("Option " + option + ": " + choice);
        }
        else
        {
            Debug.Log("The option '" + option + "' does not exist in the save files. Nothing changed.");
        }
        LoadSettingsOnScreen();
    }

    void LoadSettingsOnScreen()
    {
        bool soundOn = SaveGame.Load<bool>("SoundOn");
        if(soundOn)
        {
            Sound.transform.GetChild(0).gameObject.SetActive(true); // on text
            Sound.transform.GetChild(1).gameObject.SetActive(false); // off text
            SoundController.GetComponent<AudioManager>().SetSoundOption();
        }
        else
        {
            Sound.transform.GetChild(0).gameObject.SetActive(false); // on text
            Sound.transform.GetChild(1).gameObject.SetActive(true); // off text
            SoundController.GetComponent<AudioManager>().SetSoundOption();
        }

        bool musicOn = SaveGame.Load<bool>("MusicOn");
        if (musicOn)
        {
            Music.transform.GetChild(0).gameObject.SetActive(true); // on text
            Music.transform.GetChild(1).gameObject.SetActive(false); // off text
            MusicController.SetActive(true);
        }
        else
        {
            Music.transform.GetChild(0).gameObject.SetActive(false); // on text
            Music.transform.GetChild(1).gameObject.SetActive(true); // off text
            MusicController.SetActive(false);
        }
    }

    public void ClearSaveData()
    {
        SaveGame.Clear();
        Debug.Log("CLEARING ALL SAVE FILES");
        SceneManager.LoadScene("_MainMenu");
    }
}

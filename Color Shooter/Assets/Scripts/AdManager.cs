using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Advertisements;
using UnityEngine.Monetization;

public class AdManager : MonoBehaviour {

    public static AdManager instance;
    private static float adChance = 0.1f;

    [Header("Config")]
    [SerializeField] private string gameID = "";
    [SerializeField] private bool testMode = true;
    [SerializeField] private string rewardedVideoPlacementId;
    [SerializeField] private string regularPlacementId;
    [SerializeField] private string bannerPlacementId;

    // Use this for initialization
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            Monetization.Initialize(gameID, testMode);
            //Debug.Log("New instance");
        }
    }

    // For REGULAR AD
    public void ShowRegularAd()
    {
        float chance = Random.Range(0.0f, 1.0f);
        Debug.Log("Chance to watch ad: " + adChance);
        if (chance <= adChance)
        {
            StartCoroutine(ShowAdWhenReady());
            adChance = 0.1f;
            Debug.Log("Waching Regular Ad.");
        }
        else
        {
            adChance += 0.2f;
        }
    }

    private IEnumerator ShowAdWhenReady()
    {
        while (!Monetization.IsReady(regularPlacementId))
        {
            yield return new WaitForSeconds(0.25f);
        }

        ShowAdPlacementContent ad = null;
        ad = Monetization.GetPlacementContent(regularPlacementId) as ShowAdPlacementContent;

        if (ad != null)
        {
            ad.Show();
        }
    }

    // FOR RESPAWN REWARD AD
    public void ShowRewardedAd()
    {
        StartCoroutine(WaitForAd());
        Debug.Log("Waching Reward Ad.");
    }

    IEnumerator WaitForAd()
    {
        while (!Monetization.IsReady(rewardedVideoPlacementId))
        {
            yield return null;
        }

        ShowAdPlacementContent ad = null;
        ad = Monetization.GetPlacementContent(rewardedVideoPlacementId) as ShowAdPlacementContent;

        if (ad != null)
        {
            ad.Show(AdFinished);
        }
    }

    void AdFinished(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            // Reward the player
            Debug.Log("Player finished the Reward Ad, reward given.");
            PauseMenu.instance.Respawn();
            //StartCoroutine(PauseMenu.instance.Respawn());
            adChance = 0.0f;
        }
        else
        {
            Debug.Log("Player closed or skipped the Reward Ad, no reward given.");
        }
    }
}

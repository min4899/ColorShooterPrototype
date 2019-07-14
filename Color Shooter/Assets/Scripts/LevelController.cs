using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Serializable classes
[System.Serializable]
public class EnemyWaves 
{
    [Tooltip("time for wave generation from the moment the game started")]
    public float timeToStart;

    [Tooltip("Enemy wave's prefab")]
    public GameObject wave;

    [Tooltip("Params if using prebuilt wave")]
    public WaveParameter waveParams;
}
#endregion

#region Serializable classes
[System.Serializable]
public class WaveParameter
{
    [Tooltip("Enemy's prefab")]
    public GameObject enemy;

    [Tooltip("a number of enemies in the wave")]
    public int count;

    [Tooltip("path passage speed")]
    public float speed;

    [Tooltip("time between emerging of the enemies in the wave")]
    public float timeBetween;

    [Tooltip("whether 'Enemy' rotates in path passage direction")]
    public bool rotationByPath;

    [Tooltip("if loop is activated, after completing the path 'Enemy' will return to the starting point")]
    public bool Loop;

    [Tooltip("(Wave2 Object)Use one of the prebuilt wave movement for exiting level once current movement is complete.")]
    public GameObject nextMovement;

    [Tooltip("Time to wait before starting the next movement wave.")]
    public float nextMoveDelay;

    [Tooltip("Speed of next movement.")]
    public float nextMoveSpeed;


    [Tooltip("negative is to move left, positive is to move right, magnitude is distance")]
    public float scaleX = 1;
    [Tooltip("negative is to move down, positive is to move up, magnitude is distance")]
    public float scaleY = 1;
}
#endregion

public class LevelController : MonoBehaviour {

    [Tooltip("Use as a grouper for multiple wave types, instead of overall level controller")]
    public bool WaveCombination;

    [Tooltip("Using a prebuilt wave controller, set up wave params object if using prebuilt")]
    public bool prebuilt = false;

    //Serializable classes implements
    public EnemyWaves[] enemyWaves;

    //private EnemyWaves[] newEnemyWaves;

    /*
    public GameObject powerUp;
    public float timeForNewPowerup;
    public GameObject[] planets;
    public float timeBetweenPlanets;
    public float planetsSpeed;
    List<GameObject> planetsList = new List<GameObject>();
    */

    //Camera mainCamera;   

    private void Start()
    {
        //mainCamera = Camera.main;
        //for each element in 'enemyWaves' array creating coroutine which generates the wave
        //Debug.Log("Starting game");
        
        float totalTime = 0;
        for(int i = 0; i <enemyWaves.Length; i++)
        {
            totalTime += enemyWaves[i].timeToStart;
        }
        for (int i = 0; i < enemyWaves.Length; i++)
        {
            //StartCoroutine(CreateEnemyWave(enemyWaves[i].timeToStart, enemyWaves[i].wave));
            StartCoroutine(CreateEnemyWave(enemyWaves[i].timeToStart, enemyWaves[i].wave, enemyWaves[i].waveParams));
        }
        if (WaveCombination)
        {
            Destroy(gameObject, totalTime + 3.0f); // Destroy this object 3 seconds after the last wave started.
            //Destroy(gameObject); // Destroy object after all its waves are complete.
        }
        //StartCoroutine(PowerupBonusCreation());
        //StartCoroutine(PlanetsCreation());
    }

    //Create a new wave after a delay
    //IEnumerator CreateEnemyWave(float delay, GameObject Wave) 
    IEnumerator CreateEnemyWave(float delay, GameObject Wave, WaveParameter waveParams)
    {
        if (delay != 0)
            yield return new WaitForSeconds(delay);
        if (Player.instance != null)
        {
            GameObject wave = Instantiate(Wave);
            //test
            if (prebuilt) // if enemy is set, then the wave will run. If not, the wave will not run.
            {
                Wave waveScript = wave.GetComponent<Wave>();
                waveScript.enemy = waveParams.enemy;
                waveScript.count = waveParams.count;
                waveScript.speed = waveParams.speed;
                waveScript.timeBetween = waveParams.timeBetween;
                waveScript.rotationByPath = waveParams.rotationByPath;
                waveScript.Loop = waveParams.Loop;
                waveScript.nextMovement = waveParams.nextMovement;
                waveScript.nextMoveDelay = waveParams.nextMoveDelay;
                waveScript.nextMoveSpeed = waveParams.nextMoveSpeed;
                waveScript.scaleX = waveParams.scaleX;
                waveScript.scaleY = waveParams.scaleY;

            }
            if (wave.GetComponent<Wave>() != null && wave.GetComponent<LevelController>() == null) // only waves should do activate (levelcontrollers should not run this)
                wave.GetComponent<Wave>().Activate();
        }
            
    }

    /*
    //endless coroutine generating 'levelUp' bonuses. 
    IEnumerator PowerupBonusCreation() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(timeForNewPowerup);
            Instantiate(
                powerUp,
                //Set the position for the new bonus: for X-axis - random position between the borders of 'Player's' movement; for Y-axis - right above the upper screen border 
                new Vector2(
                    Random.Range(PlayerMoving.instance.borders.minX, PlayerMoving.instance.borders.maxX), 
                    mainCamera.ViewportToWorldPoint(Vector2.up).y + powerUp.GetComponent<Renderer>().bounds.size.y / 2), 
                Quaternion.identity
                );
        }
    }
    */

    /*
    IEnumerator PlanetsCreation()
    {
        //Create a new list copying the arrey
        for (int i = 0; i < planets.Length; i++)
        {
            planetsList.Add(planets[i]);
        }
        yield return new WaitForSeconds(10);
        while (true)
        {
            ////choose random object from the list, generate and delete it
            int randomIndex = Random.Range(0, planetsList.Count);
            GameObject newPlanet = Instantiate(planetsList[randomIndex]);
            planetsList.RemoveAt(randomIndex);
            //if the list decreased to zero, reinstall it
            if (planetsList.Count == 0)
            {
                for (int i = 0; i < planets.Length; i++)
                {
                    planetsList.Add(planets[i]);
                }
            }
            newPlanet.GetComponent<DirectMoving>().speed = planetsSpeed;

            yield return new WaitForSeconds(timeBetweenPlanets);
        }
    }
    */
}

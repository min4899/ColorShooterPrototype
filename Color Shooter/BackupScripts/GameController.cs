using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject[] hazards;
    public Vector2 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    // Use this for initialization
    void Start () {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        //Debug.Log("Start wait:" + startWait);
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector2 spawnPosition = new Vector2(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y);
                Quaternion spawnRotation = Quaternion.identity; // no rotation
                Instantiate(hazard, spawnPosition, spawnRotation);
                //Debug.Log("Spawn wait:" + spawnWait);
                yield return new WaitForSeconds(spawnWait);
            }
            //Debug.Log("Wave wait:" + waveWait);
            yield return new WaitForSeconds(waveWait);
        }
    }
}

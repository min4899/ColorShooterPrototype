using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    #region FIELDS
    public GameObject obstacle;
    public Transform spawnPoint;

    [Tooltip("a number of enemies in the wave")]
    public int count;

    [Tooltip("how fast the obstacles fall")]
    public float speed;

    [Tooltip("time between emerging of the obstacles/obstacle layout in the wave")]
    public float timeBetween;

    //[Tooltip("if loop is activated, after completing the path 'Enemy' will return to the starting point")]
    //public bool Loop;

    [Tooltip("if testMode is marked the wave will be re-generated after 3 sec")]
    public bool testMode;
    #endregion

    // Use this for initialization
    void Start()
    {
        StartCoroutine(CreateObstacleWave());
    }

    IEnumerator CreateObstacleWave()
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(obstacle, gameObject.transform.position, Quaternion.identity);
            obstacle.SetActive(true);
            yield return new WaitForSeconds(timeBetween);
        }
        if (testMode)       //if testMode is activated, waiting for 3 sec and re-generating the wave
        {
            yield return new WaitForSeconds(3);
            StartCoroutine(CreateObstacleWave());
        }
        else
            Destroy(gameObject); // destroy object once done
    }
}

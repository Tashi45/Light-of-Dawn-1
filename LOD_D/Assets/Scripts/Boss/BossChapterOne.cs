using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChapterOne : MonoBehaviour
{
    public float healt = 100;
    public int phase = 1;
    public float spawnDelay = 0.5f;
    private int currentSpawnIndex = 0;
    public GameObject vinePrefab;
    public GameObject vineUpperPrefab;
    public GameObject rockPrefab;
    public Transform[] vineSpawnOnGroundPoints;
    public Transform[] vineSpawnOnGroundPointsPhase2;
    public Transform[] vineSpawnUpperPoints;
    public Transform[] vineSpawnUpperPointsPhase2;
    public Transform[] rockSpawnPoints;
    public Transform[] rockSpawnPointsPhase2;
    public float attackRate = 0.5f;
    public bool playerAlive = true;
    private float nextAttackTime;
    private bool isSpawningVines = false;
    private float spawnRockTime = 2.5f;
    
    public float damange;

    IEnumerator SpawnVineCycle()
    {
        while (playerAlive)
        {
            isSpawningVines = true;
            currentSpawnIndex = 0;

            yield return StartCoroutine(SpawnVineOnground());
            yield return StartCoroutine(SpawnVineUpper());
            yield return StartCoroutine(SpawnRock());

            
            yield return new WaitForSeconds(attackRate);

            DestroyAllVines();
            isSpawningVines = false;
        }
    }
    
    IEnumerator SpawnVineCyclePhase2()
    {
        while (playerAlive)
        {
            isSpawningVines = true;
            currentSpawnIndex = 0;

            yield return StartCoroutine(SpawnVineOngroundPhase2());
            yield return StartCoroutine(SpawnVineUpperPhase2());
            yield return StartCoroutine(SpawnRockPhase2());
            
            yield return new WaitForSeconds(attackRate);

            DestroyAllVines();
            isSpawningVines = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (healt <= 0)
        {
            Destroy(gameObject);
        }
        
        if (healt <= 50 && phase == 1)
        {
            phase = 2;
        }

        if (phase == 1)
        {
            if (playerAlive && Time.time >= nextAttackTime)
            {
                if (!isSpawningVines && playerAlive && Time.time >= nextAttackTime)
                {
                    StartCoroutine(SpawnVineCycle());
                }
            }
        }

        else if (phase == 2)
        {
            if (playerAlive && Time.time >= nextAttackTime)
            {
                if (!isSpawningVines && playerAlive && Time.time >= nextAttackTime)
                {
                    StartCoroutine(SpawnVineCyclePhase2());
                }
            }
        }
    }
    
    IEnumerator SpawnVineOnground()
    {
        while (currentSpawnIndex < vineSpawnOnGroundPoints.Length)
        {
            Instantiate(vinePrefab, vineSpawnOnGroundPoints[currentSpawnIndex].position, Quaternion.identity);
            currentSpawnIndex++;
            

            yield return new WaitForSeconds(spawnDelay);
        }
        DestroyAllVines();
    }
    
    IEnumerator SpawnVineOngroundPhase2()
    {
        while (currentSpawnIndex < vineSpawnOnGroundPointsPhase2.Length)
        {
            Instantiate(vinePrefab, vineSpawnOnGroundPointsPhase2[currentSpawnIndex].position, Quaternion.identity);
            currentSpawnIndex++;
            yield return new WaitForSeconds(spawnDelay);
        }
        DestroyAllVines();
    }

    void DestroyAllVines()
    {
        GameObject[] vines = GameObject.FindGameObjectsWithTag("Vine");
        foreach (GameObject vine in vines)
        {
            Destroy(vine);
        }
    }
    
    IEnumerator SpawnVineUpper()
    {
        foreach (Transform spawnPoint in vineSpawnUpperPoints)
        {
            Instantiate(vineUpperPrefab, spawnPoint.position, Quaternion.Euler(0,0,180));
        }
        
        yield return null;
    }
    
    IEnumerator SpawnVineUpperPhase2()
    {
        foreach (Transform spawnPoint in vineSpawnUpperPointsPhase2)
        {
            Instantiate(vineUpperPrefab, spawnPoint.position, Quaternion.Euler(0,0,180));
        }
        
        yield return null;
    }
    
    IEnumerator SpawnRock()
    {
        foreach (Transform spawnPoint in rockSpawnPoints)
        {
            Instantiate(rockPrefab, spawnPoint.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(spawnRockTime);

        GameObject[] rocks = GameObject.FindGameObjectsWithTag("Rock");
        foreach (GameObject rock in rocks)
        {
            Destroy(rock);
        }
    }
    
    IEnumerator SpawnRockPhase2()
    {
        foreach (Transform spawnPoint in rockSpawnPointsPhase2)
        {
            Instantiate(rockPrefab, spawnPoint.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(spawnRockTime);

        GameObject[] rocks = GameObject.FindGameObjectsWithTag("Rock");
        foreach (GameObject rock in rocks)
        {
            Destroy(rock);
        }
    }
}

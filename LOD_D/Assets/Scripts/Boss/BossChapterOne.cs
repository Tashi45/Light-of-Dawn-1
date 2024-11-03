using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BossChapterOne : MonoBehaviour
{
    public float bossHealth = 100f;
    public float bossMaxHealth;
    public Image bossHealthBar;

    public GameObject warningPrefab;
    public float warningDuration = 1f;
    
    public Transform[] vineSpawnOnGroundPoints;
    public GameObject vinePrefab;
    public GameObject vineUpperPrefab;
    public float spawnDelay = 4f;
    public int minVinesToSpawn = 1;
    public int maxVinesToSpawn = 3;
    public float damage = 10f;

    public Transform[] vineSpawnGroundPointPhase2;
    public float spawnDelayPhase2 = 2f;
    public int minVinesToSpawnPhase2 = 3;  
    public int maxVinesToSpawnPhase2 = 5;  

    private float startTime;
    public float delayTime = 1f;
    
    private List<int> usedIndices = new List<int>();
    public GameObject[] rockPrefabs;
    public float initialRockDelay = 7f;  // เพิ่มตัวแปรสำหรับ delay ครั้งแรก
    public float rockSpawnInterval = 10f;  // เพิ่มตัวแปรสำหรับระยะเวลาระหว่างชุด
    public float rockLifetime = 15f;
    private bool isFirstRockSpawn = true;  // เพิ่มตัวแปรเช็คว่าเป็นการสร้างครั้งแรกไหม
    private List<KeyValuePair<GameObject, float>> rocksToRemove = new List<KeyValuePair<GameObject, float>>();
    
    private Coroutine rockSpawnCoroutine;
    
    public Transform[] vineSpawnUpperPoints;
    public Transform[] rockSpawnPoints;
    public float attackRate = 3f;
    public float dropRate = 10f;
    public bool playerAlive = true;
    private float nextAttackTime;
    private bool isSpawningVines = false;
    
    private Dictionary<GameObject, float> spawnedRocks = new Dictionary<GameObject, float>();

    void Start()
    {
        startTime = Time.time;
        bossMaxHealth = bossHealth;
        UpdateHealthBar();
        Debug.Log($"Boss Start Health: {bossHealth}"); // เพิ่ม Debug log
    }
    
    public void UpdateHealthBar()
    {
        if (bossHealthBar != null)
        {
            float healthPercentage = Mathf.Clamp01(bossHealth / bossMaxHealth);
            bossHealthBar.fillAmount = healthPercentage;
        }
    }

    public void TakeDamage(float damage)
    {
        bossHealth -= damage;
        bossHealth = Mathf.Max(0, bossHealth); // ป้องกันเลือดติดลบ
        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBar();
        
        if (bossHealth <= 0)
        {
            Destroy(gameObject);
        }

        if (Time.time - startTime >= delayTime)
        {
            if (playerAlive && Time.time >= nextAttackTime)
            {
                if (!isSpawningVines && playerAlive && Time.time >= nextAttackTime)
                {
                    StartCoroutine(SpawnVineCycle());
                    StartCoroutine(SpawnRockAndVineCycle());
                }
            }
            startTime = Time.time;
        }
    }
    
    IEnumerator SpawnVineCycle()
    {
        while (playerAlive)
        {
            isSpawningVines = true;

            // เช็คเลือด boss เพื่อเลือกรูปแบบการโจมตี
            if (bossHealth <= 50f)
            {
                StartCoroutine(SpawnVineOngroundPhase2());
            }
            else
            {
                StartCoroutine(SpawnVineOnground());
            }

            yield return new WaitForSeconds(attackRate);
            isSpawningVines = false;
        }
    }
    
   
    IEnumerator SpawnRockAndVineCycle()
    {
        if (isFirstRockSpawn)
        {
            yield return new WaitForSeconds(initialRockDelay);
            isFirstRockSpawn = false;
        }

        while (playerAlive)
        {
            StartCoroutine(SpawnRock());
            StartCoroutine(SpawnVineUpper());
            yield return new WaitForSeconds(rockSpawnInterval);
        }
    }
    
    IEnumerator SpawnVineOnground()
    {
        HashSet<int> usedIndices = new HashSet<int>();

        int vinesToSpawn = UnityEngine.Random.Range(minVinesToSpawn, maxVinesToSpawn + 1);
        for (int i = 0; i < vinesToSpawn; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = UnityEngine.Random.Range(0, vineSpawnOnGroundPoints.Length);
            } while (usedIndices.Contains(randomIndex));

            usedIndices.Add(randomIndex);

            // สร้าง warning effect
            Vector3 warningPosition = vineSpawnOnGroundPoints[randomIndex].position + new Vector3(0f,-0.7f,0f);
            GameObject warning = Instantiate(warningPrefab, warningPosition, Quaternion.identity);

            
            // รอจนกว่า warning จะหมดเวลา
            yield return new WaitForSeconds(warningDuration);

            // สร้าง vine
            GameObject newVine = Instantiate(vinePrefab, vineSpawnOnGroundPoints[randomIndex].position, Quaternion.identity);
            VineDestructor vineDestructor = newVine.AddComponent<VineDestructor>();
            vineDestructor.lifetime = 3f;

            yield return new WaitForSeconds(spawnDelay);
        }
    }
    
    IEnumerator SpawnVineOngroundPhase2()
    {
        HashSet<int> usedIndices = new HashSet<int>();

        int vinesToSpawn = UnityEngine.Random.Range(minVinesToSpawnPhase2, maxVinesToSpawnPhase2 + 1);
        for (int i = 0; i < vinesToSpawn; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = UnityEngine.Random.Range(0, vineSpawnGroundPointPhase2.Length);
            } while (usedIndices.Contains(randomIndex));

            usedIndices.Add(randomIndex);

            // สร้าง warning effect
            Vector3 warningPosition = vineSpawnGroundPointPhase2[randomIndex].position + new Vector3(0f,-0.7f,0f);
            GameObject warning = Instantiate(warningPrefab, warningPosition, Quaternion.identity);

        
            // รอจนกว่า warning จะหมดเวลา
            yield return new WaitForSeconds(warningDuration);
            
            GameObject newVine = Instantiate(vinePrefab, vineSpawnGroundPointPhase2[randomIndex].position, Quaternion.identity);
            VineDestructor vineDestructor = newVine.AddComponent<VineDestructor>();
            vineDestructor.lifetime = 5f;

            yield return new WaitForSeconds(spawnDelayPhase2);
        }
    }

    int GetRandomIndex()
    {
        int randomIndex;
        do
        {
            randomIndex = UnityEngine.Random.Range(0, vineSpawnOnGroundPoints.Length);
        } while (usedIndices.Contains(randomIndex));

        usedIndices.Add(randomIndex);
        return randomIndex;
    }
    
    IEnumerator SpawnVineUpper()
    {
        foreach (Transform spawnPoint in vineSpawnUpperPoints)
        {
            Instantiate(vineUpperPrefab, spawnPoint.position, Quaternion.Euler(0,0,180));
        }
        
        yield return null;
    }

    IEnumerator SpawnRock()
    {
        List<GameObject> currentRocks = new List<GameObject>();
    
        foreach (Transform spawnPoint in rockSpawnPoints)
        {
            if (!playerAlive) break;
        
            int randomIndex = UnityEngine.Random.Range(0, rockPrefabs.Length);
            GameObject rock = Instantiate(rockPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
        
            if (rock != null)
            {
                Rock rockComponent = rock.GetComponent<Rock>();
                if (rockComponent != null)
                {
                    rockComponent.SetInCollector(false); // เซ็ตค่าเริ่มต้น
                }
                currentRocks.Add(rock);
            }
        }

        yield return new WaitForSeconds(rockLifetime);

        foreach (GameObject rock in currentRocks)
        {
            if (rock != null)
            {
                Rock rockComponent = rock.GetComponent<Rock>();
                if (rockComponent != null && !rockComponent.IsBeingHeld()) // ทำลายเฉพาะหินที่ไม่ได้ถูกถือ
                {
                    Destroy(rock);
                }
            }
        }
        currentRocks.Clear();
    }

    void OnDestroy()
    {
        StopAllCoroutines();

        foreach (var rock in GameObject.FindGameObjectsWithTag("Rock"))
        {
            if (rock != null)
            {
                Rock rockComponent = rock.GetComponent<Rock>();
                if (rockComponent != null && !rockComponent.IsBeingHeld()) // ทำลายเฉพาะหินที่ไม่ได้ถูกถือ
                {
                    Destroy(rock);
                }
            }
        }
    }
}

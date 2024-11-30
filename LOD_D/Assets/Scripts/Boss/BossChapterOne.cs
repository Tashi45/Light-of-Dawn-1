using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class BossChapterOne : MonoBehaviour
{
    private GameObject player;
    private Transform playerTransform;
    public Animator animator;
    public Analytic analytic;
    private bool isInPhase2 = false;  // เพิ่มตัวแปรเช็คว่าอยู่ใน Phase 2 หรือไม่

    [Header("Boss Health Settings")]
    public float bossHealth = 100f;
    public float bossMaxHealth;
    public Image bossHealthBar;
    public GameObject bossDie;

    [Header("Vine warning Settings")]
    public GameObject warningPrefab;
    public float warningDuration = 1f;

    [Header("Vine Phase 1 Settings")]
    public Transform[] vineSpawnOnGroundPoints;
    public GameObject vinePrefab;
    public GameObject vineUpperPrefab;
    public float spawnDelay = 4f;
    public int minVinesToSpawn = 1;
    public int maxVinesToSpawn = 3;
    public float damage = 10f;

    [Header("Vine Phase 2 Settings")]
    public Transform[] vineSpawnGroundPointPhase2;
    public float spawnDelayPhase2 = 2f;
    public int minVinesToSpawnPhase2 = 3;
    public int maxVinesToSpawnPhase2 = 5;

    private float startTime;
    public float delayTime = 1f;

    [Header("Rock Settings")]
    private List<int> usedIndices = new List<int>();
    public GameObject[] rockPrefabs;
    public float initialRockDelay = 7f;
    public float rockSpawnInterval = 10f;
    public float rockLifetime = 15f;
    private bool isFirstRockSpawn = true;
    private List<KeyValuePair<GameObject, float>> rocksToRemove = new List<KeyValuePair<GameObject, float>>();

    private Coroutine rockSpawnCoroutine;
    
    [Header("Tracking Rock Settings")]
    public GameObject trackingRockPrefab;
    public Transform[] trackingRockSpawnPoints;
    [Range(1, 3)]
    public int minTrackingRocks = 1;
    [Range(1, 3)]
    public int maxTrackingRocks = 3;
    public float trackingRockCooldown = 5f;
    private float nextSpawnTime = 0f;
    private List<GameObject> activeTrackingRocks = new List<GameObject>();
    private Coroutine trackingRockCoroutine;

    [Header("Vine and Rock upper Settings")]
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
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        Debug.Log($"Boss Start Health: {bossHealth}");
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
        AudioManager.Instance.PlaySFX("LargeBoss");
        bossHealth = Mathf.Max(0, bossHealth);
        UpdateHealthBar();
    }
    
    void OnEnable()
    {
        TrackingRock.onRockDestroyed += OnTrackingRockDestroyed;
    }

    void OnDisable()
    {
        TrackingRock.onRockDestroyed -= OnTrackingRockDestroyed;
    }

    private void OnTrackingRockDestroyed()
    {
        activeTrackingRocks.RemoveAll(rock => rock == null);
    }

    private IEnumerator SpawnTrackingRockPhase2()
    {
        nextSpawnTime = Time.time;

        while (playerAlive && isInPhase2)  // เปลี่ยนเงื่อนไขการทำงานของ loop
        {
            if (Time.time >= nextSpawnTime)
            {
                int rocksToSpawn = UnityEngine.Random.Range(minTrackingRocks, maxTrackingRocks + 1);
                List<Transform> availableSpawnPoints = new List<Transform>(trackingRockSpawnPoints);
                
                for (int i = 0; i < rocksToSpawn && availableSpawnPoints.Count > 0; i++)
                {
                    int spawnIndex = UnityEngine.Random.Range(0, availableSpawnPoints.Count);
                    Transform selectedSpawnPoint = availableSpawnPoints[spawnIndex];
                    availableSpawnPoints.RemoveAt(spawnIndex);

                    GameObject rockObj = Instantiate(
                        trackingRockPrefab, 
                        selectedSpawnPoint.position, 
                        Quaternion.identity
                    );
                    
                    rockObj.tag = "TrackingRock";
                    
                    TrackingRock rock = rockObj.GetComponent<TrackingRock>();
                    if (rock != null)
                    {
                        rock.Initialize(playerTransform);
                    }
                    
                    activeTrackingRocks.Add(rockObj);
                }

                animator.SetBool("isAttack", true);
                nextSpawnTime = Time.time + trackingRockCooldown;
                yield return new WaitForSeconds(1f);
                animator.SetBool("isAttack", false);
            }
            yield return null;
        }
    }

    void Update()
    {
        UpdateHealthBar();
        animator.SetBool("isAttack", false);

        if (bossHealth <= 0)
        {
            if (animator != null)
            {
                animator.SetBool("isDead", true);
            }
            StartCoroutine(DestroyAfterAnimation());
        }

        // ตรวจสอบการเปลี่ยน Phase
        if (bossHealth <= 50f && !isInPhase2)
        {
            // เข้าสู่ Phase 2
            isInPhase2 = true;
            if (trackingRockCoroutine == null)
            {
                trackingRockCoroutine = StartCoroutine(SpawnTrackingRockPhase2());
            }
        }
        else if (bossHealth > 50f && isInPhase2)
        {
            // กลับสู่ Phase 1
            isInPhase2 = false;
            if (trackingRockCoroutine != null)
            {
                StopCoroutine(trackingRockCoroutine);
                trackingRockCoroutine = null;

                // ทำลาย tracking rocks ที่มีอยู่
                foreach (GameObject rock in activeTrackingRocks.ToList())
                {
                    if (rock != null)
                    {
                        Destroy(rock);
                    }
                }
                activeTrackingRocks.Clear();
            }
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

            if (bossHealth <= 50f)
            {
                StartCoroutine(SpawnVineOngroundPhase2());
                animator.SetBool("isAttack", true);
            }
            else
            {
                StartCoroutine(SpawnVineOnground());
                animator.SetBool("isAttack", true);
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
            AudioManager.Instance.PlaySFX("RockBreak");
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

            Vector3 warningPosition = vineSpawnOnGroundPoints[randomIndex].position + new Vector3(0f, -0.7f, 0f);
            GameObject warning = Instantiate(warningPrefab, warningPosition, Quaternion.identity);
            AudioManager.Instance.PlaySFX("Vines");
            
            yield return new WaitForSeconds(warningDuration);

            GameObject newVine = Instantiate(vinePrefab, vineSpawnOnGroundPoints[randomIndex].position,
                Quaternion.identity);
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

            Vector3 warningPosition = vineSpawnGroundPointPhase2[randomIndex].position + new Vector3(0f, -0.7f, 0f);
            GameObject warning = Instantiate(warningPrefab, warningPosition, Quaternion.identity);
            AudioManager.Instance.PlaySFX("Vines");

            yield return new WaitForSeconds(warningDuration);

            GameObject newVine = Instantiate(vinePrefab, vineSpawnGroundPointPhase2[randomIndex].position,
                Quaternion.identity);
            VineDestructor vineDestructor = newVine.AddComponent<VineDestructor>();
            vineDestructor.lifetime = 5f;

            yield return new WaitForSeconds(spawnDelayPhase2);
        }
    }

    IEnumerator SpawnVineUpper()
    {
        foreach (Transform spawnPoint in vineSpawnUpperPoints)
        {
            Instantiate(vineUpperPrefab, spawnPoint.position, Quaternion.Euler(0, 0, 180));
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
                    rockComponent.SetInCollector(false);
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
                if (rockComponent != null && !rockComponent.IsBeingHeld())
                {
                    Destroy(rock);
                }
            }
        }

        currentRocks.Clear();
    }

    void OnDestroy()
    {
        if (trackingRockCoroutine != null)
        {
            StopCoroutine(trackingRockCoroutine);
            trackingRockCoroutine = null;
        }
        StopAllCoroutines();

        foreach (var rock in GameObject.FindGameObjectsWithTag("Rock"))
        {
            if (rock != null)
            {
                Rock rockComponent = rock.GetComponent<Rock>();
                if (rockComponent != null && !rockComponent.IsBeingHeld())
                {
                    Destroy(rock);
                }
            }
        }

        foreach (GameObject rock in activeTrackingRocks)
        {
            if (rock != null)
            {
                Destroy(rock);
            }
        }
        activeTrackingRocks.Clear();
    }

    
    IEnumerator DestroyAfterAnimation()
    {
        if (trackingRockCoroutine != null)
        {
            StopCoroutine(trackingRockCoroutine);
            trackingRockCoroutine = null;
        }
        yield return new WaitForSeconds(1f);
        analytic.BossDead(1);
        Destroy(gameObject);
        bossDie.SetActive(true);
        SceneManager.LoadScene("BossChangeCutscene");

    }
}
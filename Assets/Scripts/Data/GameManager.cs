using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

enum GameState
{
    Intro,
    Gameplay,
    Boss,
    Pause
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameManagerHolder gameManagerHolder;

    [SerializeField] UIManager uiManager;
    [SerializeField] BossProgress progress;
    [SerializeField] float spawnDistance;

    [SerializeField] List<Transform> spawns;
    [SerializeField] PolygonCollider2D confiner;

    float spawnTimer;

    float currentTimer = 0f;

    public InputReader inputReader;
    public GameObject player { get; private set; }

    List<EnemyStats> enemies;
    int spawnEnemyAmount;

    GameObject boss;
    Witch spawnedBoss;
    bool bossSpawned;
    bool bossReady;
    [SerializeField] float playerBossSpawnOffset; // how far of the boss the player will spawn

    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] CinemachineVirtualCamera bossCamera;
    [SerializeField] GameObject[] cameraBorders;

    GameState gameState;    
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        player = Instantiate(gameManagerHolder.gameParameter.playerPrefab);
        BossProgress.onBossReady.AddListener(ReadyBoss);
        Enemy.onEnemyDeath.AddListener(EnemyDeathEvent);

        Init();
    }

    void Init()
    {
        enemies = gameManagerHolder.gameParameter.enemyList;
        boss = gameManagerHolder.gameParameter.boss;
        spawnTimer = gameManagerHolder.gameParameter.spawnSpeed;
        virtualCamera.Follow = player.transform;
    }

    public void SetPlayer(MorphController player)
    {
        player.healthChangeEvent.AddListener(uiManager.UpdateHealth);
    }

    private void Start()
    {
        gameState = GameState.Gameplay;
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.Intro:
                break;
            case GameState.Gameplay:
                currentTimer += Time.deltaTime;
                
                if (currentTimer > spawnTimer)
                {
                    Spawner.SpawnEnemy(GetWeightedPrefab(), GetRandomSpawn(spawns)).bossProgress = progress;
                    spawnEnemyAmount++;
                    currentTimer = 0f;
                }
                break;
            case GameState.Boss:
                break;
            case GameState.Pause:
                break;
            default:
                break;
        }
    }

    void EnemyDeathEvent(Enemy enm)
    {
        spawnEnemyAmount--;

        if (spawnEnemyAmount == 0)
        {
            BossSpawn();
        }
    }

    public void MassSpawnGolems(int amount, bool instantSpawn = false)
    {
        if (instantSpawn)
        {
            for (int i = 0; i < amount; i++)
            {
                Spawner.SpawnEnemy(GetWeightedPrefab(), GetRandomSpawn(spawns));
            }
        }
        else
        {
            StartCoroutine(DelayMassSpawn(amount));
        }
    }

    public List<Enemy> SpawnEnemy(GameObject enemy, int amount = 1)
    {
        List<Enemy> list = new List<Enemy>();
        for (int i = 0; i<amount; i++)
        {
            list.Add(Spawner.SpawnEnemy(enemy, GetRandomSpawn(spawns)));
        }
        return list;
    }

    public List<Enemy> SpawnEnemyAtSide(GameObject enemy, int amount = 1, string side = "left")
    {
        List<Enemy> list = new List<Enemy>();
        if (side == "left")
        {
            for (int i = 0; i < amount; i++)
            {
                list.Add(Spawner.SpawnEnemy(enemy, spawns[0]));
            }
        }
        else
        {
            for (int i = 0; i < amount; i++)
            {
                list.Add(Spawner.SpawnEnemy(enemy, spawns[1]));
            }
        }
        
        return list;
    }

    IEnumerator DelayMassSpawn(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Spawner.SpawnEnemy(GetWeightedPrefab(), GetRandomSpawn(spawns));
            yield return new WaitForSeconds(1f);
        }
    }

    void ReadyBoss()
    {
        Debug.Log("The boss bar is filled, trying to spawn the boss");
        gameState = GameState.Boss;
        bossReady = true;
        if (spawnEnemyAmount > 0)
        {
            Debug.Log("Could not spawn boss");
            Debug.Log("There are still enemies in the area");
        }
        else
        {
            BossSpawn();
        }
    }

    void BossSpawn()
    {
        Debug.Log("Trying to spawn the boss");

        if (bossSpawned || !bossReady)
        {
            return;
        }
        inputReader.DisableAllInput();

        bossSpawned = true;

        gameState = GameState.Boss;

        spawnedBoss = Spawner.SpawnEnemy(boss, GetRandomSpawn(spawns)).GetComponent<Witch>();


        spawnedBoss.onBossIntroFinished.AddListener(StartBossFight);
        virtualCamera.Follow = spawnedBoss.transform;

        spawnedBoss.onBossHealthChangeEvent.AddListener(progress.UpdateBossHP);

        Invoke("MovePlayerInFrontOfBoss", 1f);
    }

    void MovePlayerInFrontOfBoss() // called by BossSpawn invoke to give a delay
    {
        Vector2 spawnPosition = spawnedBoss.transform.position;
        Vector2 dir = spawnedBoss.transform.position - player.transform.position;
        if (dir.x < 0f)
        {
            spawnPosition.x += playerBossSpawnOffset;
        }
        else
        {
            spawnPosition.x -= playerBossSpawnOffset;
        }

        player.transform.position = spawnPosition;
    }

    void StartBossFight(Transform trans)
    {
        virtualCamera.gameObject.SetActive(false);
        bossCamera.Follow = trans;
        bossCamera.gameObject.SetActive(true);

        Invoke("ActivateCameraBorders", 1f);
    }

    void ActivateCameraBorders()
    {
        for (int i = 0; i < cameraBorders.Length; i++)
        {
            cameraBorders[i].SetActive(true);
        }
        inputReader.EnablePlayerInput();
    }

    Transform GetRandomSpawn(List<Transform> possibleSpawns)
    {
        if (possibleSpawns.Count == 0) return null;

        int rdnIndex = Random.Range(0, possibleSpawns.Count);

        Vector2 point = possibleSpawns[rdnIndex].position;

        List<Transform> UpdatedList = new List<Transform>(possibleSpawns);
        UpdatedList.RemoveAt(rdnIndex);

        if (confiner.OverlapPoint(point))
        {
            return possibleSpawns[rdnIndex];
        }
        else
        {
            return GetRandomSpawn(UpdatedList);
        }
    }

    void EndBossFight()
    {
        SceneLoader.LoadSceneByString("MainMenu");
    }

    GameObject GetWeightedPrefab()
    {
        float weigthSum = 0f;

        foreach (var item in enemies)
        {
            weigthSum += item.spawnWeigth;
        }

        //Debug.Log("Sum of weigth is " + weigthSum);

        float weigth = Random.Range(0, weigthSum);

        //Debug.Log("Weigth to look for is " + weigth);

        for (int i = 0; i < enemies.Count; i++)
        {
            int randomIndex = Random.Range(i, enemies.Count);

            if (weigth < enemies[randomIndex].spawnWeigth)
            {
                //Debug.Log("Successully found an enemy to spawn named " + enemies[randomIndex].name);

                return enemies[randomIndex].prefab;

            }
            else
            {
                weigth -= enemies[randomIndex].spawnWeigth;

                //Debug.Log("Weigth is insuffisante to spawn this enemy called -" + enemies[randomIndex].name + "- reducing weigth to " + weigth);
            }
        }

        //Debug.Log("Did not found any enemy to spawn");

        return null;
    }
}

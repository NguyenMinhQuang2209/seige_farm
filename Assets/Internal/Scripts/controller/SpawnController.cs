using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private float spawnAtHour = 7f;
    [SerializeField] private float timeBwtSpawn = 0f;
    [SerializeField] private float endSpawnDate = 25f;


    [SerializeField] private List<SpawnSetPosition> spawnSetPositions = new();
    [SerializeField] private float offsetY = 1f;
    [SerializeField] private List<DefaultEnemy> enemys = new();
    [SerializeField] private List<SpawnDay> spawnDays = new();


    int currentDay = 0;
    DateTime currentTime;
    DateTime startTime;
    TimeSpan spawnTime;

    int currentIndex = 0;
    float currentSpawnTime = 0f;
    private void Start()
    {
        currentTime = DayLightController.instance.GetCurrentDateTime();
        spawnTime = TimeSpan.FromHours(spawnAtHour);
        startTime = DayLightController.instance.GetStartTime();
        currentSpawnTime = timeBwtSpawn;
        GetEnemyObject();
        currentIndex = 0;
    }
    private void Update()
    {
        currentTime = DayLightController.instance.GetCurrentDateTime();
        if ((currentTime - startTime).Days > endSpawnDate)
        {
            return;
        }
        if (currentTime.TimeOfDay >= spawnTime && (currentTime - startTime).Days >= currentDay)
        {
            CheckSpawnTime();
        }
    }
    private void CheckSpawnTime()
    {
        currentSpawnTime += Time.deltaTime;
        if (currentSpawnTime >= timeBwtSpawn)
        {
            currentSpawnTime = 0f;
            if (spawnDays[currentDay].spawnObjects[currentIndex].quantity <= 0)
            {
                currentIndex = Mathf.Min(currentIndex + 1, spawnDays[currentDay].spawnObjects.Count);
            }
            if (currentIndex == spawnDays[currentDay].spawnObjects.Count)
            {
                startTime = currentTime;
                currentDay = Mathf.Min(currentDay + 1, spawnDays.Count - 1);
                currentIndex = 0;
                return;
            }
            for (int i = 0; i < spawnSetPositions.Count; i++)
            {
                Vector2 offsetX = spawnSetPositions[i].offsetX;
                Vector2 offsetZ = spawnSetPositions[i].offsetZ;
                float randomX = UnityEngine.Random.Range(Mathf.Min(offsetX.x, offsetX.y), Mathf.Max(offsetX.x, offsetX.y));
                float randomZ = UnityEngine.Random.Range(Mathf.Min(offsetZ.x, offsetZ.y), Mathf.Max(offsetZ.x, offsetZ.y));
                Vector3 newPos = new(randomX, offsetY, randomZ);
                if (spawnDays[currentDay].spawnObjects[currentIndex].quantity > 0)
                {
                    Instantiate(spawnDays[currentDay].spawnObjects[currentIndex].enemyObj, newPos, Quaternion.identity);
                    spawnDays[currentDay].spawnObjects[currentIndex].quantity -= 1;
                }
                else
                {
                    break;
                }
            }
        }
    }
    private void GetEnemyObject()
    {
        for (int i = 0; i < spawnDays.Count; i++)
        {
            for (int j = 0; j < spawnDays[i].spawnObjects.Count; j++)
            {
                for (int z = 0; z < enemys.Count; z++)
                {
                    if (spawnDays[i].spawnObjects[j].enemyName.Equals(enemys[z].name))
                    {
                        spawnDays[i].spawnObjects[j].enemyObj = enemys[z].enemyObject;
                        break;
                    }
                }
            }
        }
    }
}
[System.Serializable]
public class DefaultEnemy
{
    public EnemyName name;
    public GameObject enemyObject;
}

[System.Serializable]
public class SpawnObject
{
    public EnemyName enemyName;
    [HideInInspector] public GameObject enemyObj;
    public int quantity = 1;
}

[System.Serializable]
public class SpawnDay
{
    public List<SpawnObject> spawnObjects;
}
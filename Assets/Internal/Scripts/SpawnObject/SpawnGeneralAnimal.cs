using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGeneralAnimal : MonoBehaviour
{
    [SerializeField] private List<SpawnObjectItem> spawnGeneralAnimals = new();
    private List<GameObject> rootParents = new();
    [SerializeField] private List<SpawnSetPosition> spawnSetPositions = new();
    [SerializeField] private float staticY = 10f;
    [SerializeField] private LayerMask checkMask;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private float spawnDayCheck = 3f;
    [SerializeField] private float spawnTimeHour = 1f;
    private DateTime startTime;
    int currentPlusTime = 1;
    TimeSpan spawnTime;
    private void Start()
    {
        spawnTime = TimeSpan.FromHours(spawnTimeHour);
        for (int i = 0; i < spawnGeneralAnimals.Count; i++)
        {
            GameObject emptyObject = new("GeneralAnimalSpawn" + i);
            GameObject obj = Instantiate(emptyObject, new(0f, 0f, 0f), Quaternion.identity, transform);
            rootParents.Add(obj);
            Destroy(emptyObject);
        }
        for (int j = 0; j < spawnGeneralAnimals.Count; j++)
        {
            int spawnQuantity = (int)(Mathf.Ceil(spawnGeneralAnimals[j].quantity / spawnSetPositions.Count));
            for (int i = 0; i < spawnSetPositions.Count; i++)
            {
                Vector2 offsetX = spawnSetPositions[i].offsetX;
                Vector2 offsetZ = spawnSetPositions[i].offsetZ;
                SpawnObjectItem spawnObjectItem = spawnGeneralAnimals[j];
                for (int z = 0; z < spawnQuantity; z++)
                {
                    float randomX = UnityEngine.Random.Range(Mathf.Min(offsetX.x, offsetX.y) * 1f, Mathf.Max(offsetX.x, offsetX.y) * 1f);
                    float randomZ = UnityEngine.Random.Range(Mathf.Min(offsetZ.x, offsetZ.y) * 1f, Mathf.Max(offsetZ.x, offsetZ.y) * 1f);
                    Vector3 newPos = new(randomX, staticY, randomZ);
                    Vector3 checkPos = new(randomX, -1f, randomZ);
                    Collider[] hits = Physics.OverlapCapsule(newPos, checkPos, spawnObjectItem.radious, checkMask);
                    if (hits.Length == 0)
                    {
                        Ray ray = new(newPos, Vector3.down);
                        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
                        {
                            Vector3 spawnPos = hit.point + Vector3.up * 1f;
                            Instantiate(spawnObjectItem.spawnObject, spawnPos, Quaternion.identity, rootParents[j].transform);
                        }
                    }
                }
            }
        }
        startTime = DayLightController.instance.GetStartTime();
    }
    private void Update()
    {
        startTime = DayLightController.instance.GetStartTime();
        DateTime currentTime = DayLightController.instance.GetCurrentDateTime();
        if (currentTime.TimeOfDay >= spawnTime && (currentTime - startTime).Days >= spawnDayCheck * currentPlusTime)
        {
            currentPlusTime += 1;
            RespawnObject();
        }
    }
    private void RespawnObject()
    {
        for (int i = 0; i < spawnGeneralAnimals.Count; i++)
        {
            GameObject rootParent = rootParents[i];
            SpawnObjectItem spawnItem = spawnGeneralAnimals[i];
            if (rootParent.transform.childCount < spawnItem.quantity)
            {
                int spawnQuantity = (int)(Mathf.Ceil((spawnGeneralAnimals[i].quantity - rootParent.transform.childCount) / spawnSetPositions.Count));
                for (int j = 0; j < spawnSetPositions.Count; j++)
                {
                    Vector2 offsetX = spawnSetPositions[j].offsetX;
                    Vector2 offsetZ = spawnSetPositions[j].offsetZ;
                    for (int z = 0; z < spawnQuantity; z++)
                    {
                        float randomX = UnityEngine.Random.Range(Mathf.Min(offsetX.x, offsetX.y) * 1f, Mathf.Max(offsetX.x, offsetX.y) * 1f);
                        float randomZ = UnityEngine.Random.Range(Mathf.Min(offsetZ.x, offsetZ.y) * 1f, Mathf.Max(offsetZ.x, offsetZ.y) * 1f);
                        Vector3 newPos = new(randomX, staticY, randomZ);
                        Vector3 checkPos = new(randomX, -1f, randomZ);
                        Collider[] hits = Physics.OverlapCapsule(newPos, checkPos, spawnItem.radious, checkMask);
                        if (hits.Length == 0)
                        {
                            Ray ray = new(newPos, Vector3.down);
                            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
                            {
                                Vector3 spawnPos = hit.point + Vector3.up * 1f;
                                Instantiate(spawnItem.spawnObject, spawnPos, Quaternion.identity, rootParents[i].transform);
                            }
                        }
                    }
                }
            }
        }
    }
}
[System.Serializable]
public class SpawnSetPosition
{
    public Vector2 offsetX = Vector2.zero;
    public Vector2 offsetZ = Vector2.zero;
}

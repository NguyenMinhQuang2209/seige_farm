using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    [SerializeField] private List<SpawnObjectItem> spawnObjectItems = new List<SpawnObjectItem>();
    private List<GameObject> rootParents = new List<GameObject>();
    [SerializeField] private Vector2 offsetX = Vector2.one;
    [SerializeField] private Vector2 offsetZ = Vector2.one;
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
        SpawnObject();
    }
    private void SpawnObject()
    {
        for (int i = 0; i < spawnObjectItems.Count; i++)
        {
            GameObject emptyObject = new("EmptyObject" + i);
            GameObject obj = Instantiate(emptyObject, new(0f, 0f, 0f), Quaternion.identity, transform);
            rootParents.Add(obj);
            Destroy(emptyObject);
        }
        for (int j = 0; j < spawnObjectItems.Count; j++)
        {
            SpawnObjectItem spawnObjectItem = spawnObjectItems[j];
            for (int i = 0; i < spawnObjectItem.quantity; i++)
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
        spawnTime = TimeSpan.FromHours(spawnTimeHour);
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
        for (int i = 0; i < spawnObjectItems.Count; i++)
        {
            GameObject rootParent = rootParents[i];
            SpawnObjectItem spawnItem = spawnObjectItems[i];
            if (rootParent.transform.childCount < spawnItem.quantity)
            {
                int quantity = spawnItem.quantity - rootParent.transform.childCount;
                for (int j = 0; j < quantity; j++)
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
[System.Serializable]
public class SpawnObjectItem
{
    public GameObject spawnObject;
    public int quantity = 1;
    public float radious = 0.5f;
}

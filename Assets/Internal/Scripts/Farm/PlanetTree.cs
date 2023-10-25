using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetTree : MonoBehaviour
{
    [SerializeField] private PlanetProcessItem processItem;
    public PlanetProcessItem GetPlanetProcess()
    {
        return processItem;
    }
}
[System.Serializable]
public class PlanetProcess
{
    public float growTime = 0f;
    public GameObject planetObject;
}
[System.Serializable]
public class PlanetProcessItem
{
    public List<PlanetProcess> planetProcesses = new List<PlanetProcess>();
    public InventoryItems planetCollect;
    public Vector2 planetCollectRange;
}
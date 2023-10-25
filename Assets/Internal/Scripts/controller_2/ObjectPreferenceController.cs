using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPreferenceController : MonoBehaviour
{
    [SerializeField] private List<ObjectPreferenceList> objects = new();
    [SerializeField] private List<ObjectPreferenceWorldObject> worldObjects = new();

    public static ObjectPreferenceController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public InventoryItems GetPreferenceInventoryItem(ItemType type, ItemName name)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i].type.Equals(type))
            {
                List<ObjectPreferenceInventory> tempObject = objects[i].objectList;
                for (int j = 0; j < tempObject.Count; j++)
                {
                    if (tempObject[j].prefabName.Equals(name))
                    {
                        return tempObject[j].prefab;
                    }
                }
            }
        }
        return null;
    }

    public GameObject GetPreferenceWorldItem(ItemName name)
    {
        for (int i = 0; i < worldObjects.Count; i++)
        {
            if (worldObjects[i].prefabName.Equals(name))
            {
                return worldObjects[i].prefab;
            }
        }
        return null;
    }
}
[System.Serializable]
public class ObjectPreferenceInventory
{
    public InventoryItems prefab;
    public ItemName prefabName;
}
[System.Serializable]
public class ObjectPreferenceList
{
    public ItemType type;
    public List<ObjectPreferenceInventory> objectList = new();
}
[System.Serializable]
public class ObjectPreferenceWorldObject
{
    public GameObject prefab;
    public ItemName prefabName;
}
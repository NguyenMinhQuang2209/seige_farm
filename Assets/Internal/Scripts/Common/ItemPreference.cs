using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPreference : MonoBehaviour
{
    [SerializeField] private List<GameObject> containChild = new List<GameObject>();
    public List<GameObject> GetContainChild()
    {
        return containChild;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextShow : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 1f;
    private void Start()
    {
        Destroy(gameObject, 1.5f);
    }
    private void Update()
    {
        transform.position += floatSpeed * Time.deltaTime * Vector3.up;
    }
}

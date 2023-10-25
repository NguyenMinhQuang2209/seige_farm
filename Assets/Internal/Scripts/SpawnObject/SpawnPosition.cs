using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    [SerializeField] private float radius = 1f;
    
    public Vector3 GetSpawnPosition()
    {
        float randomX = Random.Range(-radius, radius);
        float randomZ = Random.Range(-radius, radius);
        return new Vector3(randomX, 0f, randomZ) + transform.position;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

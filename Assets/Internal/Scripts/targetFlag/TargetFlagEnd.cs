using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFlagEnd : MonoBehaviour
{
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float radiousCheck = 1f;

    [SerializeField] private GameObject dealthUI;
    private void Start()
    {
        dealthUI.SetActive(false);
    }
    private void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radiousCheck, enemyMask);
        if (hits.Length > 0)
        {
            Time.timeScale = 0f;
            dealthUI.SetActive(true);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radiousCheck);
    }
}

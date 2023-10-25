using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private RectTransform userIcons;
    [SerializeField] private float rate = 2f;
    [SerializeField] private GameObject map;
    private void Start()
    {
        player = PreferenceController.instance.Player;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            CursorController.instance.TriggerCursor("Map", new() { map });
        }
        if (player == null)
        {
            player = PreferenceController.instance.Player;
        }
        if (player != null)
        {
            Vector3 pos = player.transform.position;
            float xPos = pos.x / rate;
            float zPos = pos.z / rate;
            RectTransform userIconsRectTransform = userIcons.GetComponent<RectTransform>();
            if (userIconsRectTransform != null)
            {
                userIconsRectTransform.anchoredPosition = new(xPos, zPos);
            }
        }
    }
}

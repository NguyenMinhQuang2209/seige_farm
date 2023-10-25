using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public static CursorController instance;
    private List<GameObject> currentCursor = new List<GameObject>();
    private string currentCursorName = string.Empty;
    private GameObject player;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        player = PreferenceController.instance.Player;
    }
    private void Update()
    {
        if (player == null)
        {
            player = PreferenceController.instance.Player;
        }
        if (player != null && player.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            playerMovement.IsCursorBusy(!currentCursorName.Equals(string.Empty));
        }
        if (currentCursorName.Equals(string.Empty))
        {
            PreviewController.instance.ChangePreviewTxt(string.Empty, transform.position);
        }
    }
    public bool CursorBusy()
    {
        return !currentCursorName.Equals(string.Empty);
    }
    public void TriggerCursor(string itemName, List<GameObject> newItem = null)
    {
        if (currentCursorName.Equals("box"))
        {
            BoxController.instance.UpdateBoxItems();
        }
        if (currentCursorName.Equals(itemName))
        {
            currentCursorName = string.Empty;
            if (currentCursor != null)
            {
                foreach (GameObject obj in currentCursor)
                {
                    if (obj != null)
                    {
                        obj.SetActive(false);
                    }
                }
            }
            return;
        }
        currentCursorName = itemName;
        if (!currentCursorName.Equals(""))
        {
            if (player != null && player.TryGetComponent<PlayerBuilding>(out PlayerBuilding playerBuilding))
            {
                playerBuilding.SwitchBuildingObject(null, null, null);
            }
        }
        if (currentCursor != null && currentCursor.Count > 0)
        {
            foreach (GameObject obj in currentCursor)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
            currentCursor.Clear();
        }
        currentCursor = newItem;
        if (currentCursor != null)
        {
            foreach (GameObject obj in currentCursor)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
        }
    }
    public void CloseAll()
    {
        if (currentCursorName.Equals("box"))
        {
            BoxController.instance.UpdateBoxItems();
        }
        if (currentCursor != null)
        {
            currentCursorName = string.Empty;
            foreach (GameObject obj in currentCursor)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
    public void CloseTag()
    {
        TriggerCursor("", null);
    }
    public string CurrentCursorName()
    {
        return currentCursorName;
    }
}

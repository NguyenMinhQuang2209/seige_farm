using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongPickupItem : MonoBehaviour
{
    public string promptMessage;
    [SerializeField] private int ignoreLayout = 2;
    [SerializeField] private GameObject vfxContainer;
    int defaultLayout;
    bool arePickup = false;


    private Vector3 rootPosition;
    private void Start()
    {
        defaultLayout = gameObject.layer;
        rootPosition = transform.position;
    }
    public void OnDraging()
    {
        gameObject.layer = ignoreLayout;
        GameObject player = PreferenceController.instance.Player;
        if (player.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            playerMovement.DragSollider(true);
        }
        arePickup = true;
        if (vfxContainer != null)
        {
            foreach (Transform child in vfxContainer.transform)
            {
                if (child.TryGetComponent<ParticleSystem>(out ParticleSystem item))
                {
                    item.Play();
                }
            }
        }
    }
    public void OnCancelingDraging()
    {
        gameObject.layer = defaultLayout;
        rootPosition = transform.position;
        arePickup = false;
        if (vfxContainer != null)
        {
            foreach (Transform child in vfxContainer.transform)
            {
                if (child.TryGetComponent<ParticleSystem>(out ParticleSystem item))
                {
                    item.Stop();
                }
            }
        }
        GameObject player = PreferenceController.instance.Player;
        if (player.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            playerMovement.DragSollider(false);
        }
    }
    public Vector3 GetRootPosition()
    {
        return rootPosition;
    }
    public bool ArePickUp()
    {
        return arePickup;
    }
}

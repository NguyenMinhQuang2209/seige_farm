using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Portal : Interactible
{
    [SerializeField] private ParticleSystem effect;

    [SerializeField] private GameObject portalUI;
    [SerializeField] private GameObject changeScreen;
    [SerializeField] private Vector3 bossPos = Vector3.zero;

    private GameObject player;
    [SerializeField] private Boss boss;
    private void Start()
    {
        if (effect != null)
        {
            effect.Play();
        }
        if (portalUI != null)
        {
            portalUI.SetActive(false);
        }
        if (changeScreen != null)
        {
            changeScreen.SetActive(false);
        }
        if (PreferenceController.instance != null)
        {
            player = PreferenceController.instance.Player;
        }
    }
    private void Update()
    {
        if (player == null)
        {
            player = PreferenceController.instance.Player;
        }
    }
    protected override void Interact()
    {
        CursorController.instance.TriggerCursor("Portal", new() { portalUI });
    }

    public void MoveToBossPlace()
    {
        CloseCursor();
        if (player != null && player.TryGetComponent(out PlayerMovement playerMovement))
        {
            playerMovement.ChangePosition(bossPos);
            Invoke(nameof(PlayerMovePosition), 0.5f);
        }
    }
    public void CloseCursor()
    {
        CursorController.instance.CloseTag();
    }
    private void PlayerMovePosition()
    {
        boss.HavingPlayer();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputActions actions;
    public PlayerInputActions.OnFootActions onFoot;
    private PlayerMovement playerMovement;
    private void Awake()
    {
        actions = new PlayerInputActions();
        onFoot = actions.onFoot;
        playerMovement = GetComponent<PlayerMovement>();
        onFoot.Jump.performed += ctx => playerMovement.Jump();
    }
    private void Update()
    {
        Vector2 movement = onFoot.Movement.ReadValue<Vector2>();
        if (movement.y > 0f)
        {
            playerMovement.Movement(1f);
        }
        else if (movement.y < 0f)
        {
            playerMovement.Movement(-1f);
        }
        else
        {
            playerMovement.Movement(0f, false);
        }
        /*if (onFoot.MovementForward.IsPressed())
        {
            playerMovement.Movement(1f);
        }
        else if (onFoot.MovementBackward.IsPressed())
        {
            playerMovement.Movement(-1f);
        }
        else
        {
            playerMovement.Movement(0f, false);
        }*/

    }
    private void OnEnable()
    {
        actions.Enable();
    }
    private void OnDisable()
    {
        actions.Disable();
    }
}

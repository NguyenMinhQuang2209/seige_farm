using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private PlayerInput playerInput;

    [Header("Player Default")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float jumpHeight = 10f;
    [SerializeField] private float smoothTime = 0.1f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.3f;

    [Header("Interactible")]
    [SerializeField] private float interactDistance = 4f;
    [SerializeField] private LayerMask interactMask;
    [SerializeField] private TextMeshProUGUI interactString;
    [SerializeField] private GameObject interactStringContainer;
    [SerializeField] private float offsetYMouse = -4f;
    [SerializeField] private float offsetXMouse = -4f;

    [Header("Player Animation")]
    private GameObject player;
    private Animator animator;

    [Header("Player Use Mana")]
    [SerializeField] private float useManaRate = 1f;
    private PlayerHealth playerHealth;

    [Header("Long pickupItem")]
    [SerializeField] private LayerMask longpickUpMask;


    bool isGround;
    Vector3 velocity;
    float currentVelocity = 0f;
    float currentSpeed = 0f;

    bool isInteractible = false;
    bool isCursorBusy = false;
    bool isBuilding = false;
    bool isDragSollider = false;

    // Drag Item
    private LongPickupItem dragItem;

    bool playerDie = false;
    bool playerDieTrigger = false;
    bool areAttacking = false;

    float plusSpeed = 0f;

    private AudioSource walkMusic;
    private AudioSource runMusic;
    private AudioSource jumpMusic;

    bool walkMusicPlay = false;
    bool runMusicPlay = false;

    bool jumping = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        playerHealth = GetComponent<PlayerHealth>();
        player = transform.GetChild(0).gameObject;
        if (player != null)
        {
            animator = player.GetComponent<Animator>();
        }

        walkMusic = MusicController.instance.walkMusic;
        runMusic = MusicController.instance.runMusic;
        jumpMusic = MusicController.instance.jumpMusic;
    }
    private void Update()
    {
        if (playerHealth != null)
        {
            playerDie = playerHealth.ObjectDie();
        }
        if (playerDie)
        {
            if (!playerDieTrigger)
            {
                animator.SetTrigger("Die");
                playerDieTrigger = true;
            }
            return;
        }

        GravityController();
        animator.SetFloat("Speed", 0f);
        if (dragItem != null)
        {
            if (playerInput.onFoot.DragItem.IsPressed())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    dragItem.transform.position = hit.point;
                }
            }
            else
            {
                dragItem.OnCancelingDraging();
                dragItem = null;
            }
        }
        Interactible();
    }
    private void LateUpdate()
    {
        if (playerHealth != null)
        {
            playerDie = playerHealth.ObjectDie();
        }
        if (playerDie)
        {
            if (!playerDieTrigger)
            {
                animator.SetTrigger("Die");
                playerDieTrigger = true;
            }
            return;
        }

        interactStringContainer.SetActive(interactString.text != string.Empty);
        interactStringContainer.transform.position = Input.mousePosition + new Vector3(offsetXMouse, offsetYMouse, 0f);
        if (playerDie)
        {
            return;
        }
        RotationController();
    }
    public void Movement(float dir = 1f, bool useSpeed = true)
    {
        if (playerHealth != null)
        {
            playerDie = playerHealth.ObjectDie();
        }
        if (playerDie)
        {
            if (!playerDieTrigger)
            {
                animator.SetTrigger("Die");
                playerDieTrigger = true;
            }
            return;
        }

        if (!useSpeed)
        {
            runMusic.Stop();
            walkMusic.Stop();
            walkMusicPlay = false;
            runMusicPlay = false;
            return;
        }
        float currentSpeedTemp;
        if (areAttacking)
        {
            return;
        }
        currentSpeed = moveSpeed + plusSpeed;
        currentSpeedTemp = moveSpeed;
        if (playerInput.onFoot.Run.IsPressed())
        {
            bool canUseMana = playerHealth.UseMana(Time.deltaTime * useManaRate);
            if (canUseMana)
            {
                currentSpeed = runSpeed + plusSpeed;
                currentSpeedTemp = runSpeed;
            }
        }
        if (currentSpeedTemp == moveSpeed)
        {
            if (!walkMusicPlay && !jumping)
            {
                runMusic.Stop();
                walkMusic.Play();
                walkMusic.loop = true;
            }
            walkMusicPlay = true;
            runMusicPlay = false;
        }
        else if (currentSpeedTemp == runSpeed)
        {
            if (!runMusicPlay && !jumping)
            {
                walkMusic.Stop();
                runMusic.Play();
                runMusic.loop = true;
            }
            runMusicPlay = true;
            walkMusicPlay = false;
        }
        animator.SetFloat("Speed", currentSpeedTemp * dir);
        controller.Move(currentSpeed * dir * Time.deltaTime * transform.forward);
    }
    public void Jump()
    {
        if (playerHealth != null)
        {
            playerDie = playerHealth.ObjectDie();
        }
        if (playerDie)
        {
            if (!playerDieTrigger)
            {
                animator.SetTrigger("Die");
                playerDieTrigger = true;
            }
            return;
        }

        if (isGround)
        {
            jumping = true;
            runMusic.Stop();
            walkMusic.Stop();
            jumpMusic.Play();
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("Jump", true);
        }
    }
    private void GravityController()
    {
        isGround = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);
        velocity.y += gravity * Time.deltaTime;
        if (isGround && velocity.y < 0f)
        {
            if (jumping)
            {
                jumping = false;
                if (walkMusicPlay)
                {
                    walkMusic.Play();
                    runMusic.Stop();
                }
                else if (runMusicPlay)
                {
                    walkMusic.Stop();
                    runMusic.Play();
                }
            }
            velocity.y = -2f;
            animator.SetBool("Jump", false);
        }
        controller.Move(velocity * Time.deltaTime);
    }
    public void RotationController()
    {
        if (isCursorBusy)
        {
            return;
        }
        Vector3 mousePosition = Input.mousePosition;
        Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = mousePosition - playerScreenPos;
        float targetAngle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    public void Interactible()
    {
        interactString.text = string.Empty;
        if (isBuilding)
        {
            return;
        }
        if (isCursorBusy)
        {
            return;
        }
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit mouseHit, Mathf.Infinity, interactMask))
        {
            float distance = Vector3.Distance(mouseHit.collider.gameObject.transform.position, transform.position);
            if (mouseHit.collider.gameObject.TryGetComponent<Interactible>(out Interactible target))
            {
                if (target.useDistance)
                {
                    if (distance <= interactDistance)
                    {
                        isInteractible = target.useInteract;
                        interactString.text = target.promptMessage;
                        if (playerInput.onFoot.Interact.triggered)
                        {
                            target.BaseInteract();
                        }
                    }
                    return;
                }
                interactString.text = target.promptMessage;
                if (target.TryGetComponent<ItemUpdating>(out ItemUpdating targetItem))
                {
                    if (targetItem.showInWorld)
                    {
                        interactString.text += "\n" + targetItem.GetLevelShow();
                        if (playerInput.onFoot.UpdateItem.triggered)
                        {
                            targetItem.UpdatingItem();
                        }
                    }
                }
                if (target.TryGetComponent<RecoverHealth>(out RecoverHealth recoverHealth))
                {
                    if (recoverHealth.showInWorld)
                    {
                        interactString.text += "\n" + recoverHealth.GetRecoverHealth();
                        if (playerInput.onFoot.RecoverHealth.triggered)
                        {
                            recoverHealth.RecoverHealthItem();
                        }
                    }
                }
                isInteractible = target.useInteract;
                if (playerInput.onFoot.Interact.triggered)
                {
                    target.BaseInteract();
                }
                return;
            }
        }

        if (Physics.Raycast(mouseRay, out RaycastHit longCasthit, Mathf.Infinity, longpickUpMask))
        {
            if (longCasthit.collider.gameObject.TryGetComponent<LongPickupItem>(out LongPickupItem item))
            {
                interactString.text = item.promptMessage;
                isInteractible = true;
                if (playerInput.onFoot.DragItem.triggered)
                {
                    item.OnDraging();
                    dragItem = item;
                }
                return;
            }
        }
        isInteractible = false;
    }
    public GameObject GetCharacter()
    {
        return player;
    }
    public void ChangeAttackingStatus(bool areAttacking)
    {
        this.areAttacking = areAttacking;
    }
    public bool CanAttack()
    {
        return !isInteractible && !isCursorBusy && !isBuilding && !isDragSollider;
    }
    public void IsInteractible(bool status)
    {
        isInteractible = status;
    }
    public void IsCursorBusy(bool status)
    {
        isCursorBusy = status;
    }
    public void IsBuilding(bool status)
    {
        isBuilding = status;
    }
    public void DragSollider(bool status)
    {
        isDragSollider = status;
    }
    public void ChangePlusSpeed(float speed)
    {
        plusSpeed = speed;
    }
    public void ContactController(bool value)
    {
        controller.enabled = value;
    }

    public void ChangePosition(Vector3 newPos)
    {
        controller.enabled = false;
        transform.position = newPos;
        controller.enabled = true;
    }
}

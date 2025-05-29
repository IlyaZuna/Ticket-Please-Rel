using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Control Panel")]
    public ControlPanel controlPanel;

    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 2f;

    [Header("Camera Settings")]
    public Camera playerCamera;
    public float minVerticalAngle = -90f;
    public float maxVerticalAngle = 90f;

    [Header("Head Bob Settings")]
    public float walkBobSpeed = 14f;
    public float walkBobAmount = 0.05f;
    public float sprintBobSpeed = 18f;
    public float sprintBobAmount = 0.1f;
    private float defaultCameraYPos = 0;
    private float headBobTimer = 0;

    [Header("Interaction")]
    public float interactionDistance = 5f;
    public HintSystem hintSystem; // Ссылка на систему подсказок

    private CharacterController controller;
    private Animator animator;
    private float verticalRotation = 0f;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool isMoving;
    private bool _lockState = false;
    private MapController mapController; // Ссылка на MapController
    private DialogueSystem dialogueSystem; // Ссылка на систему диалогов

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        defaultCameraYPos = playerCamera.transform.localPosition.y;

        // Находим MapController в сцене
        mapController = FindObjectOfType<MapController>();

        // Находим DialogueSystem в сцене
        dialogueSystem = FindObjectOfType<DialogueSystem>();
        if (dialogueSystem == null)
        {
            Debug.LogError("DialogueSystem not found in scene!");
        }

        // Проверяем, что камера привязана
        if (playerCamera == null)
        {
            Debug.LogError("Player Camera is not assigned in FirstPersonController!");
        }

        // Курсор больше не управляется здесь, это делается в DialogueSystem и MapController
        if (hintSystem == null)
        {
            Debug.LogError("HintSystem is not assigned in the Inspector!");
        }
    }

    void Update()
    {
        // Проверяем, открыта ли панель управления
        //if (controlPanel != null && controlPanel.IsPaused()) return; //чек
        if (!_lockState)
        {
            // Проверяем, открыта ли карта
            if (mapController != null && mapController.IsMapOpen()) return;

            HandleRotation();
            HandleMovement();
            HandleJump();
            HandleHeadBob();
            HandleInteraction();
        }
    }

    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up, mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    private void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        float speed = isSprinting ? sprintSpeed : walkSpeed;
        isMoving = new Vector2(horizontal, vertical).magnitude > 0.1f;

        Vector3 move = transform.forward * vertical + transform.right * horizontal;
        controller.Move(move * speed * Time.deltaTime);

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (animator != null)
        {
            animator.SetBool("isWalking", isMoving && !isSprinting);
            animator.SetBool("isRunning", isMoving && isSprinting);
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            if (animator != null) animator.SetTrigger("isJumping");
        }
    }

    private void HandleHeadBob()
    {
        if (!isGrounded || !isMoving)
        {
            Vector3 cameraZPos = playerCamera.transform.localPosition;
            cameraZPos.y = Mathf.Lerp(cameraZPos.y, defaultCameraYPos, Time.deltaTime * 5f);
            playerCamera.transform.localPosition = cameraZPos;
            return;
        }

        float bobSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintBobSpeed : walkBobSpeed;
        float bobAmount = Input.GetKey(KeyCode.LeftShift) ? sprintBobAmount : walkBobAmount;

        headBobTimer += Time.deltaTime * bobSpeed;
        float newYPos = defaultCameraYPos + Mathf.Sin(headBobTimer) * bobAmount;

        Vector3 cameraPos = playerCamera.transform.localPosition;
        cameraPos.y = Mathf.Lerp(cameraPos.y, newYPos, Time.deltaTime * 10f);
        playerCamera.transform.localPosition = cameraPos;
    }

    private void HandleInteraction()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.CompareTag("BusStop")) return;

            // Показываем подсказку через HintSystem
            if (hintSystem != null)
            {
                hintSystem.ShowHint(hit.collider.gameObject);
            }

            Debug.Log($"Рэй попал в объект: {hit.collider.name}");

            if (Input.GetKeyDown(KeyCode.E) && hit.collider.TryGetComponent(out IInteractable interactable))
            {
                // Блокируем движение игрока перед началом диалога
                LockStatePlayer();

                // Запускаем взаимодействие (диалог)
                interactable.Interact();
            }
        }
        else
        {
            // Скрываем подсказку, если луч никуда не попал
            if (hintSystem != null)
            {
                hintSystem.HideHint();
            }
        }
    }

    public bool IsMovementLocked()
    {
        return _lockState;
    }
    public void LockStatePlayer() => _lockState = !_lockState;
}
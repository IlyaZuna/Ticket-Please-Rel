using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    public float moveSpeed = 5f;        // �������� ������������
    public float multiplySpeed = 5f;
    public float mouseSensitivity = 2f; // ���������������� ����
    public Transform cameraRig;         // ������, � �������� ��������� ������
    public float cameraDistance = 4f;   // ���������� ������ �� ���������
    public float cameraHeight = 2f;     // ������ ������ ������������ ���������
    public bool _lockState = false;
    private CharacterController controller;
    private Animator animator;          // ��������� ��������
    [SerializeField] private Camera playerCamera; // ������ ������
    [SerializeField] private float interactionDistance = 5f; // ��������� ��������������

    private float yaw = 0f;             // ���� �������� ������ �� ��� Y

    void Start()
    {     
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // ������������� Animator
        Cursor.lockState = CursorLockMode.Locked; // ��������� � �������� ������
        Cursor.visible = false; // ���������, ��� ������ �����
    }

    void Update()
    {
        if (!_lockState)
        {
            RotateCharacter();
            MoveCharacter();
            UpdateCameraPosition();
            RayCaster();
        }
    }

    private void RotateCharacter()
    {
        // �������� ���� � ���� ��� �������� �� ��� Y
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;

        // ��������� ���� �������� ������ (yaw)
        yaw += mouseX;

        // ������������ ��������� �� ��� Y
        transform.Rotate(Vector3.up, mouseX);
    }

    private void MoveCharacter()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? moveSpeed * multiplySpeed : moveSpeed;

        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        bool isMoving = moveDirection.magnitude > 0;

        // ���������� ����������
        animator.SetBool("isWalking", isMoving && !isRunning);
        animator.SetBool("isRunning", isMoving && isRunning);

        // ������� ��� �������� �������
        //Debug.Log("isWalking: " + (isMoving && !isRunning));
        //Debug.Log("isRunning: " + (isMoving && isRunning));
    }


    private void UpdateCameraPosition()
    {
        // ��������� ������� ������ ������������ ���������
        Vector3 targetPosition = transform.position - cameraRig.forward * cameraDistance + Vector3.up * cameraHeight;

        // ������� ������
        cameraRig.position = targetPosition;

        // ������ ������ �������� �� ���������
        cameraRig.LookAt(transform.position + Vector3.up * cameraHeight);
    }

    public void LockStatePlayer()
    {
        _lockState = !_lockState;
    }
    private void RayCaster()
    {
        Vector3 rayOrigin = playerCamera.transform.position;
        Vector3 rayDirection = playerCamera.transform.forward; // ����������� ������ �� ������

        Ray ray = new Ray(rayOrigin, rayDirection);
        int layerMask = ~LayerMask.GetMask("Bus"); // ���������� ���� "Bus"

        RaycastHit hit;

        // ������ �����, ������� ����� ������������
        string[] ignoredTags = { "BusStop" };

        // ��������� ������� � ������������ ���������
        if (Physics.Raycast(ray, out hit, interactionDistance, layerMask))
        {
            // ���������, ����� �� ��� � ������ � ������������ �����
            foreach (string tag in ignoredTags)
            {
                if (hit.collider.CompareTag(tag))
                {
                    return; // ������� �� ������, ���� ������ ����� ������������ ���
                }
            }

            Debug.Log($"��� ����� � ������: {hit.collider.name}");

            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (Input.GetKeyDown(KeyCode.E) && interactable != null)
            {
                interactable.Interact();
            }           
        }
    }
}
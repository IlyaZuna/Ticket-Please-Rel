using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    public float moveSpeed = 5f;        // Скорость передвижения
    public float mouseSensitivity = 2f; // Чувствительность мыши
    public Transform cameraRig;         // Объект, к которому привязана камера
    public float cameraDistance = 4f;   // Расстояние камеры от персонажа
    public float cameraHeight = 2f;     // Высота камеры относительно персонажа

    private CharacterController controller;
    private Animator animator;          // Компонент анимации

    private float yaw = 0f;             // Угол поворота камеры по оси Y

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // Инициализация Animator
        Cursor.lockState = CursorLockMode.Locked; // Блокируем и скрываем курсор
        Cursor.visible = false; // Убедитесь, что курсор скрыт
    }

    void Update()
    {
        RotateCharacter();
        MoveCharacter();
        UpdateCameraPosition();
    }

    private void RotateCharacter()
    {
        // Получаем ввод с мыши для поворота по оси Y
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;

        // Обновляем угол поворота камеры (yaw)
        yaw += mouseX;

        // Поворачиваем персонажа по оси Y
        transform.Rotate(Vector3.up, mouseX);
    }

    private void MoveCharacter()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? moveSpeed * 2 : moveSpeed;

        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        bool isMoving = moveDirection.magnitude > 0;

        // Управление анимациями
        animator.SetBool("isWalking", isMoving && !isRunning);
        animator.SetBool("isRunning", isMoving && isRunning);

        // Отладка для проверки условий
        //Debug.Log("isWalking: " + (isMoving && !isRunning));
        //Debug.Log("isRunning: " + (isMoving && isRunning));
    }


    private void UpdateCameraPosition()
    {
        // Вычисляем позицию камеры относительно персонажа
        Vector3 targetPosition = transform.position - cameraRig.forward * cameraDistance + Vector3.up * cameraHeight;

        // Позиция камеры
        cameraRig.position = targetPosition;

        // Камера должна смотреть на персонажа
        cameraRig.LookAt(transform.position + Vector3.up * cameraHeight);
    }
}
using UnityEngine;

public class DoorAnimationController : MonoBehaviour {
    public BusController controller;
    public Animator door1Animator;
    public Animator door2Animator;
    public Animator door3Animator;
    public Animator door4Animator;

    public string door1AnimationName = "Open_1";
    public string door2AnimationName = "Open_2";
    public string door3AnimationName = "Open_3";
    public string door4AnimationName = "Open_4";

    bool areDoorsOpen = false;

    [Range(0.1f, 1.0f)]
    public float animationSpeed = 0.5f; // скорость анимации

    void Start() {
        // Устанавливаем начальную скорость для всех анимаций
        SetAnimationSpeed(animationSpeed);
    }
    
    void Update() {
        bool open = controller.areDoorsOpen;
        if (open)
        {
            PlayDoorAnimations();
        }
        
    }

    void PlayDoorAnimations() {
        // Запускаем каждую анимацию по её имени
        door1Animator.Play(door1AnimationName, 0);
        door2Animator.Play(door2AnimationName, 0);
        door3Animator.Play(door3AnimationName, 0);
        door4Animator.Play(door4AnimationName, 0);

    }

    void SetAnimationSpeed(float speed) {
        // Устанавливаем скорость для всех анимаций
        door1Animator.speed = speed;
        door2Animator.speed = speed;
        door3Animator.speed = speed;
        door4Animator.speed = speed;
    }

    // Этот метод можно использовать для изменения скорости во время игры
    public void UpdateAnimationSpeed(float newSpeed) {
        animationSpeed = newSpeed;
        SetAnimationSpeed(animationSpeed);
    }
}

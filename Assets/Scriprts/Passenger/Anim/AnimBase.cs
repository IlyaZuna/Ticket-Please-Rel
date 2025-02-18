using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimBase : MonoBehaviour
{
    [SerializeField] private Animator animator;
    void Start()
    {
        // Получаем компонент Animator на объекте
        animator = GetComponent<Animator>();
    }
    public void Idle()
    {
        ResetAllStates();
        animator.SetBool("idle", true);
    }
    public void Walk()
    {
        ResetAllStates();
        animator.SetBool("walk", true);
    }
    public void Sit()
    {
        ResetAllStates();
        animator.SetBool("sit", true);
    }
    private void ResetAllStates()
    {
        animator.SetBool("idle", false);
        animator.SetBool("walk", false);
        animator.SetBool("sit", false);
    }
}

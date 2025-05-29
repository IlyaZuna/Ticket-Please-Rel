using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusDoorController : MonoBehaviour
{
    // ������ �� ��������� Animator
    [SerializeField]private Animator animator;
    [SerializeField] private AudioClip open;
    private AudioSource audioSource;
    // ��� �������� ��� �������� ������
    private const string OpenDoorsTrigger = "OpenDoors";
    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator �� ������! �������� ��������� Animator � �����.");
        }
        


    }
    public void ToggleDoor()
    {        
        animator.SetTrigger(OpenDoorsTrigger); // ���������� ������� ��� ��������
       
    }
}
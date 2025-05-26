using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusDoorController : MonoBehaviour
{
    // ������ �� ��������� Animator
    private Animator animator;
    // ��� �������� ��� �������� ������
    private const string OpenDoorsTrigger = "OpenDoors";
    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator �� ������! �������� ��������� Animator � �����.");
        }
        ButtonDoor.OnButtonPressed += ToggleDoor;
    }
    public void ToggleDoor()
    {        
        animator.SetTrigger(OpenDoorsTrigger); // ���������� ������� ��� ��������
    }
}
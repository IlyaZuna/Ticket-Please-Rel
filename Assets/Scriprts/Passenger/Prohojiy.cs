using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Prohojiy : MonoBehaviour
{
    [SerializeField] public AnimBase animator;
    [SerializeField] private Transform[] WalkPoint;
    private Transform target;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (WalkPoint != null && WalkPoint.Length != 0)
        {
            target = WalkPoint[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
    }
    private void Walk()
    {
        agent.SetDestination(target.position);
        animator.Walk();
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToPoint();  // Выбираем новую точку, когда дошли
        }
    }
    private void MoveToPoint()
    {
        if (target = WalkPoint[0])
        {
            target = WalkPoint[1];
        }
        else
        {
            target = WalkPoint[0];
        }
    }
}

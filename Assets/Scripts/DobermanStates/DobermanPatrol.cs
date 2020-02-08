using UnityEngine;
using System.Collections;

public class DobermanPatrol : IDobermanState
{
    private Doberman doberman;

    private float patrolTimer;

    private float patrolDuration = 5f;

    public void Enter(Doberman doberman)
    {
        this.doberman = doberman;
    }

    public void Execute()
    {
        Patrol();

        doberman.Move();

        if (doberman.Target != null)
        {
            doberman.ChangeState(new DobermanMeeleeState());
        }
    }

    public void Exit()
    {

    }

    public void OnTriggerEnter(Collider2D other)
    {
        if (other.tag == "Edge")
        {
            doberman.ChangeDirection();
        }
    }

    private void Patrol()
    {
        doberman.MyAnimator.SetFloat("speed", 0);

        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolDuration)
        {
            doberman.ChangeState(new DobermanIdleState());
        }
    }
}

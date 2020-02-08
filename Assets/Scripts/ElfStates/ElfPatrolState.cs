using UnityEngine;
using System.Collections;
using System;

public class ElfPatrolState : IElfState
{
    private Elf elf;

    private float patrolTimer;

    private float patrolDuration = 5f;

    public void Enter(Elf elf)
    {
        this.elf = elf;
    }

    public void Execute()
    {
        Debug.Log("Elf's patrolling");

        Patrol();

        elf.RunAway();
    }

    public void Exit()
    {

    }

    public void OnTriggerEnter(Collider2D other)
    {

    }

    private void Patrol()
    {
        elf.MyAnimator.SetFloat("speed", 0);
        
        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolDuration)
        {
            elf.ChangeElfState(new ElfIdleState());
        }
    }
}

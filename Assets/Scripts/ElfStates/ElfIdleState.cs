using UnityEngine;
using System.Collections;
using System;

public class ElfIdleState : IElfState
{
    private Elf elf;

    //so it doesn't enter a loop state, we're making our own timer
    private float idleTimer;

    private float idleDuration = 5f;

    public void Enter(Elf elf)
    {
        this.elf = elf;
    }

    public void Execute()
    {
        ElfIdle();

        if (elf.Target != null)
        {
            elf.ChangeElfState(new ElfPatrolState());
        }
    }

    public void Exit()
    {

    }

    public void OnTriggerEnter(Collider2D other)
    {

    }

    private void ElfIdle()
    {
        elf.MyAnimator.SetFloat("speed", 0);

        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDuration)
        {
            elf.ChangeElfState(new ElfPatrolState());
        }
    }
}

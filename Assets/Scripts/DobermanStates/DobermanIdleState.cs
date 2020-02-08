using UnityEngine;
using System.Collections;
using System;

public class DobermanIdleState : IDobermanState
{
    private Doberman doberman;

    //so it doesn't enter a loop state, we're making our own timer
    private float idleTimer;

    private float idleDuration = 5f;

    void IDobermanState.Enter(Doberman doberman)
    {
        this.doberman = doberman;
    }

    void IDobermanState.Execute()
    {
        Idle();

        if (doberman.Target != null)
        {
            doberman.ChangeState(new DobermanMeeleeState());
        }
    }

    void IDobermanState.Exit()
    {

    }

    void IDobermanState.OnTriggerEnter(Collider2D other)
    {
 
    }

    private void Idle()
    {
        doberman.MyAnimator.SetFloat("speed", 0);

        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDuration)
        {
            doberman.ChangeState(new DobermanMeeleeState());
        }
    }
}

using UnityEngine;
using System.Collections;
using System;

public class DobermanFollow : IDobermanState
{
    private Doberman doberman;

    public void Enter(Doberman doberman)
    {
        this.doberman = doberman;
    }

    public void Execute()
    {
        doberman.Move();
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
}

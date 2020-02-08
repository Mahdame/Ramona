using UnityEngine;
using System.Collections;
using System;

public class DobermanMeeleeState : IDobermanState
{
    private Doberman doberman;

    //private float throwTimer;

    //private float throwCoolDown = 1f;

    //private bool canThrow = true;

    void IDobermanState.Enter(Doberman doberman)
    {
        this.doberman = doberman;
    }

    void IDobermanState.Execute()
    {
        //Throw();

        if (doberman.Target != null)
        {
            //doberman.Move();
            doberman.MyAnimator.SetTrigger("attack");
        }
        else
        {
            doberman.ChangeState(new DobermanIdleState());
        }
    }

    void IDobermanState.Exit()
    {

    }

    void IDobermanState.OnTriggerEnter(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            doberman.MyAnimator.SetTrigger("attack");
        }
    }

    private void Throw()
    {
        //throwTimer += Time.deltaTime;

        //if (throwTimer >= throwCoolDown)
        //{
        //    canThrow = true;
        //    throwTimer = 0;
        //}

        //if (canThrow)
        //{
        //    canThrow = false;
        //    doberman.MyAnimator.SetTrigger("attack");
        //}
    }
}

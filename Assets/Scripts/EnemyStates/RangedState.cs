using UnityEngine;
using System.Collections;
using System;

public class RangedState : IEnemyState
{
    private Enemy enemy;

    //private float throwTimer;

    //private float throwCoolDown = 1f;

    //private bool canThrow = true;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        //Throw();

        if (enemy.Target != null)
        {
            enemy.Move();
        }
        else
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {

    }

    public void OnTriggerEnter(Collider2D other)
    {
        
    }

    //private void Throw()
    //{
    //    throwTimer += Time.deltaTime;

    //    if (throwTimer >= throwCoolDown)
    //    {
    //        canThrow = true;
    //        throwTimer = 0;
    //    }

    //    if (canThrow)
    //    {
    //        canThrow = false;
    //        enemy.MyAnimator.SetTrigger("throw");
    //    }
    //}
}

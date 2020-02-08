using UnityEngine;
using System.Collections;
using System;

public class MeleeState : IEnemyState
{
    private float attackTimer;

    private float attackCoolDown = 1f;

    private bool canAttack = true;

    private Enemy enemy;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Attack();

        if (enemy.InThrowRange && !enemy.InMeleeRange)
        {
            //enemy.ChangeState(new RangedState());
            enemy.ChangeState(new PatrolState());
        }
        else if (enemy.Target == null)
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

    private void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCoolDown)
        {
            canAttack = true;
            attackTimer = 0;
        }

        if (canAttack)
        {
            canAttack = false;
            //AUMENTAR A VELOCIDADE QUANDO FOR ATACAR
            enemy.MyAnimator.SetTrigger("attack");
        }
    }
}

using UnityEngine;
using System.Collections;
using System;

////Adiciona um componente do tipo Rigidbody2D quando o Script for colocado.
//[RequireComponent(typeof(Rigidbody2D))]
public class Doberman : Character
{
    private static Doberman instance;

    public static Doberman Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Doberman>();
            }

            return instance;
        }
    }

    private IDobermanState currentState;

    private float lifeTimer;

    public GameObject Target { get; set; }

    public override bool IsDead
    {
        get
        {
            return health <= 0;
        }
    }

    public override void Start ()
    {
        base.Start();

        ChangeState(new DobermanIdleState());
    }

    void Update()
    {
        if (!IsDead)
        {
            if (!TakingDamage)
            {
                currentState.Execute();
            }

            LookAtTarget();

            Life();
        }

        if (IsDead)
        {
            Death();
        }
    }

    private void LookAtTarget()
    {
        if (Target != null)
        {
            float xDir = Target.transform.position.x - transform.position.x;

            if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
            {
                ChangeDirection();
            }
        }

        ChangeState(new DobermanPatrol());
    }

    public void ChangeState(IDobermanState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter(this);
    }

    public void Move()
    {
        if (!Attack)
        {
            MyAnimator.SetFloat("speed", 1);

            transform.Translate(GetDirection() * (movementSpeed * Time.deltaTime));
        }
    }

    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter(other);
    }

    public override IEnumerator TakeDamage()
    {
        health -= 10;

        if (!IsDead)
        {
            //Damage animation
        }
        else
        {
            MyAnimator.SetTrigger("death");
            yield return null;
        }
    }

    public void Life()
    {
        lifeTimer += Time.deltaTime;
        //Debug.Log(lifeTimer);

        if (lifeTimer >= 20)
        {
            health = 0;
            MyAnimator.SetTrigger("death");
        }
    }

    /// <summary>
    ///  Quando sair da tela, é deletado.
    /// </summary>
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public override void Death()
    {
        ChangeState(new DobermanIdleState());
        MyAnimator.SetTrigger("death");
        Destroy(gameObject, 3f);
    }
}

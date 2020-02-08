using UnityEngine;
using System.Collections;
using System;

public class Enemy : Character
{
    private IEnemyState currentState;

    public GameObject Target { get; set; }

    [SerializeField]
    private float meleeRange;

    [SerializeField]
    private float throwRange;

    public bool InMeleeRange
    {
        get
        {
            //if the distance between the target and the player is <= meleeRange it will return true
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;
            }

            return false;
        }
    }

    public bool InThrowRange
    {
        get
        {
            //if the distance between the target and the player is <= meleeRange it will return true
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= throwRange;
            }

            return false;
        }
    }

    public override bool IsDead
    {
        get
        {
            return health <= 0;
        }
    }

    private Vector3 startPos;

    [SerializeField]
    private Transform leftEdge;

    [SerializeField]
    private Transform rightEdge;

    // Use this for initialization
    public override void Start ()
    {
        startPos = transform.position;

        base.Start();

        //RemoveTarget will be called whenever the event 'Dead' is triggered
        Player.Instance.Dead += new DeadEventHandler(RemoveTarget);

        ChangeState(new IdleState());
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!IsDead)
        {
            if (!TakingDamage)
            {
                currentState.Execute();
            }

            LookAtTarget();
        }

        if (IsDead)
        {
            Destroy(gameObject, 1f);
        }
	}

    public void RemoveTarget()
    {
        Target = null;
        ChangeState(new PatrolState());
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
    }

    public void ChangeState(IEnemyState newState)
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
            if (transform.right.x > 0 && transform.position.x < rightEdge.position.x || transform.right.x < 0 && transform.position.x > leftEdge.position.x)
            //if (GetDirection().x > 0 && transform.position.x < rightEdge.position.x || GetDirection().x < 0 && transform.position.x > leftEdge.position.x)
            {
                MyAnimator.SetFloat("speed", 1);

                transform.Translate(transform.right * movementSpeed * Time.deltaTime);

                //transform.Translate(GetDirection() * (movementSpeed * Time.deltaTime));
            }
            else if (currentState is PatrolState)
            {
                ChangeDirection();
            }
        }
    }

    //public Vector2 GetDirection()
    //{
    //    return facingRight ? Vector2.right : Vector2.left;
    //}

    /// <summary>
    /// If the enemy collides with an object
    /// </summary>
    /// <param name="other">The colliding object</param>
    public override void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Hit");
        //calls the base on trigger enter
        base.OnTriggerEnter2D(other);

        //calls OnTriggerEnter on the current state
        currentState.OnTriggerEnter(other);
    }

    public override IEnumerator TakeDamage()
    {
        health -= 10;

        if (!IsDead)
        {
            MyAnimator.SetTrigger("damage");
        }
        else
        {
            MyAnimator.SetTrigger("death");
            yield return null;
        }
    }

    public override void Death()
    {
        MyAnimator.ResetTrigger("death");
        MyAnimator.SetTrigger("idle");
        health = 30;
        transform.position = startPos;
    }
}

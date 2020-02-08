using UnityEngine;
using System.Collections;
using System;

public class Elf : Character
{
    private IElfState currentElfState;

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

    //private Vector3 startPos;

    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Transform leftEdge;

    [SerializeField]
    private Transform rightEdge;

    [SerializeField]
    private bool dropsLoot;

    [SerializeField]
    private GameObject moneyBag;

    // Use this for initialization
    public override void Start()
    {
        //startPos = transform.position;

        base.Start();

        facingRight = true;

        ChangeElfState(new ElfIdleState());

        Player.Instance.elfIsAlive = true;

        spriteRenderer = GetComponent<SpriteRenderer>();

        //RemoveTarget will be called whenever the event 'Dead' is triggered
        Player.Instance.Dead += new DeadEventHandler(RemoveTarget);

        ChangeElfState(new ElfIdleState());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentElfState);

        if (!IsDead)
        {
            if (!TakingDamage)
            {
                currentElfState.Execute();
            }

            LookAtTarget();
        }
        else
        {
            Death();
        }
    }

    public void RemoveTarget()
    {
        Target = null;
        ChangeElfState(new ElfPatrolState());
    }

    private void LookAtTarget()
    {
        if (Target != null)
        {

        }

        //if (Target != null)
        //{
        //    float xDir = Target.transform.position.x - transform.position.x;

        //    if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
        //    {
        //        ChangeDirection();
        //    }
        //}
    }

    public void ChangeElfState(IElfState newElfState)
    {
        if (currentElfState != null)
        {
            currentElfState.Exit();
        }

        currentElfState = newElfState;

        currentElfState.Enter(this);
    }

    public void Move()
    {
        if (facingRight)
        {
            transform.Translate(Vector2.right * movementSpeed * Time.deltaTime);

            MyAnimator.SetFloat("speed", 1);
        }
        else
        {
            transform.Translate(Vector2.left * movementSpeed * Time.deltaTime);

            MyAnimator.SetFloat("speed", 1);
        }

        //if (Vector2.right.x > 0 && transform.position.x < rightEdge.position.x || Vector2.right.x < 0 && transform.position.x > leftEdge.position.x)
        //{
        //    MyAnimator.SetFloat("speed", 1);

        //    transform.Translate(Vector2.right * movementSpeed * Time.deltaTime);

        //    //Debug.Log("Andando");
        //}
        //else if (currentElfState is ElfPatrolState)
        //{
        //    ChangeDirection();
        //}
    }

    public void RunAway()
    {
        Vector3 moveDir = Player.Instance.transform.position - transform.position;

        transform.Translate(moveDir.normalized * movementSpeed * Time.deltaTime);

        MyAnimator.SetFloat("speed", 1);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            Player.Instance.ElfCoins++;

            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Player")
        {
            ChangeDirection();
            //ChangeElfState(new ElfPatrolState());
        }
    }

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
        currentElfState.OnTriggerEnter(other);
    }

    private IEnumerator IndicateImmortal()
    {
        while (TakingDamage)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(.1f);

            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(.1f);
        }
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

        //If dropsLoot is true he'll drop a MoneyBag
        if (dropsLoot) Instantiate(moneyBag, transform.position, transform.rotation);

        //Elf disappears/dies
        MyAnimator.gameObject.SetActive(false);

        //transform.position = startPos;        
    }
}


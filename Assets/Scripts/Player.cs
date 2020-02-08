using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public delegate void DeadEventHandler();

public class Player : Character
{
    private static Player instance;

    public event DeadEventHandler Dead;

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }

            return instance;
        }
    }

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpForce;

    private bool immortal = false;

    [SerializeField]
    private float immortalTime;

    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float doubleJumpForce;

    private Vector3 startPos;

    //Doberman
    [SerializeField]
    private GameObject dobermanPrefab;

    [SerializeField]
    private Transform dobermanpoint;

    //with capital letters to differ properties from variables
    //using properties to access them from other places
    public Rigidbody2D MyRigidbody { get; set; }

    public bool Slide { get; set; }

    public bool Jump { get; set; }

    public bool DoubleJump { get; set; }

    public bool Crouch { get; set; }

    public bool Carry { get; set; }

    public bool Throw { get; set; }

    public bool OnGround { get; set; }

    public override bool IsDead
    {
        get
        {
            if (health <= 0)
            {
                OnDead();
            }

            return health <= 0;
        }
    }

    //Grab
    public bool grabbed;
    RaycastHit2D hit;
    public float distance = 2f;
    public Transform holdpoint;
    public float throwforce;
    public LayerMask notgrabbed;

    //Coins
    private int score;

    public bool elfIsAlive;

    [SerializeField]
    private GameObject coinPrefab;

    [SerializeField]
    private GameObject moneyBag;

    [SerializeField]
    private Text coinTxt;

    private int collectedCoins;

    private int elfCoins;

    public int CollectedCoins
    {
        get
        {
            return collectedCoins;
        }

        set
        {
            coinTxt.text = value.ToString();
            collectedCoins = value;
        }
    }

    public int ElfCoins
    {
        get
        {
            return elfCoins;
        }

        set
        {
            elfCoins = value;
        }
    }

    public override void Start()
    {
        base.Start();

        startPos = transform.position;
        MyRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!IsDead && !TakingDamage)
        {
            if (transform.position.y <= -0.1f)
            {
                Death();
            }
        }

        HandleInput();

        ToGrab();
    }

    void FixedUpdate()
    {
        if (!TakingDamage)
        {
            float horizontal = Input.GetAxis("Horizontal");

            OnGround = IsGrounded();

            HandleMovement(horizontal);

            Flip(horizontal);

            HandleLayers();
        }

        Score();
        //Debug.Log(MyRigidbody.velocity.y);
    }

    public void OnDead()
    {
        if (Dead != null)
        {
            Dead();
        }
    }

    private void Score()
    {
        if (elfIsAlive)
        {
            score = CollectedCoins;
            coinTxt.text = score.ToString();
        }
        else
        {
            score = ElfCoins + CollectedCoins;
            coinTxt.text = score.ToString();
        }
    }

    private IEnumerator IndicateImmortal()
    {
        while (immortal)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(.1f);

            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(.1f);
        }
    }

    public override IEnumerator TakeDamage()
    {
        if (!immortal)
        {
            //Debug.Log("Player Taking Damage");
            health -= 10;

            if (!IsDead)
            {
                MyAnimator.SetTrigger("damage");
                immortal = true;
                StartCoroutine(IndicateImmortal());

                yield return new WaitForSeconds(immortalTime);

                immortal = false;
            }
            else
            {
                MyAnimator.SetLayerWeight(1, 0);
                MyAnimator.SetTrigger("death");
            }
        }
    }

    private void HandleMovement(float horizontal)
    {
        if (MyRigidbody.velocity.y < 0)
        {
            MyAnimator.SetBool("land", true);
        }

        if (!Attack && !Slide && !Crouch || (OnGround || airControl))
        {
            MyRigidbody.velocity = new Vector2(horizontal * movementSpeed, MyRigidbody.velocity.y);
        }

        if (Jump && MyRigidbody.velocity.y == 0 && !Crouch)
        {
            MyRigidbody.AddForce(new Vector2(0, jumpForce));
        }

        ////player boundaries on X axis
        //if (transform.position.x <= -4.3f)
        //{
        //    transform.position = new Vector2(-4.3f, transform.position.y);
        //}
        //else if (transform.position.x >= 27.75f)
        //{
        //    transform.position = new Vector2(27.75f, transform.position.y);
        //}

        MyAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MyAnimator.SetBool("crouch", true);
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            MyAnimator.SetBool("crouch", false);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MyAnimator.SetTrigger("jump");

            if (Jump && !OnGround)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    MyRigidbody.AddForce(new Vector2(0, doubleJumpForce));
                    MyAnimator.SetTrigger("jump");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            MyAnimator.SetTrigger("slide");
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            MyAnimator.SetTrigger("attack");
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            MyAnimator.SetTrigger("throw");
            MyAnimator.SetBool("carry", false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            MyAnimator.SetTrigger("invoke");
            InvokeDoberman(0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            ChangeDirection();
        }
    }

    private bool IsGrounded()
    {
        if (MyRigidbody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }

                }
            }
        }

        return false;
    }

    private void HandleLayers()
    {
        if (!OnGround)
        {
            MyAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            MyAnimator.SetLayerWeight(1, 0);
        }
    }

    public void ToGrab()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!grabbed)
            {
                Physics2D.queriesStartInColliders = false;
                hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance);

                if (hit.collider != null && hit.collider.tag == "Pick Up")
                {
                    grabbed = true;

                    hit.transform.SetParent(this.transform, true);

                    MyAnimator.SetTrigger("pickup");
                    MyAnimator.SetBool("carry", true);
                    MyAnimator.ResetTrigger("throw");
                }
            }
            else if (Physics2D.OverlapPoint(holdpoint.position, notgrabbed))
            {
                grabbed = false;

                if (hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
                {
                    hit.rigidbody.isKinematic = false;
                    hit.collider.isTrigger = false;

                    hit.transform.parent = null;

                    hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x, 1) * throwforce;
                    MyAnimator.SetTrigger("throw");
                    MyAnimator.SetBool("carry", false);
                }
            }

            if (grabbed)
            {
                hit.collider.gameObject.transform.position = holdpoint.position;
            }
        }
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            CollectedCoins++;

            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Money Bag")
        {
            elfIsAlive = false;
            score = ElfCoins + CollectedCoins;
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Pick Up")
        {
            hit.rigidbody.isKinematic = true;
            hit.collider.isTrigger = true;
        }
    }

    public void InvokeDoberman(int value)
    {
        if (facingRight)
        {
            Instantiate(dobermanPrefab, dobermanpoint.position, Quaternion.identity);          
        }
        else
        {
            Instantiate(dobermanPrefab, dobermanpoint.position, Quaternion.Euler(0, -180, 0));
        }
    }

/// <summary>
/// Death and Respawn
/// </summary>
    public override void Death()
    {
        MyRigidbody.velocity = Vector2.zero;
        MyAnimator.SetTrigger("idle");
        MyAnimator.ResetTrigger("death");
        health = 30;
        transform.position = startPos;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

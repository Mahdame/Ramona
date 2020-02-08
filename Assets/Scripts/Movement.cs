using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    public float maxSpeed = 10f;
    private bool facingRight = true;
    private float movex = 0f;

    private Rigidbody2D rb2D;
    private Animator animator;

    private bool grounded = false;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    public float jumpForce;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("ground", false);
            rb2D.AddForce(new Vector2(0, jumpForce));
        }
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        animator.SetBool("ground", grounded);

        animator.SetFloat("yspeed", rb2D.velocity.y);

        if (!grounded) return;

        movex = Input.GetAxis("Horizontal");

        animator.SetFloat("speed", Mathf.Abs(movex));

        rb2D.velocity = new Vector2(movex * maxSpeed, rb2D.velocity.y);

        if (movex > 0 && !facingRight)
        {
            Flip();
        }
        else if (movex < 0 && facingRight)
        {
            Flip();
        }
    }



    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}

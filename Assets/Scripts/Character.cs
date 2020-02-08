using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Character : MonoBehaviour {
    
    [SerializeField]
    protected float movementSpeed;

    protected bool facingRight;

    public Animator MyAnimator { get; private set; }

    public bool Attack { get; set; }

    public bool TakingDamage { get; set; }

    [SerializeField]
    protected int health;

    [SerializeField]
    private PolygonCollider2D weaponCollider;

    public PolygonCollider2D WeaponCollider
    {
        get
        {
            return weaponCollider;
        }
    }

    [SerializeField]
    private List<string> damageSources;

    public abstract bool IsDead { get; }

    // Use this for initialization
    public virtual void Start ()
    {
        facingRight = true;
        MyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update ()
    {
        ChangeDirection();
    }

    public abstract IEnumerator TakeDamage();

    public abstract void Death();

    public void ChangeDirection()
    {
        facingRight = !facingRight;

        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    public void MeleeAttack()
    {
        WeaponCollider.enabled = true;
    }

    public virtual void OnTriggerEnter2D (Collider2D other)
    {
        if (damageSources.Contains(other.tag))
        {
            Debug.Log("Enemy taking damage");
            StartCoroutine(TakeDamage());
        }

        if (other.tag == "Edge")
        {
            ChangeDirection();
        }
    }
}

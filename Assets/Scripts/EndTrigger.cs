using UnityEngine;
using System.Collections;

public class EndTrigger : MonoBehaviour
{
    private BoxCollider2D playerCollider;

    void Start()
    {
        playerCollider = GameObject.Find("Player").GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            Physics2D.IgnoreCollision(other, playerCollider, true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            Player.Instance.MyAnimator.SetTrigger("stageclear");
            Player.Instance.MyRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}

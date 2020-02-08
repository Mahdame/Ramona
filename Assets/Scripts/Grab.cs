using UnityEngine;
using System.Collections;

public class Grab : MonoBehaviour
{
    public bool grabbed;
    RaycastHit2D hit;
    public float distance = 2f;
    public Transform holdpoint;
    public float throwforce;
    public LayerMask notgrabbed;

    // Update is called once per frame
    void FixedUpdate ()
    {
        ToGrab();

        //Throw();
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

                    Player.Instance.MyAnimator.SetTrigger("pickup");
                    Player.Instance.MyAnimator.SetBool("carry", true);
                    Player.Instance.MyAnimator.ResetTrigger("throw");
                }
            }
            else if (Physics2D.OverlapPoint(holdpoint.position, notgrabbed))
            {
                grabbed = false;
                //Debug.Log(grabbed);

                if (hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
                {
                    hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x, 1) * throwforce;
                    Player.Instance.MyAnimator.SetTrigger("throw");
                    Player.Instance.MyAnimator.SetBool("carry", false);
                }
            }

            if (grabbed)
            {
                hit.collider.gameObject.transform.position = holdpoint.position;
            }
        }
    }
}

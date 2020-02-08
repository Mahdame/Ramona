using UnityEngine;
using System.Collections;

public class IgnoreCollision : MonoBehaviour
{
    [SerializeField]
    private Collider2D other;

    [SerializeField]
    private Collider2D another;

    private void Awake()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other, true);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), another, true);

        Physics2D.IgnoreLayerCollision(9, 10);
        Physics2D.IgnoreLayerCollision(9, 11);
        Physics2D.IgnoreLayerCollision(9, 12);
    }
}

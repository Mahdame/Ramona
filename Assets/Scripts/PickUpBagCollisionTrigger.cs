using UnityEngine;
using System.Collections;

public class PickUpBagCollisionTrigger : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bag"))
        {
            other.gameObject.SetActive(false);
        }
    }
}

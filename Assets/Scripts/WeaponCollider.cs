using UnityEngine;
using System.Collections;

public class WeaponCollider : MonoBehaviour
{
    [SerializeField]
    private string targetTag;

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == targetTag)
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }
}

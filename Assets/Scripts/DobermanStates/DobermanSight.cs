using UnityEngine;
using System.Collections;

public class DobermanSight : MonoBehaviour {

    [SerializeField]
    private Doberman doberman;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            doberman.Target = other.gameObject;
        }

        Debug.Log(doberman.Target);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            doberman.Target = null;
        }    
    }
}

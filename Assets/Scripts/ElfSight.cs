using UnityEngine;
using System.Collections;

public class ElfSight : MonoBehaviour {

    [SerializeField]
    private Elf elf;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coin")
        {
            elf.Target = other.gameObject;
        }

        if (other.tag == "Player")
        {
            elf.ChangeDirection();
            elf.ChangeElfState(new ElfPatrolState());
        }

        //Debug.Log("Elf's Target: " + elf.Target);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        elf.RemoveTarget();
        //Debug.Log("Elf's Target: " + elf.Target);
    }
}

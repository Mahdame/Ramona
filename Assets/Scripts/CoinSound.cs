using UnityEngine;
using System.Collections;

public class CoinSound : MonoBehaviour
{
    public AudioClip coinSound;
    private AudioSource source;

	void Awake ()
    {
        source = GetComponent<AudioSource>();	
	}

    void OnTriggerEnter2D (Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            source.PlayOneShot(coinSound);
        }
    }
}

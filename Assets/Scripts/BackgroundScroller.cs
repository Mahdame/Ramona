using UnityEngine;
using System.Collections;

public class BackgroundScroller : MonoBehaviour {

    public float speed = 0;
    private float pos = 0;

    private Material mat;

    // Use this for initialization
    void Start () {

        mat = GetComponent<Renderer>().material;

    }
	
	// Update is called once per frame
	void Update () {

        pos += speed;

        if (pos > 1.0f)
        {
            pos -= 1.0f;

            mat.mainTextureOffset = new Vector2(pos, 0);
        }
    }
}

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    // used for following the player
    [SerializeField]
    public bool IsFollowing;

    // bounds of the level
    [SerializeField]
    public BoxCollider2D LevelBounds;

    private GameObject pl;
    //private GameObject cam;

    public float speed = 0;
    private float pos = 0;

    private Vector3        
        _min,
        _max;

    public void Start()
    {
        _min = LevelBounds.bounds.min;
        _max = LevelBounds.bounds.max;
        pl = GameObject.FindGameObjectWithTag("Player");
        //cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public void Update()
    {
        var x = transform.position.x;
        var y = transform.position.y;

        if (IsFollowing)
        {
            var vel = pl.GetComponent<Rigidbody2D>().velocity.x;

            if (vel != 0f)
            {
                //var side = cam.transform.localScale.x;
                var side = pl.transform.localScale.x;
                pos += speed * side;
            }
        }

        var cameraHalfWidth = Camera.main.orthographicSize * ((float)Screen.width / Screen.height);

        // lock the camera to the right or left bound if we are touching it
        x = Mathf.Clamp(x, _min.x + cameraHalfWidth, _max.x - cameraHalfWidth);

        // lock the camera to the top or bottom bound if we are touching it
        y = Mathf.Clamp(y, _min.y + Camera.main.orthographicSize, _max.y - Camera.main.orthographicSize);

        transform.position = new Vector3(x, y, transform.position.z);
    }
}

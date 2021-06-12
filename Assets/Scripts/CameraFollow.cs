using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public BoxCollider2D bounds;

    // Start is called before the first frame update
    void Start()
    {
        SetCameraObjects();
    }

    // Update is called once per frame
    void Update()
    {
        if (ReferencesAreSet())
        {
            FollowPlayer();
        }
    }

    public void SetCameraObjects()
    {
        target = GameObject.FindGameObjectWithTag("GoodPlayer").transform;
        bounds = GameObject.Find("CameraBounds").GetComponent<BoxCollider2D>();
        //Debug.Log("camera bounds = " + bounds + ", min: " + bounds.bounds.min + ", max: " + bounds.bounds.max);

        if(target == null || bounds == null)
        {
            transform.position = new Vector3(0, 0, -10f);
        }
        else
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        transform.position = new Vector3(
                Mathf.Clamp(target.position.x, bounds.bounds.min.x + 8, bounds.bounds.max.x - 8),
                Mathf.Clamp(target.position.y, bounds.bounds.min.y + 5, bounds.bounds.max.y - 5),
                -10f);
    }

    public bool ReferencesAreSet()
    {
        if (target != null && bounds != null)
            return true;
        else
            return false;
    }
}

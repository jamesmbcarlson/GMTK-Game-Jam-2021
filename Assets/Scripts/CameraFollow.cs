using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    private BoxCollider2D bounds;

    // Start is called before the first frame update
    void Start()
    {
        SetCameraObjects();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            Mathf.Clamp(target.position.x, bounds.bounds.min.x + 8, bounds.bounds.max.x - 8),
            Mathf.Clamp(target.position.y, bounds.bounds.min.y + 5, bounds.bounds.max.y - 5),
            -10f);
    }

    private void SetCameraObjects()
    {
        target = GameObject.FindGameObjectWithTag("GoodPlayer").transform;
        bounds = GameObject.Find("CameraBounds").GetComponent<BoxCollider2D>();
        Debug.Log("camera bounds = " + bounds + ", min: " + bounds.bounds.min + ", max: " + bounds.bounds.max);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowXY : MonoBehaviour
{
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<Camera>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(
            target.transform.position.x,
            target.transform.position.y);
    }
}

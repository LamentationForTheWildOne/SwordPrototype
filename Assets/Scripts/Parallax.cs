using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    // Start is called before the first frame update
    private float parallaxSpeed = 0.95f;
    private float startPositionX;
    private Transform cameraTransform;
    void Start()
    {
        cameraTransform = Camera.main.transform;
        startPositionX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float relativeDist = cameraTransform.position.x * parallaxSpeed;
        transform.position = new Vector3(startPositionX + relativeDist, transform.position.y, transform.position.z);
    }
}

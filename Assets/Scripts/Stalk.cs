using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalk : MonoBehaviour
{
    public Transform target;
    public Vector3 offset; 
    [SerializeField] private float smoothness = 5f;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 currentPosition = transform.position;
        Vector3 currentVelocity = Vector2.zero;

        float newX = Mathf.SmoothDamp(currentPosition.x, targetPosition.x, ref currentVelocity.x, smoothness);
        float newY = Mathf.SmoothDamp(currentPosition.y, targetPosition.y, ref currentVelocity.y, smoothness);
        float newZ = Mathf.SmoothDamp(currentPosition.z, targetPosition.z, ref currentVelocity.z, smoothness);

        transform.position = new Vector3(newX, newY, newZ);
    }
}

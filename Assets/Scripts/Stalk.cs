using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalk : MonoBehaviour
{
    public Transform target;
    [SerializeField] private Vector2 offset; 
    [SerializeField] private float smoothness = 5f;

    private Rigidbody2D _rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 targetPosition = target.position;
        Vector2 currentPosition = transform.position;
        Vector2 currentVelocity = _rigidbody.velocity;

        float newX = Mathf.SmoothDamp(currentPosition.x, targetPosition.x, ref currentVelocity.x, smoothness);
        float newY = Mathf.SmoothDamp(currentPosition.y, targetPosition.y, ref currentVelocity.y, smoothness);

        transform.position = new Vector2(newX, newY) + offset;
    }
}

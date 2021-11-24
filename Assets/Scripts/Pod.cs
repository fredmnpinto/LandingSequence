using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pod : MonoBehaviour
{
    public static Pod Instance;
    
    private Transform _boosterL;
    private Transform _boosterR;
    private Transform _auxL;
    private Transform _auxR;

    private Rigidbody2D _rb;
    private EdgeCollider2D _criticalAreaCollider;

    private float _force = 5f;
    public float resistance = 1.5f;
    public LayerMask floor;
    

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _boosterL = gameObject.transform.GetChild(0);
        _boosterR = gameObject.transform.GetChild(1);
        _auxL = gameObject.transform.GetChild(2);
        _auxR = gameObject.transform.GetChild(3);

        _criticalAreaCollider = GetComponent<EdgeCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /* Main thrust */
        if (Input.GetKey(KeyCode.A))
        {
            _rb.AddForceAtPosition(_boosterL.up * _force, _boosterL.position);
        }
        if (Input.GetKey(KeyCode.D))
        {
            _rb.AddForceAtPosition(_boosterR.up * _force, _boosterR.position);
        }
        
        /* Auxiliary thrust */
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _rb.AddForceAtPosition(_auxL.up * _force, _auxL.position);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _rb.AddForceAtPosition(_auxR.up * _force, _auxR.position);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        /* Is either a wall or floor */
        if (floor == (floor | (1 << other.gameObject.layer)))
        {
            ContactPoint2D contact = other.GetContact(0);
            Vector2 contactPosition = contact.point;
            
            /* Is either a stronger collision than the pod can handle
             or it collided on a critical area of the vessel */
            if (_criticalAreaCollider.IsTouching(other.collider) ||
                other.relativeVelocity.y > resistance)
            {
                Debug.Log("Exploded " + other.relativeVelocity.y);
            }
            else
            {
                Debug.Log("Landed Safely " + other.relativeVelocity.y);
            }
        }
    }
}

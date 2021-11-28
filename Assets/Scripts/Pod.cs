using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
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

    public bool invertedButtons;

    [SerializeField] private float totalThrustCapacity;
    [SerializeField] private float thrustDiminishRate;
    [SerializeField] private float thrustRegainRate;
    public float currentThrustCapacity;

    public float thrustForce = 5f;
    public float hullResistance = 1.5f;
    public LayerMask floorMask;
    private Vector2 _initialPosition;


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
        currentThrustCapacity = totalThrustCapacity;

        _initialPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool thrusting = false;

        /* Main thrust */
        if ((!invertedButtons && Input.GetKey(KeyCode.A)) || (invertedButtons && Input.GetKey(KeyCode.D)))
        {
            ThrustAt(1, _boosterL.transform);
            thrusting = true;
        }

        if ((!invertedButtons && Input.GetKey(KeyCode.D)) || (invertedButtons && Input.GetKey(KeyCode.A)))
        {
            ThrustAt(1, _boosterR.transform);
            thrusting = true;
        }

        /* Auxiliary thrust */
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _rb.AddForceAtPosition(_auxL.up * thrustForce, _auxL.position);
            thrusting = true;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            _rb.AddForceAtPosition(_auxR.up * thrustForce, _auxR.position);
            thrusting = true;
        }

        if (!thrusting && !IsGrounded())
        {
            GradualRegain();
        }

        /* Reset Position */
        if (Input.GetKey(KeyCode.R))
        {
            Reset();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        /* Is either a wall or floor */
        if (floorMask == (floorMask | (1 << other.gameObject.layer)))
        {
            ContactPoint2D contact = other.GetContact(0);
            Vector2 contactPosition = contact.point;

            /* Is either a stronger collision than the pod can handle
             or it collided on a critical area of the vessel */
            if (_criticalAreaCollider.IsTouching(other.collider) ||
                other.relativeVelocity.y > hullResistance)
            {
                Debug.Log("Exploded " + other.relativeVelocity.y);
                Explode();
            }
            else
            {
                OnLand();
            }
        }
    }

    private bool IsGrounded()
    {
        Vector2 curPosition = transform.position;
        float castDistance = 0.5f;

        RaycastHit2D hitOnGround = Physics2D.Raycast(curPosition, -transform.up, castDistance, floorMask);
        return hitOnGround.collider != null;
    }

    private void GradualRegain()
    {
        if (currentThrustCapacity < totalThrustCapacity)
            currentThrustCapacity += thrustRegainRate;
        else
            currentThrustCapacity = totalThrustCapacity;
    }
    
    private void OnLand()
    {
        currentThrustCapacity = totalThrustCapacity;
    }

    private void Explode()
    {
        // gameObject.SetActive(false);
        Reset();
    }

    private void Reset()
    {
        transform.position = _initialPosition;
        transform.rotation = Quaternion.identity;

        _rb.angularVelocity = 0;
        _rb.velocity = Vector2.zero;
    }

    private void ThrustAt(float forceScale, Transform thruster)
    {
        if (currentThrustCapacity > 0)
        {
            _rb.AddForceAtPosition(thruster.up * thrustForce * forceScale, thruster.position);
            currentThrustCapacity -= thrustDiminishRate;
        }
        else
        {
            Debug.Log("Thrusters capacity empty");
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;
using UnityEditor;
using UnityEngine;

public class Pod : MonoBehaviour
{
    public static Pod Instance;

    [SerializeField] private CinemachineVirtualCamera playerCamera;

    private Transform _boosterL;
    private Transform _boosterR;
    private Transform _auxL;
    private Transform _auxR;

    private bool _boarded;

    private Rigidbody2D _rb;
    private PolygonCollider2D _criticalAreaCollider;

    public bool invertedButtons;

    public float thrustForce = 5f;
    public float hullResistance = 1.5f;
    public LayerMask floorMask;
    private Vector2 _initialPosition;
    private Vector2 _spriteSize;

    private PodEntrance _podEntrance;

    private bool _pressedBoardingBtn = false;

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

        _boarded = false;

        _criticalAreaCollider = GetComponent<PolygonCollider2D>();
        _rb = GetComponent<Rigidbody2D>();

        _spriteSize = GetComponent<SpriteRenderer>().size;

        _initialPosition = transform.position;

        _podEntrance = GetComponentInChildren<PodEntrance>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            
            _pressedBoardingBtn = true;
        }
    }

    void FixedUpdate()
    {
        if (_boarded)
        {
            /* Main thrusters */
            if (PressedLeftThrusterBtn() && PressedRightThrusterBtn())
            {
                ThrustAt(2f, transform);
            }
            else
            {
                if (PressedRightThrusterBtn())
                {
                    ThrustAt(1f, _boosterR.transform);
                }
                else
                if (PressedLeftThrusterBtn())
                {
                    ThrustAt(1f, _boosterL.transform);
                }
            }
            
            if (_pressedBoardingBtn && IsGrounded())
            {
                Unboard();
            }
            
            /* Reset Command */
            if (Input.GetKey(KeyCode.R))
            {
                Reset();
            }
        }
        else if (_podEntrance.IsAstronautTouchingDoor() && _pressedBoardingBtn)
        {
            Board();
        }

        _pressedBoardingBtn = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        /* Is either a wall or floor */
        if (floorMask == (floorMask | (1 << other.gameObject.layer)))
        {
            ContactPoint2D contact = other.GetContact(0);

            /* Is either a stronger collision than the pod can handle
             or it collided on a critical area of the vessel */
            if (_criticalAreaCollider.IsTouching(other.collider) ||
                other.relativeVelocity.y > hullResistance)
            {
                Explode();
            }
            else
            {
                OnLand();
            }
        }
    }

    private void Board()
    {
        SetCamera(true);
        Astronaut astronaut = Astronaut.Instance;
        
        astronaut.OnBoard();
        
        _boarded = true;
    }

    private void Unboard()
    {
        SetCamera(false);
        
        _boarded = false;
        Astronaut astronaut = Astronaut.Instance;
        Transform astronautTransform = astronaut.transform;
        
        astronautTransform.position = _podEntrance.transform.position;
        astronaut.OnUnboard();
    }

    private void OnLand()
    {

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
        _rb.AddForceAtPosition(thruster.up * thrustForce * forceScale, thruster.position);
    }

    private bool PressedRightThrusterBtn()
    {
        return (!invertedButtons && Input.GetKey(KeyCode.D)) || (invertedButtons && Input.GetKey(KeyCode.A));
    }
    
    private bool PressedLeftThrusterBtn()
    {
        return (!invertedButtons && Input.GetKey(KeyCode.A)) || (invertedButtons && Input.GetKey(KeyCode.D));
    }

    private void SetCamera(bool focusOnPod)
    {
        if (focusOnPod)
        {
            playerCamera.m_Priority = 10;
        }
        else
        {
            playerCamera.m_Priority = 0;
        }
    }
    
    private bool IsGrounded()
    {
        Vector2 curPosition = transform.position;

        float castDistance = 0.3f;

        RaycastHit2D hitOnGroundC = Physics2D.Raycast(curPosition, -transform.up, castDistance, floorMask);
        RaycastHit2D hitOnGroundL = Physics2D.Raycast(new Vector2(_boosterL.position.x, curPosition.y), -transform.up, castDistance, floorMask);
        RaycastHit2D hitOnGroundR = Physics2D.Raycast(new Vector2(_boosterR.position.x, curPosition.y), -transform.up, castDistance, floorMask);

        return hitOnGroundC.collider != null || hitOnGroundR.collider != null || hitOnGroundL.collider != null;
    }
}

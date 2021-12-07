using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Astronaut : MonoBehaviour
{
    public static Astronaut Instance;

    [SerializeField] private CinemachineVirtualCamera playerCamera;
    
    private Rigidbody2D _rb;
    [SerializeField] private float baseVelocity = 5f;
    [SerializeField] private float runningSpeedMultiplier = 1.2f;
    [SerializeField] private float jumpForce = 3f;
    
    public LayerMask floorMask;

    private bool _pressedJump;
    private Vector2 _spriteSize;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteSize = GetComponent<SpriteRenderer>().size;
    }

    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            _pressedJump = true;
        }
    }

    // FixedUpdate is called on a fixed frequency of time
    private void FixedUpdate()
    {
        Vector2 currentVelocity = _rb.velocity;
        float horizontalSpeed = baseVelocity;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            horizontalSpeed *= runningSpeedMultiplier;
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            currentVelocity.x = -horizontalSpeed;

            transform.localScale = new Vector3(-1, 1, 1);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            currentVelocity.x = horizontalSpeed;

            transform.localScale = new Vector3(1, 1, 1);
        }

        else
        {
            currentVelocity.x = 0;
        }

        if (_pressedJump)
        {
            currentVelocity.y = jumpForce;
            _pressedJump = false;
        }

        _rb.velocity = currentVelocity;
    }

    private bool IsGrounded()
    {
        Vector2 curPosition = transform.position;
        
        float castDistance = _spriteSize.y / 2;

        RaycastHit2D hitOnGround = Physics2D.Raycast(curPosition, -transform.up, castDistance, floorMask);

        return hitOnGround.collider != null;
    }

    private void SetCamera(bool focusOnAstronaut)
    {
        if (focusOnAstronaut)
        {
            playerCamera.m_Priority = 10;
        }
        else
        {
            playerCamera.m_Priority = 0;
        }
    }

    public void OnBoard()
    {
        SetCamera(false);
        gameObject.SetActive(false);
    }
    
    public void OnUnboard()
    {
        SetCamera(true);
        gameObject.SetActive(true);
        
        _rb.velocity = Vector2.zero;
    }
}

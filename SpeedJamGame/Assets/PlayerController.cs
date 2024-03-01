using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    
    [SerializeField] private GameObject targetSwingObject;
    [SerializeField] private float radius;
    [SerializeField] private float initialSwingSpeed = 5;
    [SerializeField] private float decelFactor = 0.5f;
    [SerializeField] private LineRenderer swingLine;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _target;
    private float _vert;
    private float _horiz;
    private bool _canSwing;
    private bool _isGrounded;
    private Vector3 _playerToAnchor;
    private bool _isSwingingLeft;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _canSwing = true;
        _isGrounded = true;
        _target = targetSwingObject.transform.position;
    }

    private void Update()
    {
        _isGrounded = _rb.IsTouchingLayers(groundLayer);
        _canSwing = InRange(_target);

        MovePlayer();
        if (Input.GetButtonDown("Jump") && _canSwing)
            StartSwing();
        if (Input.GetButton("Jump") && _canSwing)
            Swing();
        else
            StopSwing();
    }

    private void StartSwing()
    {
        _isSwingingLeft = _rb.position.x > targetSwingObject.transform.position.x;
    }

    private void Jump()
    {
        _isGrounded = false;
        _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
    }
    
    private void MovePlayer()
    {
        _horiz = Input.GetAxis("Horizontal");
        _vert = Input.GetAxis("Vertical"); 


        if(_vert > 0 && _isGrounded)
            Jump();
        if(_horiz != 0)
            transform.localScale = new Vector3(_horiz > 0 ? 1 : -1, 1, 1);
        
        _rb.velocity += new Vector2(_horiz * moveSpeed, 0f);
        _rb.velocity = new Vector2(Mathf.Clamp(_rb.velocity.x, -moveSpeed, moveSpeed), _rb.velocity.y);
    }

    private void Swing()
    {
        Vector2 toAnchor = (Vector2)targetSwingObject.transform.position - _rb.position;
        Vector2 onRadius = toAnchor.normalized * radius;
        Vector2 tangent = new Vector2(-onRadius.y, onRadius.x).normalized;
        
        // Dynamically adjust swing direction based on player's position relative to the swing point
        if (_isSwingingLeft)
        {
            // Player is to the right of the swing point, swing counter-clockwise
            tangent = new Vector2(-onRadius.y, onRadius.x).normalized;
        }
        else
        {
            // Player is to the left of the swing point, swing clockwise
            tangent = new Vector2(onRadius.y, -onRadius.x).normalized;
        }

        // Apply the tangential velocity to swing, optionally maintain any existing vertical motion for realism
        _rb.velocity = tangent * initialSwingSpeed;
        
        swingLine.enabled = true;
        swingLine.SetPosition(0, transform.position);
        swingLine.SetPosition(1, targetSwingObject.transform.position);
    }
    
    private void StopSwing()
    {
        swingLine.enabled = false;
    }


    private bool InRange(Vector2 target)
    {
        return radius >= Vector2.Distance(transform.position, target);
    }
}

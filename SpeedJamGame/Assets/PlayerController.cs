using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float accelTime;
    [SerializeField] private float decelTime;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private PhysicsMaterial2D icePhysics;
    [SerializeField] private PhysicsMaterial2D normalPhysics;
    
    [SerializeField] private List<GameObject> targetSwingObjects;
    [SerializeField] private float radius;
    [SerializeField] private float initialSwingSpeed = 5;
    [SerializeField] private float swingSpeedDecel = 2.5f;
    [SerializeField] private float swingHeightDecel;
    [SerializeField] private LineRenderer swingLine;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    private Vector2 _respawnPoint;
    private float _vert;
    private float _horiz;
    private bool _canSwing;
    private bool _isGrounded;
    private Vector3 _playerToAnchor;
    private float _speed;
    private bool _isSwinging;
    private int _currentlyActiveSwingPoint;

    private void Start()
    {
        Application.targetFrameRate = 60;
        
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _canSwing = true;
        _isGrounded = true;
        _speed = moveSpeed;
    }

    private void Update()
    {
        _isGrounded = _rb.IsTouchingLayers(groundLayer);
        _canSwing = InRange(targetSwingObjects[_currentlyActiveSwingPoint].transform.position);
        _rb.gravityScale = _isSwinging ? 0f : 1f;

        MovePlayer();

        if (Input.GetButton("Jump") && _canSwing)
        {
            if (!_isSwinging)
            {
                if(transform.position.x <= targetSwingObjects[_currentlyActiveSwingPoint].transform.position.x)
                    Swing();
            }
            else
                Swing();
        }

        for (int i = 0; i < targetSwingObjects.Count; i++)
        {
            if (Vector2.Distance(targetSwingObjects[i].transform.position, transform.position) <= 20f)
            {
                _currentlyActiveSwingPoint = i;
                break;
            }
        }

        if(Input.GetButtonUp("Jump"))
            StopSwing();
        if (Input.GetKey(KeyCode.LeftShift))
            Slide();
        if (Input.GetKeyUp(KeyCode.LeftShift))
            StopSlide();
    }

    private void Slide()
    {
        _collider.sharedMaterial = icePhysics;
        _rb.sharedMaterial = icePhysics;
    }
    
    private void StopSlide()
    {
        _collider.sharedMaterial = normalPhysics;
        _rb.sharedMaterial = normalPhysics;
    }

    private void Jump()
    {
        _isGrounded = false;
        _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
    }
    
    private void MovePlayer()
    {
        if(_isSwinging)
            return;
        _horiz = Input.GetAxis("Horizontal");
        _vert = Input.GetAxis("Vertical"); 


        if(_vert > 0 && _isGrounded)
            Jump();
        if(_horiz != 0)
            transform.localScale = new Vector3(_horiz > 0 ? 1 : -1, 1, 1);
        if(_horiz != 0)
            Accelerate(_horiz, moveSpeed);
        if(_horiz == 0 && !_isSwinging && _isGrounded)
            Decelerate();
    }
    
    private void Accelerate(float direction, float maxSpeed)
    {
        var acceleration = (direction * maxSpeed - _rb.velocity.x) / accelTime;
        _rb.velocity += new Vector2(acceleration * Time.deltaTime * 100, 0f);
    }

    private void Decelerate()
    {
        var deceleration = (0 - _rb.velocity.x) / decelTime;
        _rb.velocity += new Vector2(deceleration * Time.deltaTime * 100, 0f);
    }

    private void Swing()
    {
        _isSwinging = true;
        var toAnchor = (Vector2)targetSwingObjects[_currentlyActiveSwingPoint].transform.position - _rb.position;
        var onRadius = toAnchor.normalized * radius;
        var tangent = new Vector2(onRadius.y, -onRadius.x).normalized;
        _rb.velocity = tangent * (initialSwingSpeed + 0.1f);
        if(transform.position.x <= targetSwingObjects[_currentlyActiveSwingPoint].transform.position.x) // Left
            initialSwingSpeed += swingSpeedDecel / 100 * Mathf.Abs(_speed);
        else
            initialSwingSpeed -= swingSpeedDecel / 100 * Mathf.Abs(_speed);
        swingLine.enabled = true;
        swingLine.SetPosition(0, transform.position);
        swingLine.SetPosition(1, targetSwingObjects[_currentlyActiveSwingPoint].transform.position);
    }
    
    private void StopSwing()
    {
        _isSwinging = false;
        swingLine.enabled = false;
    }
    
    private bool InRange(Vector2 target)
    {
        return radius >= Vector2.Distance(transform.position, target) || _isSwinging;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector3(-999f, targetSwingObjects[_currentlyActiveSwingPoint].transform.position.y), new Vector3(999f, swingHeightDecel));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Checkpoint"))
            _respawnPoint = collision.transform.position;
    }

    public void Respawn()
    {
        transform.position = _respawnPoint;
    }
}

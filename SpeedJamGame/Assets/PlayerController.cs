using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public Action<GameObject> onCheckPointHit;

    [SerializeField] private List<float> accelerationValues = new List<float>();
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sizeMomentum;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundDistance = 0.8f;
    [SerializeField] private PhysicsMaterial2D icePhysics;
    [SerializeField] private PhysicsMaterial2D normalPhysics;
    [Header("Swinging")]
    [SerializeField] private List<GameObject> targetSwingObjects;
    [SerializeField] private float radius;
    [SerializeField] private float minSwingSpeed = 5;
    [SerializeField] private float swingPercentage = 5;
    [SerializeField] private float swingHeightDecel;
    [SerializeField] private LineRenderer swingLine;

    [Header("Animations")] 
    [SerializeField] private Vector3 jumpScale;
    [SerializeField] private float jumpDuration;
    [SerializeField] private Vector3 landScale;
    [SerializeField] private float landDuration;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    private Vector2 _respawnPoint;
    private float _vert;
    private float _horiz;
    private bool _canSwing;
    private bool _hasLanded;
    private bool _isGrounded;
    private Vector3 _playerToAnchor;
    private float _speed;
    private bool _isSwinging;
    private bool _isSliding;
    private int _currentlyActiveSwingPoint;
    private float _tierAcceleration;
    private float _initialSwingSpeed = 5;
    private float _swingSpeedDecel = 2.5f;
    private int _currentTier;

    private void Start()
    {
        Application.targetFrameRate = 60;
        
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _canSwing = true;
        _isGrounded = true;
        _speed = moveSpeed;
        _tierAcceleration = accelerationValues[_currentTier];
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(3))
            IncreaseTier();
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundLayer);
        if (!_hasLanded && _isGrounded)
        {
            transform.DOScale(landScale, landDuration).OnComplete(() =>
            { 
                transform.DOScale(Vector3.one, landDuration);
            });
            _hasLanded = true;
        }

        if (!_isGrounded)
            _hasLanded = false;
        
        _canSwing = InRange(targetSwingObjects[_currentlyActiveSwingPoint].transform.position);
        _rb.gravityScale = _isSwinging ? 0f : 1f;

        if(!_isSliding)
            MovePlayer();

        if (Input.GetButtonDown("Jump") && _canSwing)
            StartSwing();
        
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
        _isSliding = true;
    }
    
    private void StopSlide()
    {
        _collider.sharedMaterial = normalPhysics;
        _rb.sharedMaterial = normalPhysics;
        _isSliding = false;
    }

    private void Jump()
    {
        _isGrounded = false;
        _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
        _rb.AddForce(new Vector2(0, jumpForce * _tierAcceleration + 2f), ForceMode2D.Impulse);
        transform.DOScale(jumpScale, jumpDuration);
    }
    
    private void MovePlayer()
    {
        if(_isSwinging)
            return;
        _horiz = Input.GetAxis("Horizontal");
        _vert = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) ? 1 : 0;
        if (_vert > 0 && _isGrounded)
            Jump();
        
        if (_horiz != 0)
        {
            //transform.localScale = new Vector3(_horiz > 0 ? 1 : -1, 1, 1);
            Accelerate(_horiz, moveSpeed);
        }
    }
    
    private void Accelerate(float direction, float maxSpeed)
    {
        var acceleration = (direction * maxSpeed - _rb.velocity.x) / sizeMomentum;
        _rb.velocity += new Vector2(acceleration * Time.deltaTime * 100, 0f);
        _rb.AddForce(new Vector2(direction * _tierAcceleration,0), ForceMode2D.Impulse);
    }

    private void StartSwing()
    {
        _initialSwingSpeed = Mathf.Max(minSwingSpeed, swingPercentage / 100 * _rb.velocity.x);
        _swingSpeedDecel = _initialSwingSpeed / 5f;
    }
    
    private void Swing()
    {
        _isSwinging = true;
        var toAnchor = (Vector2)targetSwingObjects[_currentlyActiveSwingPoint].transform.position - _rb.position;
        var onRadius = toAnchor.normalized * radius;
        var tangent = new Vector2(onRadius.y, -onRadius.x).normalized;
        _rb.velocity = tangent * (_initialSwingSpeed + 0.1f);
        if(transform.position.x <= targetSwingObjects[_currentlyActiveSwingPoint].transform.position.x) // Left
            _initialSwingSpeed += _swingSpeedDecel / 100 * Mathf.Abs(_speed);
        else
            _initialSwingSpeed -= _swingSpeedDecel / 100 * Mathf.Abs(_speed);
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

    public void IncreaseTier()
    {
        _currentTier++;
        _tierAcceleration = accelerationValues[_currentTier];
    }

    public int GetCurrentTier()
    {
        return _currentTier;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector3(-999f, targetSwingObjects[_currentlyActiveSwingPoint].transform.position.y), new Vector3(999f, swingHeightDecel));
        Gizmos.DrawLine(transform.position, transform.position - Vector3.up * groundDistance);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            _respawnPoint = collision.transform.position;
            onCheckPointHit?.Invoke(collision.gameObject);
        }
    }

    public void Respawn(Vector2 pos)
    {
        transform.position = pos;
    }

    public Transform GetActiveSwingPoint()
    {
        return targetSwingObjects[_currentlyActiveSwingPoint].transform;
    }
}

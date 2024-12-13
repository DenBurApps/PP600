using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    private const string JumpAnimationTrigger = "Jump";
    private const string RunAnimationTrigger = "Run";

    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _transperentColor;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _jumpDuration = 0.7f;
    
    private BoxCollider2D _boxCollider;
    private Animator _animator;
    private ControllScheme _controllScheme;
    
    private bool _isGrounded = true;
    private bool _isJumping = false;
    private Vector2 _originalPosition;
    private Coroutine _jumpCoroutine;
    private Transform _transform;
    private IEnumerator _movingCoroutine;
    private SpriteRenderer _spriteRenderer;

    public event Action HitWood;
    
    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _controllScheme = new ControllScheme();

        _controllScheme.Move.Jump.performed += callback => Jump();
        _transform = transform;
        _movingCoroutine = StartMoving();
    }

    private void OnEnable()
    {
        DisableMovement();
    }

    private void OnDisable()
    {
        DisableMovement();
    }

    public void EnableMovement()
    {
        _controllScheme.Enable();

        if (_movingCoroutine != null)
        {
            StartCoroutine(_movingCoroutine);
        }

        _spriteRenderer.color = _defaultColor;
    }

    public void DisableMovement()
    {
        _controllScheme.Disable();
        
        if (_movingCoroutine != null)
        {
            StopCoroutine(_movingCoroutine);
        }
    }

    public void MakeTransperent()
    {
        _spriteRenderer.color = _transperentColor;
    }

    private IEnumerator StartMoving()
    {
        while (enabled)
        {
            if (_isGrounded && !_isJumping)
            {
                _animator.SetTrigger(RunAnimationTrigger);
                EnablePlayer();
            }

            yield return null;
        }
    }

    private void Jump()
    {
        if (_isGrounded && !_isJumping)
        {
            _isJumping = true;
            _isGrounded = false;
            
            _animator.ResetTrigger(RunAnimationTrigger); 
            _animator.SetTrigger(JumpAnimationTrigger);
            
            DisablePlayer();

            if (_jumpCoroutine != null) StopCoroutine(_jumpCoroutine);
            _jumpCoroutine = StartCoroutine(JumpMovement());
        }
    }

    private IEnumerator JumpMovement()
    {
        _originalPosition = _transform.position;
        Vector2 targetPosition = new Vector2(_originalPosition.x, _originalPosition.y + _jumpHeight);

        float elapsedTime = 0f;
        
        while (elapsedTime < _jumpDuration / 2f)
        {
            _transform.position = Vector2.Lerp(_originalPosition, targetPosition, elapsedTime / (_jumpDuration / 2f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _transform.position = targetPosition;
        
        elapsedTime = 0f;

        while (elapsedTime < _jumpDuration / 2f)
        {
            _transform.position = Vector2.Lerp(targetPosition, _originalPosition, elapsedTime / (_jumpDuration / 2f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _transform.position = _originalPosition;

        _isJumping = false;
        _isGrounded = true;

        EnablePlayer();
    }

    private void EnablePlayer()
    {
        _boxCollider.isTrigger = true;
    }

    private void DisablePlayer()
    {
        _boxCollider.isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out WoodLog woodLog))
        {
            HitWood?.Invoke();
        }
    }
}

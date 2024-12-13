using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class WoodLog : MonoBehaviour
{
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _transparentColor;

    [SerializeField] private Vector2 _startScale;
    [SerializeField] private Vector2 _maxScale;
    [SerializeField] private float _scaleIncreaseRate = 0.03f;
    [SerializeField] private float _fallSpeed = 1.5f;

    private Transform _transform;
    private BoxCollider2D _boxCollider;
    private IEnumerator _movingCoroutine;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _transform = transform;
        _transform.localScale = _startScale;
    }

    public void SetTransparent()
    {
        _spriteRenderer.color = _transparentColor;
    }

    public void SetDefaultColor()
    {
        _spriteRenderer.color = _defaultColor;
    }

    private void OnDisable()
    {
        if (_movingCoroutine != null)
        {
            StopCoroutine(_movingCoroutine);
        }
    }

    public void EnableMovement()
    {
        _movingCoroutine = StartMoving();
        StartCoroutine(_movingCoroutine);
        
        SetDefaultColor();
    }

    public void StopMovement()
    {
        if (_movingCoroutine != null)
            StopCoroutine(_movingCoroutine);
        
        SetTransparent();
    }

    private IEnumerator StartMoving()
    {
        while (true)
        {
            _transform.Translate(Vector2.down * _fallSpeed * Time.deltaTime);

            if (_transform.localScale.x < _maxScale.x)
            {
                _transform.localScale += new Vector3(_scaleIncreaseRate, _scaleIncreaseRate) * Time.deltaTime;
            }

            yield return null;
        }
    }
}
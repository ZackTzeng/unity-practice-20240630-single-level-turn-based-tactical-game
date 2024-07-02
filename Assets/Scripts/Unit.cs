using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public static event Action<Unit> UnitKilled;
    [SerializeField] private Sprite _idleSprite;
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private int _range;
    [SerializeField] private int _attack;
    [SerializeField] private int _attackRange;
    [SerializeField] private int _health;
    private SpriteRenderer _spriteRenderer;
    private WorldPosition _worldPosition;
    private float _takeDamageAnimationDuration = 0.4f;
    private Color _takeDamageTargetColor = Color.red;
    private bool _isTakeDamageCoroutineRunning = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Spawn(WorldPosition worldPosition)
    {
        SetIdleSprite();
        Move(worldPosition);
    }

    public void Move(WorldPosition targetWorldPosition)
    {
        if (_worldPosition == targetWorldPosition)
        {
            return;
        }
        _worldPosition = targetWorldPosition;
        transform.position = targetWorldPosition.GetWorldPositionVector3();
    }

    public void SetIdleSprite()
    {
        _spriteRenderer.sprite = _idleSprite;
    }

    public void SetActiveSprite()
    {
        _spriteRenderer.sprite = _activeSprite;
    }

    public WorldPosition GetWorldPosition()
    {
        return _worldPosition;
    }

    public int GetRange()
    {
        return _range;
    }

    public int GetAttackRange()
    {
        return _attackRange;
    }

    public int GetAttack()
    {
        return _attack;
    }

    public void TakeDamage(int damage)
    {
        if (!_isTakeDamageCoroutineRunning)
        {
            _health -= damage;
            Debug.Log($"Unit: TakeDamage(): {name} takes {damage} damage!");
            Debug.Log($"Unit: TakeDamage(): {name} has {_health} health!");
            StartCoroutine(TakeDamageCoroutine());
        }

    }

    private IEnumerator TakeDamageCoroutine()
    {
        _isTakeDamageCoroutineRunning = true;
        yield return StartCoroutine(AnimateTakeDamageCoroutine());

        if (_health <= 0)
        {
            Debug.Log("Unit is dead");
            UnitKilled?.Invoke(this);
            Destroy(gameObject);
        }
        _isTakeDamageCoroutineRunning = false;
    }

    private IEnumerator AnimateTakeDamageCoroutine()
    {
        Color originalColor = _spriteRenderer.color;
        float elapsedTime = 0f;

        // Transition to the target color
        while (elapsedTime < _takeDamageAnimationDuration)
        {
            _spriteRenderer.color = Color.Lerp(originalColor, _takeDamageTargetColor, elapsedTime / _takeDamageAnimationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the target color is set at the end
        _spriteRenderer.color = _takeDamageTargetColor;

        // // Wait for a moment at the target color
        // yield return new WaitForSeconds(1.0f);

        elapsedTime = 0f;

        // Transition back to the original color
        while (elapsedTime < _takeDamageAnimationDuration)
        {
            _spriteRenderer.color = Color.Lerp(_takeDamageTargetColor, originalColor, elapsedTime / _takeDamageAnimationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}

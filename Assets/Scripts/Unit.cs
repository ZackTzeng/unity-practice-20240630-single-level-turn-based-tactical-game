using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public static event Action<Unit> UnitKilled;
    public event Action<float> HealthChanged;

    private UnitSO _unitSO;
    [SerializeField] private HealthBarUI healthBarUI;
    private Sprite _idleSprite;
    private Sprite _activeSprite;
    private int _range;
    private int _attack;
    private int _attackRange;
    private int _health;
    private int _maxHealth;
    private SpriteRenderer _spriteRenderer;
    private WorldPosition _worldPosition;
    private float _takeDamageAnimationDuration = 0.4f;
    private Color _takeDamageTargetColor = Color.red;
    private bool _isTakeDamageCoroutineRunning = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Spawn(UnitSO unitSO, WorldPosition worldPosition)
    {
        _unitSO = unitSO;
        _idleSprite = _unitSO.IdleSprite;
        _activeSprite = _unitSO.ActiveSprite;
        _range = _unitSO.Range;
        _attack = _unitSO.Attack;
        _attackRange = _unitSO.AttackRange;
        _maxHealth = _unitSO.MaxHealth;
        _health = _maxHealth;
        HealthChanged?.Invoke(1f);
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

    public int GetHealth()
    {
        return _health;
    }

    public float GetHealthPercentage()
    {
        return ((float)_health) / ((float)_maxHealth);
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
            Debug.Log($"Unit: {name} has {GetHealthPercentage()} health percentage!");
            HealthChanged?.Invoke(GetHealthPercentage());
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

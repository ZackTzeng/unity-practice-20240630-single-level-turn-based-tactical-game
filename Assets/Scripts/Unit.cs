using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Sprite _idleSprite;
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private int _range;
    private SpriteRenderer _spriteRenderer;
    private WorldPosition _worldPosition;

    private void Awake() {
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
}

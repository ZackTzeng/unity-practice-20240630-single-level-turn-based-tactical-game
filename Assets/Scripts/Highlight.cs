using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ShowHighlight(Sprite highlightSprite)
    {
        _spriteRenderer.sprite = highlightSprite;
    }

    public void HideHighlight()
    {
        _spriteRenderer.sprite = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HighlightService : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private Grid _grid;
    [SerializeField] private GameObject _highlightPrefab;
    [SerializeField] private Sprite movableHighlightSprite;
    [SerializeField] private Sprite attackableHighlightSprite;

    private GridSystem _gridSystem;
    private List<GameObject> _highlightGameObjects = new();

    private void Awake()
    {
        _gridSystem = GetComponent<GridSystem>();
    }

    public void HighlightMovableTiles(List<GridPosition> movableGridPositions)
    {
        foreach (GridPosition gridPosition in movableGridPositions)
        {
            WorldPosition worldPosition = _gridSystem.GetWorldPositionFromGridPosition(gridPosition);
            GameObject highlightGameObject = Instantiate(_highlightPrefab, worldPosition.GetWorldPositionVector3(), Quaternion.identity);
            Highlight highlight = highlightGameObject.GetComponent<Highlight>();
            highlight.ShowHighlight(movableHighlightSprite);
            _highlightGameObjects.Add(highlightGameObject);
        }
    }

    public void UnhighlightHighlightedTiles()
    {
        foreach (GameObject highlightGameObject in _highlightGameObjects)
        {
            Destroy(highlightGameObject);
        }
        _highlightGameObjects.Clear();
    }
}

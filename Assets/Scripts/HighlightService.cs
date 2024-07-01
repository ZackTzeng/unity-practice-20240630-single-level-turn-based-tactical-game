using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HighlightService : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private Grid _grid;
    [SerializeField] private GameObject _highlightPrefab;
    [SerializeField] private Sprite _movableHighlightSprite;
    [SerializeField] private Sprite _attackableHighlightSprite;
    [SerializeField] private UnitActionUI _unitActionUI;
    private TileValidationService _tileValidationService;

    private GridSystem _gridSystem;
    private List<GameObject> _highlightGameObjects = new();

    private void Awake()
    {
        _gridSystem = GetComponent<GridSystem>();
        _tileValidationService = GetComponent<TileValidationService>();
    }

    public void HighlightMovableTiles(List<GridPosition> movableGridPositions)
    {
        foreach (GridPosition gridPosition in movableGridPositions)
        {
            WorldPosition worldPosition = _gridSystem.GetWorldPositionFromGridPosition(gridPosition);
            GameObject highlightGameObject = Instantiate(_highlightPrefab, worldPosition.GetWorldPositionVector3(), Quaternion.identity);
            Highlight highlight = highlightGameObject.GetComponent<Highlight>();
            highlight.ShowHighlight(_movableHighlightSprite);
            _highlightGameObjects.Add(highlightGameObject);
        }
    }

    public void HighlightAttackableTiles(List<GridPosition> attackableGridPositions)
    {
        foreach (GridPosition gridPosition in attackableGridPositions)
        {
            WorldPosition worldPosition = _gridSystem.GetWorldPositionFromGridPosition(gridPosition);
            GameObject highlightGameObject = Instantiate(_highlightPrefab, worldPosition.GetWorldPositionVector3(), Quaternion.identity);
            Highlight highlight = highlightGameObject.GetComponent<Highlight>();
            highlight.ShowHighlight(_attackableHighlightSprite);
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

    private void UnitActionUI_UnitMoveButtonClicked(Unit unit)
    {
        UnhighlightHighlightedTiles();
        List<GridPosition> movableGridPositions = _tileValidationService.GetMovableTileGridPositions(unit);
        HighlightMovableTiles(movableGridPositions);
    }

    private void UnitActionUI_UnitAttackButtonClicked(Unit unit)
    {
        UnhighlightHighlightedTiles();
        List<GridPosition> attackableGridPositions = _tileValidationService.GetAttackableTileGridPosition(unit);
        HighlightAttackableTiles(attackableGridPositions);
    }

    private void OnEnable() {
        _unitActionUI.UnitMoveButtonClicked += UnitActionUI_UnitMoveButtonClicked;
        _unitActionUI.UnitAttackButtonClicked += UnitActionUI_UnitAttackButtonClicked;
    }

    private void OnDisable() {
        _unitActionUI.UnitMoveButtonClicked -= UnitActionUI_UnitMoveButtonClicked;
        _unitActionUI.UnitAttackButtonClicked -= UnitActionUI_UnitAttackButtonClicked;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridSystem : MonoBehaviour
{
    public event Action<Unit> UnitBecameActive;
    public event Action ActiveUnitBecameIdle;
    [SerializeField] private Grid _grid;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private List<GridPosition> _allyUnitStartingGridPositions = new();
    [SerializeField] private List<GridPosition> _enemyUnitStartingGridPositions = new();
    [SerializeField] private MouseController _mouseController;
    [SerializeField] private GameObject _unitPrefab;
    [SerializeField] private List<UnitSO> _allyUnitSOs = new();
    [SerializeField] private List<UnitSO> _enemyUnitSOs = new();
    [SerializeField] private GameObject _allyUnitPrefab;
    [SerializeField] private GameObject _enemyUnitPrefab;
    [SerializeField] private UnitActionUI _unitActionUI;
    private Dictionary<GridPosition, Unit> _dictGridPositionToUnit = new();
    private Unit _activeUnit = null;
    private bool _isAttackSelected = false;
    private TileValidationService _tileValidationService;
    private HighlightService _highlightService;
    private CombatService _combatService;

    private void OnValidate() {
        if (_allyUnitStartingGridPositions.Count != _allyUnitSOs.Count)
        {
            Debug.LogError("The number of ally unit starting grid positions must be equal to the number of ally unit SOs.");
        }

        if (_enemyUnitStartingGridPositions.Count != _enemyUnitSOs.Count)
        {
            Debug.LogError("The number of enemy unit starting grid positions must be equal to the number of enemy unit SOs.");
        }
    }
    
    private void Start() {
        _tileValidationService = GetComponent<TileValidationService>();
        _highlightService = GetComponent<HighlightService>();
        _combatService = new();
        BoundsInt bounds = _tilemap.cellBounds;
        foreach (Vector3Int position in bounds.allPositionsWithin)
        {
            _dictGridPositionToUnit[new GridPosition(position)] = null;
        }
        for (int i = 0; i < _allyUnitStartingGridPositions.Count; i++)
        {
            SpawnUnit(_allyUnitSOs[i], _allyUnitStartingGridPositions[i]);
        }
        for (int i = 0; i < _enemyUnitStartingGridPositions.Count; i++)
        {
            SpawnUnit(_enemyUnitSOs[i], _enemyUnitStartingGridPositions[i]);
        }
    }

    private void SpawnUnit(UnitSO unitSO, GridPosition spawnGridPosition)
    {
        GameObject unitGameObject = Instantiate(_unitPrefab, Vector3.zero, Quaternion.identity);
        WorldPosition spawnWorldPosition = GetWorldPositionFromGridPosition(spawnGridPosition);
        Unit unit = unitGameObject.GetComponent<Unit>();
        unit.Spawn(unitSO, spawnWorldPosition);
        _dictGridPositionToUnit[spawnGridPosition] = unit;
    }

    public WorldPosition GetWorldPositionFromGridPosition(GridPosition gridPosition)
    {
        Vector3Int cellPosition = gridPosition.GetGridPositionVector3Int();
        WorldPosition worldPosition = new(_tilemap.GetCellCenterWorld(cellPosition));
        return worldPosition;
    }

    public GridPosition GetGridPositionFromWorldPosition(WorldPosition worldPosition)
    {
        Vector3Int cellPosition = _tilemap.WorldToCell(worldPosition.GetWorldPositionVector3());
        GridPosition gridPosition = new(cellPosition);
        return gridPosition;
    }

    private void MouseController_LeftMouseClicked(WorldPosition mouseWorldPosition)
    {
        // Handle four cases when the mouse is clicked on a grid position that contains a unit:
          // 1. No active unit && the mouse clicked on a unit: set the clicked unit as the active unit.
          // 2. An active unit && the mouse clicked on the active unit: set the active unit as idle.
          // 3. An active unit && no selected skill && the mouse clicked on another unit: set the clicked unit as the active unit.
          // 4. An active unit && a selected skill && the mouse clicked on another unit: resolve combat
        // Handle two cases when the mouse is clicked on a grid position that does not contain a unit:
          // 1. An active unit && no selected skill:
          //   a. mouse clicked on one of the movable tiles: move the active unit to the clicked grid position.
          //   b. mouse clicked on a non-movable tile: set the active unit as idle.
          // 2. An active unit && a selected skill: set the active unit as idle.
        
        GridPosition mouseGridPosition = GetGridPositionFromWorldPosition(mouseWorldPosition);

        // Handle four cases when the mouse is clicked on a grid position that contains a unit
        if (_dictGridPositionToUnit.ContainsKey(mouseGridPosition) && _dictGridPositionToUnit[mouseGridPosition] != null)
        {
            Unit selectedUnit = _dictGridPositionToUnit[mouseGridPosition];
            // No active unit && the mouse clicked on a unit:
            // set the clicked unit as the active unit
            if (_activeUnit == null)
            {
                SetActiveUnit(selectedUnit);
            }

            // An active unit && the mouse clicked on a unit
            else
            {
                // An active unit && the mouse clicked on the active unit
                if (_activeUnit == _dictGridPositionToUnit[mouseGridPosition])
                {
                    SetActiveUnitIdle();
                }

                // An active unit && no selected skill && the mouse clicked on another unit
                else if (!_isAttackSelected && _activeUnit != _dictGridPositionToUnit[mouseGridPosition])
                {
                    SetActiveUnitIdle();
                    SetActiveUnit(selectedUnit);
                }

                // An active unit && a selected skill && the mouse clicked on another unit
                else if (_isAttackSelected && _activeUnit != _dictGridPositionToUnit[mouseGridPosition])
                {
                    _combatService.ResolveAttack(_activeUnit, selectedUnit);
                }
                
            }
        }

        // Handle two cases when the mouse is clicked on a grid position that does not contain a unit
        else
        {
            // An active unit && no selected skill
            if (_activeUnit != null && !_isAttackSelected)
            {
                // mouse clicked on one of the movable tiles
                if (_tileValidationService.GetMovableTileGridPositions(_activeUnit).Contains(mouseGridPosition))
                {
                    MoveActiveUnit(mouseGridPosition);
                }

                // b. mouse clicked on a non-movable tile
                else
                {
                    SetActiveUnitIdle();
                }
            }

            // An active unit && a selected skill
            else if (_activeUnit != null && _isAttackSelected)
            {
                SetActiveUnitIdle();
            }
        }
    }

    public bool IsGridPositionOccupiedByUnit(GridPosition gridPosition)
    {
        return _dictGridPositionToUnit[gridPosition] != null;
    }

    private void SetActiveUnitIdle()
    {
        Debug.Log($"Set the active unit as idle");
        _highlightService.UnhighlightHighlightedTiles();
        _activeUnit.SetIdleSprite();
        _activeUnit = null;
        _isAttackSelected = false;
        ActiveUnitBecameIdle?.Invoke();
    }

    private void SetActiveUnit(Unit unit)
    {
        Debug.Log($"Set Unit {unit} as the active unit");
        _activeUnit = unit;
        _isAttackSelected = false;
        unit.SetActiveSprite();
        List<GridPosition> movableTileGridPositions = _tileValidationService.GetMovableTileGridPositions(unit);
        foreach (GridPosition gridPosition in movableTileGridPositions)
        {
            Debug.Log($"SetActiveUnit GP{gridPosition}");
        }
        _highlightService.HighlightMovableTiles(movableTileGridPositions);
        UnitBecameActive?.Invoke(_activeUnit);
    }

    private void MoveActiveUnit(GridPosition targetGridPosition)
    {
        Debug.Log($"Move the active unit to the clicked grid position GP{targetGridPosition}.");
        _highlightService.UnhighlightHighlightedTiles();
        _dictGridPositionToUnit[GetGridPositionFromWorldPosition(_activeUnit.GetWorldPosition())] = null;
        _activeUnit.Move(GetWorldPositionFromGridPosition(targetGridPosition));
        _dictGridPositionToUnit[targetGridPosition] = _activeUnit;
        _highlightService.HighlightMovableTiles(_tileValidationService.GetMovableTileGridPositions(_activeUnit));
    }

    private void UnitActionUI_UnitMoveButtonClicked(Unit unit)
    {
        Debug.Log($"Unit {unit} move button clicked");
        _isAttackSelected = false;
    }

    private void UnitActionUI_UnitAttackButtonClicked(Unit unit)
    {
        Debug.Log($"Unit {unit} attack button clicked");
        _isAttackSelected = true;
    }

    private void Unit_UnitKilled(Unit unit)
    {
        Debug.Log($"Unit {unit} is killed");
        _dictGridPositionToUnit[GetGridPositionFromWorldPosition(unit.GetWorldPosition())] = null;
    }

    private void OnEnable() {
        _mouseController.LeftMouseClicked += MouseController_LeftMouseClicked;
        _unitActionUI.UnitMoveButtonClicked += UnitActionUI_UnitMoveButtonClicked;
        _unitActionUI.UnitAttackButtonClicked += UnitActionUI_UnitAttackButtonClicked;
        Unit.UnitKilled += Unit_UnitKilled;
    }

    private void OnDisable() {
        _mouseController.LeftMouseClicked -= MouseController_LeftMouseClicked;
        _unitActionUI.UnitMoveButtonClicked -= UnitActionUI_UnitMoveButtonClicked;
        _unitActionUI.UnitAttackButtonClicked -= UnitActionUI_UnitAttackButtonClicked;
        Unit.UnitKilled -= Unit_UnitKilled;
    }

    public void ShowDictGridPositionToUnit()
    {
        foreach (KeyValuePair<GridPosition, Unit> kvp in _dictGridPositionToUnit)
        {
            if (kvp.Value != null)
            {
                Debug.Log($"GridPosition: {kvp.Key}, Unit: {kvp.Value}");
            }
        }
    }
}

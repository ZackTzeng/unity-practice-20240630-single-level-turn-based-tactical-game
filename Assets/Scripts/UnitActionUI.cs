using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionUI : MonoBehaviour
{
    public event Action<Unit> UnitMoveButtonClicked;
    public event Action<Unit> UnitAttackButtonClicked;
    [SerializeField] private GameObject _unitActionUIContainer;
    [SerializeField] private GameObject _unitMoveActionUIButtonPrefab;
    [SerializeField] private GameObject _unitAttackActionUIButtonPrefab;
    [SerializeField] private GridSystem _gridSystem;
    private Unit _activeUnit;
    private List<GameObject> _unitActionUIButtons = new();

    private void ShowUnitActionUIButtons()
    {
        GameObject moveActionUIButtonGameObject = Instantiate(_unitMoveActionUIButtonPrefab);
        moveActionUIButtonGameObject.transform.SetParent(_unitActionUIContainer.transform, false);
        _unitActionUIButtons.Add(moveActionUIButtonGameObject);
        Button moveActionUIButton = moveActionUIButtonGameObject.GetComponent<Button>();
        moveActionUIButton.onClick.AddListener(() =>
        {
            UnitMoveButtonClicked?.Invoke(_activeUnit);
        });

        GameObject attackActionUIButtonGameObject = Instantiate(_unitAttackActionUIButtonPrefab);
        attackActionUIButtonGameObject.transform.SetParent(_unitActionUIContainer.transform, false);
        _unitActionUIButtons.Add(attackActionUIButtonGameObject);
        Button attackActionUIButton = attackActionUIButtonGameObject.GetComponent<Button>();
        attackActionUIButton.onClick.AddListener(() =>
        {
            UnitAttackButtonClicked?.Invoke(_activeUnit);
        });
    }

    private void HideUnitActionUIButtons()
    {
        foreach (GameObject unitActionUIButton in _unitActionUIButtons)
        {
            Destroy(unitActionUIButton);
        }
        _unitActionUIButtons.Clear();
    }

    private void GridSystem_ActiveUnitBecameIdle() {
        Debug.Log("Unit became idle");
        HideUnitActionUIButtons();
        _activeUnit = null;
    }

    private void GridSystem_OnUnitBecameActive(Unit unit) {
        Debug.Log("Unit became active");
        _activeUnit = unit;
        ShowUnitActionUIButtons();
    }

    private void OnEnable() {
        _gridSystem.ActiveUnitBecameIdle += GridSystem_ActiveUnitBecameIdle;
        _gridSystem.UnitBecameActive += GridSystem_OnUnitBecameActive;
    }

    private void OnDisable() {
        _gridSystem.ActiveUnitBecameIdle -= GridSystem_ActiveUnitBecameIdle;
        _gridSystem.UnitBecameActive -= GridSystem_OnUnitBecameActive;
    }
}

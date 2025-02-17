using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public enum TargetSelectionResult
{
    Running = 0,
    Success = 1,
    Failed = 2
}

public class TargetSelection : MonoBehaviour
{
    [SerializeField] private Button _cancelButton;

    public TargetSelectionResult TargetSelectionResult { get; private set; } = TargetSelectionResult.Running;

    private List<Unit> _availableTargets;
    private Transform _originalParent;

    public event Action<Unit> OnTargetSelected;
    public event Action OnTargetSelectionCancelled;

    public Unit SelectedTarget { get; private set; }

    private void OnEnable()
    {
        _cancelButton.onClick.AddListener(CancelTargetSelection);
    }

    private void OnDisable()
    {
        _cancelButton.onClick.RemoveListener(CancelTargetSelection);
    }

    public void StartTargetSelection(List<Unit> availableTargets, Transform originalParent)
    {
        TargetSelectionResult = TargetSelectionResult.Running;

        this.gameObject.SetActive(true);

        if (availableTargets.Count < 1) //No available targets
        {
            CancelTargetSelection();
            return;
        }

        _originalParent = originalParent;
        _availableTargets = availableTargets;

        foreach (var target in _availableTargets)
        {
            target.transform.parent.SetParent(this.transform);

            target.SetSelectable(true);
            target.OnUnitSelected.AddListener(SelectTarget);
        }
    }

    private void SelectTarget(Unit selectedTarget)
    {
        TargetSelectionResult = TargetSelectionResult.Success;
        SelectedTarget = selectedTarget;
        OnTargetSelected?.Invoke(SelectedTarget);

        foreach (var target in _availableTargets)
        {
            target.transform.parent.SetParent(_originalParent);

            target.SetSelectable(false);
            target.OnUnitSelected.RemoveListener(SelectTarget);
        }

        this.gameObject.SetActive(false);
    }

    public void CancelTargetSelection()
    {
        TargetSelectionResult = TargetSelectionResult.Failed;
        OnTargetSelectionCancelled?.Invoke();

        foreach (var target in _availableTargets)
        {
            target.transform.parent.SetParent(_originalParent);

            target.SetSelectable(false);
            target.OnUnitSelected.RemoveListener(SelectTarget);
        }

        this.gameObject.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    public Unit SelectedTarget = null;

    private void OnEnable()
    {
        _cancelButton.onClick.AddListener(CancelTargetSelection);
    }

    private void OnDisable()
    {
        _cancelButton.onClick.RemoveListener(CancelTargetSelection);
    }

    private void Update()
    {
        
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

        foreach (var availableTarget in _availableTargets)
        {
            availableTarget.transform.parent.SetParent(_originalParent);
        }

        this.gameObject.SetActive(false);
    }

    public void CancelTargetSelection()
    {
        TargetSelectionResult = TargetSelectionResult.Failed;
        OnTargetSelectionCancelled?.Invoke();

        foreach (var availableTarget in _availableTargets)
        {
            availableTarget.transform.SetParent(_originalParent);
        }

        this.gameObject.SetActive(false);
    }
}

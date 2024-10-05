using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseUILogic : MonoBehaviour
{
    private const string MainMenuButtonName = "MainMenuButton";
    private const string RoundLabelName = "roundLabel";
    private const string BloonsLabelName = "bloonsLabel";

    public event EventHandler MainMenuButtonPressed;

    private UIDocument _overlayDocument;
    private SpawnManager _spawnManager;

    private void Awake()
    {
        _spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
    }

    private void OnEnable()
    {
        _overlayDocument = GetComponent<UIDocument>();
        if (_overlayDocument == null)
        {
            Debug.LogError("No UIDocument found on OverlayManager object! Disabling OverlayManager script.");
            enabled = false;
            return;
        }
        _overlayDocument.rootVisualElement.Q<Button>(MainMenuButtonName).clicked += () =>
        {
            MainMenuButtonPressed?.Invoke(this, EventArgs.Empty);
        };

        _overlayDocument.rootVisualElement.Q<Label>(RoundLabelName).text = _spawnManager.Round.ToString();
        _overlayDocument.rootVisualElement.Q<Label>(BloonsLabelName).text = _spawnManager.TotalBloons.ToString();
    }

}

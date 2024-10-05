using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUILogic : MonoBehaviour
{
    private const string LevelSelectorName = "LevelSelector";
    private const string StartButtonName = "StartButton";
    private const string QuitButtonName = "QuitButton";


    private UIDocument _mainMenuUIDocument;

    private void OnEnable()
    {
        _mainMenuUIDocument = GetComponent<UIDocument>();
        if (_mainMenuUIDocument == null)
        {
            Debug.LogError("Main Menu UI Document is not found");
            enabled = false;
            return;
        }

        _mainMenuUIDocument.rootVisualElement.Q<Button>(StartButtonName).clicked += () =>
        {
            Debug.Log("Start Button Pressed");
            int sceneNr = _mainMenuUIDocument.rootVisualElement.Q<DropdownField>(LevelSelectorName).index + 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNr);
        };

        _mainMenuUIDocument.rootVisualElement.Q<Button>(QuitButtonName).clicked += () =>
        {
#if !UNITY_EDITOR
            Application.Quit();
#elif UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        };
    }
}
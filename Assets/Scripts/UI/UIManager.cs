using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OverlayUIManager : MonoBehaviour
{
    [SerializeField] private ShopUILogic shopUILogic;
    [SerializeField] private PauseUILogic pauseUILogic;
    private float _timeScale;
    private Player _player;
    private bool _isOpen = false;
    private ShopUILogic _shopPanel;
    private PauseUILogic _pausePanel;

    // Start is called before the first frame update

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _player.OpenShop += OnOpenShop;
        _player.Pause += OnPause;
        //_overlayDocument = GetComponent<UIDocument>();
        _shopPanel = Instantiate(shopUILogic, transform);
        _shopPanel.gameObject.SetActive(false);
        _shopPanel.ButtonPressed += OnShopButtonPressed;

        _pausePanel = Instantiate(pauseUILogic, transform);
        _pausePanel.gameObject.SetActive(false);
        _pausePanel.MainMenuButtonPressed += OnMainMenuButtonPressed;
    }

    private void OnMainMenuButtonPressed(object sender, EventArgs e)
    {
        Time.timeScale = _timeScale;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void OnShopButtonPressed(object sender, EventArgs e)
    {
        UnpauseScreen();
        _shopPanel.gameObject.SetActive(false);
    }

    private void OnOpenShop(object sender, Player.ShopEventArgs e)
    {
        if (_shopPanel.isActiveAndEnabled)
        {
            UnpauseScreen();
            _shopPanel.gameObject.SetActive(false);
        }
        else {
            if (_pausePanel.isActiveAndEnabled)
            {
                _pausePanel.gameObject.SetActive(false);
                //_player.toggleShop();
            }
            else
            {
                PauseScreen();
            }
            _shopPanel.gameObject.SetActive(true);
        }
    }
    private void OnPause(object sender, Player.PauseEventArgs e)
    {
        if (_pausePanel.isActiveAndEnabled)
        {
            UnpauseScreen();
            _pausePanel.gameObject.SetActive(false);
        }
        else
        {
            if (_shopPanel.isActiveAndEnabled)
            {
                _shopPanel.gameObject.SetActive(false);
                _player.toggleShop();
            }
            else
            {
                PauseScreen();
            }
            _pausePanel.gameObject.SetActive(true);

        }
    }

    private void PauseScreen() {
        _timeScale = Time.timeScale;
        Time.timeScale = 0;
        _player.GetComponentInChildren<FirstPersonLook>().UnlockCursor();
    }
    private void UnpauseScreen()
    {
        Time.timeScale = _timeScale;
        _player.GetComponentInChildren<FirstPersonLook>().LockCursor();
    }
}




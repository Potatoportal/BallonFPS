using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopUILogic : MonoBehaviour
{

    [SerializeField] private DartMonkey DartMonkey;
    [SerializeField] private DartMonkey BombMonkey;

    private const string DartMonkeyButtonName = "DartMonkeyButton";
    private const string BombButtonName = "BombButton";
    private const string BombMonkeyButtonName = "BombMonkeyButton";

    public event EventHandler ButtonPressed;

    private UIDocument _overlayDocument;

    private Player _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
        _overlayDocument.rootVisualElement.Q<Button>(DartMonkeyButtonName).clicked += () =>
        {
            if (_player.Money >= 100)
            {
                _player.AddMoney(-100);
                Transform t = _player.gameObject.transform;
                Transform monkey = Instantiate(DartMonkey, t.position, t.rotation).transform;
                monkey.SetParent(t);
                monkey.position += monkey.forward * 3;
                _player.toggleMonkey();
                _player.DartMonkey = DartMonkey;
                ButtonPressed?.Invoke(this, EventArgs.Empty);
            }
        };
        _overlayDocument.rootVisualElement.Q<Button>(BombButtonName).clicked += () =>
        {
            if (_player.Money >= 200)
            {
                _player.AddMoney(-200);
                _player.ActivateBombs();
                ButtonPressed?.Invoke(this, EventArgs.Empty);
            }
        };
        _overlayDocument.rootVisualElement.Q<Button>(BombMonkeyButtonName).clicked += () =>
        {
            if (_player.Money >= 150)
            {
                _player.AddMoney(-150);
                Transform t = _player.gameObject.transform;
                Transform monkey = Instantiate(BombMonkey, t.position, t.rotation).transform;
                monkey.SetParent(t);
                monkey.position += monkey.forward * 3;
                _player.toggleMonkey();
                _player.DartMonkey = BombMonkey;
                ButtonPressed?.Invoke(this, EventArgs.Empty);
            }
        };
    }

}

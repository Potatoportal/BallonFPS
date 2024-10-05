using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OverlayUILogic : MonoBehaviour
{

    private const string HealthLabelName = "HealthLabel";
    private const string MoneyLabelName = "MoneyLabel";
    private const string NotificationLabelName = "NotificationLabel";
    private Player _player;
    private SpawnManager _spawnManager;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _player.HealthUpdate += OnHealthUpdate;
        _player.MoneyUpdate += OnMoneyUpdate;

        _spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
        _spawnManager.Notification += OnNotification;
    }

    private void OnNotification(object sender, SpawnManager.NotificationEventArgs e) {
        _overlayDocument.rootVisualElement.Q<Label>(NotificationLabelName).text = e.Notification;
        Invoke(nameof(RemoveNotification), 2f);
    }

    private void RemoveNotification() {
        _overlayDocument.rootVisualElement.Q<Label>(NotificationLabelName).text = "";
    }

    private void OnHealthUpdate(object sender, Player.HealthEventArgs e)
    {
        _overlayDocument.rootVisualElement.Q<Label>(HealthLabelName).text = e.Health.ToString();
    }

    private void OnMoneyUpdate(object sender, Player.MoneyEventArgs e)
    {
        _overlayDocument.rootVisualElement.Q<Label>(MoneyLabelName).text = e.Money.ToString();
    }

    private UIDocument _overlayDocument;

    private void OnEnable()
    {
        _overlayDocument = GetComponent<UIDocument>();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private Projectile bomb;
    [SerializeField] private int health = 100;
    [SerializeField] private int money = 100;
    private bool _hasMonkey = false;
    private bool _inShop = false;
    public DartMonkey DartMonkey { get; set; }
    private Transform _cameraTransform;
    // Start is called before the first frame update


    private void Awake()
    {
        _cameraTransform = GetComponentInChildren<Camera>().gameObject.transform;
    }
    private void OnShoot()
    {
        if (!_inShop)
        {
            Instantiate(projectile, _cameraTransform.position, _cameraTransform.rotation);
        }
    }

    private void OnPause() {
        Pause?.Invoke(this, new PauseEventArgs());
    }

    private void OnShop() {
        OpenShop?.Invoke(this, new ShopEventArgs());
        toggleShop();
    }

    private void OnPlace()
    {
        if (_hasMonkey) {
            toggleMonkey();
            toggleShop();
            Transform monkey = transform.GetChild(transform.childCount - 1);
            Instantiate(DartMonkey, monkey.position, monkey.rotation);
            Destroy(monkey.gameObject);
        }
    }

    public void takeDamage(int damage) {
        health -= damage;
        HealthUpdate?.Invoke(this, new HealthEventArgs { Health = health });
    }

    public void AddMoney(int amount) {
        money += amount;
        MoneyUpdate?.Invoke(this, new MoneyEventArgs { Money = money });
    }
    public void ActivateBombs() {
        toggleShop();
        projectile = bomb;
    }

    public void toggleMonkey() {
        _hasMonkey = _hasMonkey ? false : true;
    }
    public void toggleShop()
    {
        _inShop = _inShop ? false : true;
    }

    public int Money => money;
    public int Health => health;

    public event EventHandler<HealthEventArgs> HealthUpdate;
    public event EventHandler<MoneyEventArgs> MoneyUpdate;
    public event EventHandler<ShopEventArgs> OpenShop;
    public event EventHandler<PauseEventArgs> Pause;

    public class HealthEventArgs : EventArgs
    {
        public int Health{ get; set; }
    }
    public class MoneyEventArgs : EventArgs
    {
        public int Money { get; set; }
    }

    public class ShopEventArgs : EventArgs
    {
    }
    public class PauseEventArgs : EventArgs
    {
    }



}

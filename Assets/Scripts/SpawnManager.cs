using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private Spawn[] _spawns;
    private bool _roundStart = true;
    private bool _hasSpawned = false;
    private bool _roundEnd = false;
    private int _round = 0;
    private int _totalBloons = 0;
    private int _bloonsSpawned = 0;
    private int _bloonCluster = 0;
    private Player _player;
    (int, (int, int)[])[] _rounds = new (int, (int, int)[])[6];

    public int Round => _round;
    public int TotalBloons => _totalBloons;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _spawns = GetComponentsInChildren<Spawn>();
        (int,int)[] round1 = { (5, 1),(5,1), (5,1) };
        (int, int)[] round2 = { (3,1), (10,2), (5,1), (5,2)};
        (int, int)[] round3 = { (5, 2), (5, 3), (5, 2), (5, 3), (5,2) };
        (int, int)[] round4 = { (20, 3), (10, 2), (10, 4), (5, 3) };
        (int, int)[] round5 = { (1, 5), (5, 4), (2, 5), (10, 5), (5,2), (5,5), (5,5) };
        (int, int)[] round6 = { (1,6) };

        //_rounds = { (15, round1), (totalBloons: 23, bloons: round2), (totalBloons: 25, bloons: round3), (totalBloons: 45, bloons: round4), (totalBloons: 33, bloons: round5) };
        _rounds[0] = ((totalBloons: 15, bloons: round1));
        _rounds[1] = ((totalBloons: 23, bloons: round2));
        _rounds[2] = ((totalBloons: 25, bloons: round3));
        _rounds[3] = ((totalBloons: 45, bloons: round4));
        _rounds[4] = ((totalBloons: 33, bloons: round5));
        _rounds[5] = ((totalBloons: 4, bloons: round6));
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.Health <= 0) {
            Notification?.Invoke(this, new NotificationEventArgs { Notification = $"You Lost" });
            Invoke(nameof(GameFinished), 3f);
            _player.toggleShop();
            _roundStart = false;
            _hasSpawned = true;
            _totalBloons = 2;
        }
        if (_roundStart) {
            _totalBloons = _rounds[_round].Item1;
            _roundStart = false;
            _hasSpawned = false;
            Notification?.Invoke(this, new NotificationEventArgs { Notification = $"Round {_round}" });
        }
        if (!_hasSpawned)
        {
            if (_bloonCluster < _rounds[_round].Item2.Length && _bloonsSpawned == _rounds[_round].Item2[_bloonCluster].Item1)
            {
                _bloonCluster++;
                _bloonsSpawned = 0;
            }
            if (_bloonCluster < _rounds[_round].Item2.Length) {

                _bloonsSpawned++;
                _hasSpawned = true;

                GameObject bloon = _spawns[_bloonCluster%3].SpawnBloon(_rounds[_round].Item2[_bloonCluster].Item2);
                if (_round < 5)
                {
                    bloon.GetComponent<Bloon>().BloonPopped += OnBloonDestroyed;
                    bloon.GetComponent<Bloon>().NextBloon += OnNextBallon;
                }
                else if(_round == 5) {
                    bloon.GetComponent<MOAB>().BloonPopped += OnBloonDestroyed;
                    bloon.GetComponent<MOAB>().NextBloon += OnNextBallon;
                }

                Invoke(nameof(ResetHasSpawned), 0.5f);

            }
        }
        if (_totalBloons == 0 && !_roundEnd) {
            _roundEnd = true;
            _round++;
            _bloonCluster = 0;
            _bloonsSpawned = 0;
            _player.AddMoney(25);
            _hasSpawned = true;
            if (_round < _rounds.Length)
            {
                Invoke(nameof(ResetRoundStart), 3f);
            }
            else
            {
                Notification?.Invoke(this, new NotificationEventArgs { Notification = "You Won" });
                Invoke(nameof(GameFinished), 3f);
            }
        }
    }

    private void ResetHasSpawned()
    {
        _hasSpawned = false;
    }
    private void ResetRoundStart()
    {
        _roundStart = true;
        _roundEnd = false;
    }

    private void OnBloonDestroyed(object sender, EventArgs e)
    {
        _totalBloons -= 1;
    }
    private void OnNextBallon(object sender, Bloon.NextBloonEventArgs e) {
        e.Next.BloonPopped += OnBloonDestroyed;
        e.Next.NextBloon += OnNextBallon;
    }

    private void GameFinished() {
        int index = 0;
        if (_player.Health > 0)
        {
            index = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            index = index + 1 >= 3 ? 0 : index + 1;
        }
        _player.GetComponentInChildren<FirstPersonLook>().UnlockCursor();
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }

    public event EventHandler<NotificationEventArgs> Notification;
    public class NotificationEventArgs : EventArgs
    {
        public string Notification { get; set; }
    }
}

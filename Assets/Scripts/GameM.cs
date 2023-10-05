using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameM : MonoBehaviour
{
    public static GameM instance;

    [SerializeField] private Text _healthText;
    [SerializeField] private int _health = 100;
    [Space(5)]
    [SerializeField] private Text _goldText;
    [Space(5)]
    [SerializeField] private Enemy _enemy;
    [SerializeField] private GameObject _startCube;
    [SerializeField] private Text _waveText;
    [SerializeField] private Text _waveTimeText;
    [Space(5)]
    [SerializeField] private int _wavesCount = 5;
    [SerializeField] private float _nextWaveTime = 10;
    [SerializeField] private float _spawnInterval = 1;
    [SerializeField] private float _startTime = 5;
    [Space(5)]
    public int _gold = 100;
    public int _turretCost = 100;

    private int _waveIndex;
    private bool _endGame;

    private void Start()
    {
        instance = this;

        _healthText.text = "здоровье: " + _health;
        _waveText.gameObject.SetActive(false);

        UpdateGold();
    }

    private void Update()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        if(_waveIndex >= _wavesCount && enemies.Length == 0 && !_endGame)
        {
            _endGame = true;
            Debug.Log("Victory!");
            SceneManager.LoadScene("win");
        }

        if(_waveIndex >= _wavesCount)
        {
            _waveTimeText.gameObject.SetActive(false);
            return;
        }

        if(_startTime <= 0)
        {
            StartCoroutine(Spawn());
            _startTime = _nextWaveTime;
        }

        _startTime -= Time.deltaTime;
        _startTime = Mathf.Clamp(_startTime, 0, Mathf.Infinity);
        _waveTimeText.text = string.Format("{0:00.00}", _startTime);
        if(_waveIndex > 0)
        {
            _waveText.gameObject.SetActive(true);
            _waveText.text = _waveIndex + "/" + _wavesCount + " волна";
        }
    }

    public void UpdateGold()
    {
        _goldText.text = _gold + "g";
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if(_health <= 0)
        {
            _endGame = true;
            Debug.Log("Defeat!");
            SceneManager.LoadScene("defeat");
        }

        _healthText.text = "здоровье: " + _health;
    }

    IEnumerator Spawn()
    {
        _waveIndex++;

        for(int i = 0; i < _waveIndex; i++)
        {
            Instantiate(_enemy, _startCube.transform.position, _enemy.transform.rotation);
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
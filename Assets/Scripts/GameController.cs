using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using RectTransform = UnityEngine.RectTransform;

public class GameController : MonoBehaviour
{
    public float xSpawn;
    public float ySpawnMin;
    public float ySpawnMax;

    public GameObject player;
    public GameObject[] enemies;

    public float spawnCooldown;
    public float passivePtCooldown;
    public float difficultyIncreaseCooldown;
    public float randomEventCooldown;

    public Scroller scroller;

    public TMP_Text eventText;
    public TMP_Text scoreText;
    public GameObject blackScreen;

    private float _nextSpawnTime;
    private float _nextPointTime;
    private float _nextDifficultyTime;
    private float _nextEventTime;
    private float _enemySpeed = 4f;
    private int _score;
    private bool _gameOver;
    private bool _isWave;

    private Coroutine currentEvent;
    private PlayerMovement _playerMovement;
    private Shoot _shoot;

    private void Start()
    {
        _nextDifficultyTime = Time.time + difficultyIncreaseCooldown;
        _nextEventTime = Time.time + randomEventCooldown;
        _playerMovement = player.GetComponent<PlayerMovement>();
        _shoot = player.GetComponent<Shoot>();
    }

    private void Update()
    {
        if (Time.time > _nextPointTime && !_gameOver)
        {
            _nextPointTime = Time.time + passivePtCooldown;
            _score++;
            scoreText.text = _score.ToString();
        }

        if (Time.time > _nextDifficultyTime && !_gameOver && currentEvent == null)
        {
            _nextDifficultyTime = Time.time + difficultyIncreaseCooldown;
            spawnCooldown *= 0.7f;
            _shoot.fireCooldown *= 0.75f;
            _enemySpeed += 1;

            currentEvent = StartCoroutine(FillEvent(new object[] { "Difficulty increased!" }));
            StartCoroutine(ZombieWave());
        }

        if (Time.time > _nextEventTime && !_gameOver && currentEvent == null)
        {
            _nextEventTime = Time.time + randomEventCooldown;
            DoEvent();
        }

        if (Time.time < _nextSpawnTime || _gameOver) return;
        _nextSpawnTime = Time.time + spawnCooldown;

        var spawnPosition = new Vector3(xSpawn, Random.Range(ySpawnMin, ySpawnMax), 0);
        var chosen = _isWave
            ? enemies[Random.Range(1, enemies.Length)]
            : enemies[Random.Range(0, enemies.Length)];

        var newEnemy = Instantiate(chosen, spawnPosition, Quaternion.identity * Quaternion.Euler(0, 180, 0));
        var enemyScript1 = newEnemy.GetComponent<Enemy1>();
        var enemyScript2 = newEnemy.GetComponent<Enemy2>();
        var enemyScript3 = newEnemy.GetComponent<Enemy3>();
        if (enemyScript1 != null)
        {
            enemyScript1.player = player;
            enemyScript1.gameController = this;
            enemyScript1.SetSpeed(_enemySpeed);
        }
        else if (enemyScript2 != null)
        {
            enemyScript2.gameController = this;
            enemyScript2.SetSpeed(_enemySpeed);
        }
        else if (enemyScript3 != null)
        {
            enemyScript3.gameController = this;
            enemyScript3.SetSpeed(_enemySpeed);
        }
    }

    public void ZombieKill()
    {
        _score += 10;
        scoreText.text = _score.ToString();
    }

    public void GameOver()
    {
        _gameOver = true;
        scroller.GameOver();
        _playerMovement.enabled = false;
        _shoot.enabled = false;
        eventText.text = "";
        object[] parms2 = { 0.3f, true };
        StartCoroutine(BlackThingDown(parms2));
    }

    IEnumerator BlackThingDown(object[] parms)
    {
        float speed = (float)parms[0];
        var rectTransform = blackScreen.GetComponent<RectTransform>();
        if (rectTransform.offsetMin.y - speed <= 0)
        {
            rectTransform.offsetMin = new Vector2(0, 0);
            rectTransform.offsetMax = new Vector2(0, 0);
            if (!(bool)parms[1])
            {
                yield return new WaitForSeconds(1f);
                rectTransform.offsetMin = new Vector2(0, 604);
                rectTransform.offsetMax = new Vector2(0, 604);
                currentEvent = StartCoroutine(FillEvent(new object[] { "Just kidding" }));
            }
        }
        else
        {
            rectTransform.offsetMin = new Vector2(0, rectTransform.offsetMin.y - speed);
            rectTransform.offsetMax = new Vector2(0, rectTransform.offsetMax.y + speed);

            yield return new WaitForSeconds(0.02f);
            object[] parms2 = { speed * 1.2f, parms[1] };
            StartCoroutine(BlackThingDown(parms2));
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator FillEvent(IReadOnlyList<object> parms)
    {
        var text = (string)parms[0];
        eventText.text = text;
        yield return new WaitForSeconds(3);
        eventText.text = "";
        yield return new WaitForSeconds(1);
        currentEvent = null;
    }

    private IEnumerator ZombieWave()
    {
        _isWave = true;
        var oldValue = spawnCooldown;
        spawnCooldown *= 0.35f;
        yield return new WaitForSeconds(1.5f);
        _isWave = false;
        spawnCooldown = oldValue;
    }

    private void DoEvent()
    {
        var random = Random.Range(0, 16);
        switch (random)
        {
            case 0:
            case 1:
                currentEvent = StartCoroutine(FillEvent(new object[] { "Controls inverted!?" }));
                _playerMovement.inverted = !_playerMovement.inverted;
                break;
            case 2:
            case 3:
                currentEvent = StartCoroutine(FillEvent(new object[] { "So tired..." }));
                var oldSpeed = _playerMovement.moveSpeed;
                _playerMovement.moveSpeed *= 0.2f;
                StartCoroutine(RestoreMoveSpeed(new object[] { oldSpeed }));
                break;
            case 4:
            case 5:
                currentEvent = StartCoroutine(FillEvent(new object[] { "BRRRRRRRRRRRRR\n(Hold down space)" }));
                var oldCooldown = _shoot.fireCooldown;
                _shoot.fireCooldown = 0.05f;
                StartCoroutine(RestoreFireCooldown(new object[] { oldCooldown }));
                break;
            case 6:
            case 7:
                currentEvent = StartCoroutine(FillEvent(new object[] { "Dollarama bullets" }));
                _shoot.isCheap = true;
                StartCoroutine(RestoreCheap());
                break;
            case 8:
                object[] parms2 = { 0.3f, false };
                StartCoroutine(BlackThingDown(parms2));
                break;
            case 9:
            case 10:
                currentEvent = StartCoroutine(FillEvent(new object[] { "BIG BULLETS" }));
                _shoot.isBig = true;
                StartCoroutine(RestoreBig());
                break;
        }
    }

    private IEnumerator RestoreMoveSpeed(IReadOnlyList<object> parms)
    {
        var speed = (float)parms[0];
        yield return new WaitForSeconds(5);
        _playerMovement.moveSpeed = speed;
    }

    private IEnumerator RestoreFireCooldown(IReadOnlyList<object> parms)
    {
        var cooldown = (float)parms[0];
        yield return new WaitForSeconds(4);
        _shoot.fireCooldown = cooldown;
    }

    private IEnumerator RestoreCheap()
    {
        yield return new WaitForSeconds(8);
        _shoot.isCheap = false;
    }
    
    private IEnumerator RestoreBig()
    {
        yield return new WaitForSeconds(5);
        _shoot.isBig = false;
    }
}
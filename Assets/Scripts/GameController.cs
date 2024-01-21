using System;
using System.Collections;
using TMPro;
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

    public Scroller scroller;

    public TMP_Text eventText;
    public TMP_Text scoreText;
    public GameObject blackScreen;

    private float _nextSpawnTime;
    private float _nextPointTime;
    private float _nextDifficultyTime;
    private int _score;
    private bool _gameOver;
    private Coroutine currentEvent;

    private void Start()
    {
        _nextDifficultyTime = Time.time + difficultyIncreaseCooldown;
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
            spawnCooldown *= 0.9f;
            currentEvent = StartCoroutine(FillEvent(new object[] { "Difficulty increased!" }));
        }

        if (Time.time < _nextSpawnTime || _gameOver) return;
        _nextSpawnTime = Time.time + spawnCooldown;

        var spawnPosition = new Vector3(xSpawn, Random.Range(ySpawnMin, ySpawnMax), 0);
        var chosen = enemies[Random.Range(0, enemies.Length)];
        var newEnemy = Instantiate(chosen, spawnPosition, Quaternion.identity * Quaternion.Euler(0, 180, 0));
        var enemyScript1 = newEnemy.GetComponent<Enemy1>();
        var enemyScript2 = newEnemy.GetComponent<Enemy2>();
        var enemyScript3 = newEnemy.GetComponent<Enemy3>();
        if (enemyScript1 != null)
        {
            enemyScript1.player = player;
            enemyScript1.gameController = this;
        }
        else if (enemyScript2 != null) enemyScript2.gameController = this;
        else if (enemyScript3 != null) enemyScript3.gameController = this;
    }

    public void GameOver()
    {
        _gameOver = true;
        scroller.GameOver();
        player.GetComponent<PlayerMovement>().GameOver();
        object[] parms2 = { 0.3f };
        StartCoroutine(BlackThingDown(parms2));
    }

    IEnumerator BlackThingDown(object[] parms)
    {
        float speed = (float)parms[0];
        var rectTransform = blackScreen.GetComponent<RectTransform>();
        if (rectTransform.offsetMin.y - speed <= 0)
        {
            blackScreen.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            blackScreen.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        }
        else
        {
            rectTransform.offsetMin = new Vector2(0, rectTransform.offsetMin.y - speed);
            rectTransform.offsetMax = new Vector2(0, rectTransform.offsetMax.y + speed);

            yield return new WaitForSeconds(0.02f);
            object[] parms2 = { speed * 1.2f };
            StartCoroutine(BlackThingDown(parms2));
        }
    }

    public void ZombieKill()
    {
        _score += 10;
        scoreText.text = _score.ToString();
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator FillEvent(object[] parms)
    {
        var text = (string)parms[0];
        eventText.text = text;
        yield return new WaitForSeconds(3);
        eventText.text = "";
        yield return new WaitForSeconds(1);
        currentEvent = null;
    }
}
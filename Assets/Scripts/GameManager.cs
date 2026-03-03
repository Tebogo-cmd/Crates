using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;


public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<GameObject> targets;
    private float spawnRate = 1;
    public int Score { private set; get; }
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI pointText;
    public TextMeshProUGUI ScoreTMP;
    public Button restartButton;
    public GameObject gameOverScreen;
    private float defaultFontSize;
    public bool isGameOver = false;
    private bool gameOverScreenShown = false;
    public GameObject titleScreen;

    private Coroutine animationController;

    public Dictionary<string, Color> canvasColors = new()
    {
        {"Addition", new Color(0.2f,0.8f,0.2f,1f) },
        {"Subtration", new Color(0.9f,0.13f,0.07f,1f) }
    };

    void Start()
    {
        defaultFontSize = pointText.fontSize;
       
     
    }

    public void StartGame(int difficulty)
    {
        
        ScoreTMP.gameObject.SetActive(true);
        titleScreen.SetActive(false);
        spawnRate  = (spawnRate /difficulty)*1.6f;

        if(difficulty == 2)
        {
            for(int i = 0; i < 2; i++)
            {
                targets.Add(targets[0]);
            }
        }
        if(difficulty == 3)
        {
            for (int i = 0; i < 4; i++)
            {
                targets.Add(targets[0]);
            }
        }

        UpdateScore(0);
        StartCoroutine(SpawnTarget());
    }

    // Update is called once per frame
    void Update()
    {
        if (Score <= -20 && !gameOverScreenShown)
        {
            isGameOver = true;
            Instantiate(gameOverScreen); //show the gameOver Animation
            gameOverScreenShown = true;
            StartCoroutine(waitThenShowRestart());
            
        }
    }

    IEnumerator waitThenShowRestart()
    {
        yield return new WaitForSeconds(2.5f);
        restartButton.gameObject.SetActive(true); //show restartbutton
    }


    IEnumerator SpawnTarget()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(spawnRate);
            Instantiate(targets[(int)random(0, targets.Count)]);
        }

    }



    float random(float min, float max)
    {
        return Random.Range(min, max);
    }

    public void UpdateScore(int score)
    {
        this.Score += score;
        scoreTxt.text = "Score: " + this.Score;

        pointText.text = (score > 0) ? "+" + score : "-" + Mathf.Abs(score);
    }



    IEnumerator textAnimation()   //Poor naming: This is animation for the point system
    {
        pointText.alpha = 1;
        for (int i = 0; i < 5; i++)
        {
            pointText.fontSize += 10;
            yield return new WaitForSeconds(0.1f);
        }
        pointText.fontSize = defaultFontSize;
        pointText.alpha = 0;

    }


    public void showAnimation()
    {
        if (animationController != null)
            StopCoroutine(animationController);


        animationController = StartCoroutine(textAnimation());
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }




}

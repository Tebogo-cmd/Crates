using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<GameObject> targets;
    private float spawnRate = 1;
    public int Score { private set; get; }
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI pointText;
    public TextMeshProUGUI ScoreTMP;
    public TextMeshProUGUI timerTxt;
    private int timer = 60;
    public Slider slider;
    public Toggle timerToggle;
    public Button restartButton;
    public GameObject gameOverScreen;
    private float defaultFontSize;
    public bool isGameOver = false;
    private bool gameOverScreenShown = false;
    public GameObject titleScreen;
    private Coroutine TimerCoroutine;
    private AudioSource audioSourceSFX;
    private AudioSource audioSourceBackground;
    public AudioClip[] sounds = new AudioClip[4]; //0-> modeSelection 1->TargetCollision 2->ExplosionSound 3->backgroundSound

    private Coroutine animationController;
    public float soundVolume = 0;
    public Dictionary<string, Color> canvasColors = new()
    {
        {"Addition", new Color(0.2f,0.8f,0.2f,1f) },
        {"Subtration", new Color(0.9f,0.13f,0.07f,1f) }
    };

    void Start()
    {
        defaultFontSize = pointText.fontSize;
        audioSourceSFX = GetComponent<AudioSource>();
        audioSourceBackground = gameObject.AddComponent<AudioSource>();
        soundVolume = slider.value;
        audioSourceBackground.clip = sounds[3];
        audioSourceBackground.loop = true;
        audioSourceBackground.volume = slider.value ;
        audioSourceBackground.Play();
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

        if (timerToggle.isOn)
        {
            timerTxt.gameObject.SetActive(true);
           TimerCoroutine= StartCoroutine(CountDown());
        }

        UpdateScore(0);
        StartCoroutine(SpawnTarget());
    }

    public void ChangeVolume()
    {
        soundVolume = slider.value;
        audioSourceBackground.volume = slider.value;
    }

    // Update is called once per frame
    void Update()
    {
        if (Score <= -20 && !gameOverScreenShown)
        {
            GameOver();
        }
    }


    void GameOver()
    {
        isGameOver = true;
        Instantiate(gameOverScreen); //show the gameOver Animation
        gameOverScreenShown = true;
        StopCoroutine(TimerCoroutine);
        StartCoroutine(waitThenShowRestart());

    }


    IEnumerator CountDown()
    {
        while (true)
        {
            if (timer == 0)
                break;
            else
            {
                
                yield return new WaitForSeconds(1);
                --timer;
                timerTxt.text = "Timer: " + timer;
            }

        }

        GameOver();
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

   

    public void PlayAudio(int index)//0-> modeSelection 1->TargetCollision 2->ExplosionSound
    {
        switch (index)
        {
            case 0: audioSourceSFX.PlayOneShot(sounds[0], soundVolume);
                break;
            case 1: audioSourceSFX.PlayOneShot(sounds[1], soundVolume);
                break;
            case 2: audioSourceSFX.PlayOneShot(sounds[2], soundVolume);
                break;
            default: Debug.Log($"Requested audio sound Index: {index} Not found!\n. Number of sounds: {sounds.Length} available");
                break;
        }
    }



}

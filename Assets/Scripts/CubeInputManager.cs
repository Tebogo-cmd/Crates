using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class CubeEraser : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public InputAction cubeInput;
    private Vector3 nextCubePos = Vector3.zero;
    private Vector3 restingPos;
    private bool isTimeUp = true;
    private int badFood = -10;  //starting var for point deduction
    private GameManager gameManagerScript;
    public ParticleSystem[] explosionParticle = new ParticleSystem[4];




    private void Awake()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();


    }

    void Start()
    {
        restingPos = transform.position;


        gameManagerScript.pointText.alpha = 0;

    }

    private void OnEnable()
    {
        cubeInput.Enable();
        cubeInput.performed += handleCubeInput;
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void handleCubeInput(InputAction.CallbackContext context)
    {
        if (!gameManagerScript.isGameOver) //if game is over do not allow Cube Input to Move
        {
            if (isTimeUp)
            {
                nextCubePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                nextCubePos = new Vector3(nextCubePos.x, nextCubePos.y, 0);
                transform.position = nextCubePos;
                isTimeUp = false;
                StartCoroutine(HideCube());
            }
            else
                Debug.Log("Wait!");
        }
        else
        {
            transform.position = restingPos;  //move the cube to the restingPosition
        }
    }

    IEnumerator HideCube()
    {
        yield return new WaitForSeconds(0.25f);
        nextCubePos = restingPos;
        transform.position = nextCubePos;
        yield return new WaitForSeconds(0.1f);
        isTimeUp = true;
    }
    private void OnDisable()
    {
        cubeInput.performed -= handleCubeInput;
        cubeInput.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        int particleIndex = 0;

        gameManagerScript.pointText.color = gameManagerScript.canvasColors["Addition"];

        if (other.gameObject.CompareTag("Good"))
        {
            gameManagerScript.UpdateScore(10);
            particleIndex = 0;
        }
        else if (other.gameObject.CompareTag("Good1"))
        {
            gameManagerScript.UpdateScore(20);
            particleIndex = 1;

        }
        else if (other.gameObject.CompareTag("Good2"))
        {
            gameManagerScript.UpdateScore((int)Random.Range(1, 50));
            particleIndex = 2;

        }
        else if (other.gameObject.CompareTag("Bad"))
        {
            gameManagerScript.UpdateScore(badFood);
            gameManagerScript.pointText.color = gameManagerScript.canvasColors["Subtration"];
            particleIndex = 3;

            badFood -= 20;
        }

       gameManagerScript.showAnimation();
        Destroy(other.gameObject);
        Instantiate(explosionParticle[particleIndex], transform.position, transform.rotation);
        gameManagerScript.PlayAudio(2);
    }




}

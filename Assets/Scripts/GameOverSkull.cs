using UnityEngine;
using System.Collections;
using TMPro;


public class GameOverSkull : MonoBehaviour
{

    private bool lockedIn = false;
    private TextMeshProUGUI gameOverText;
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverText = GameObject.FindGameObjectWithTag("gameOverText").GetComponent<TextMeshProUGUI>();
        transform.position = new Vector3(0, 1, 0);
        StartCoroutine(aniamateSkull());
    }

    // Update is called once per frame
    void Update()
    {
        if(lockedIn)
        transform.Rotate(Vector3.up*Time.deltaTime, 0.8f);
    }
    

    void cleanUpCanvas()
    {
       
    }


    IEnumerator aniamateSkull()
    {

        cleanUpCanvas();


        for (int i = 0; i < 5; i++)
        {
            transform.Translate(Vector3.up);
            yield return new WaitForSeconds(0.2f);
        }
     
        lockedIn = true;
        StartCoroutine(animateText());
       
    }


    IEnumerator animateText()
    {

        gameOverText.alpha = 1;
        while (true)
        {
            gameOverText.fontSize += 5;
            yield return new WaitForSeconds(0.3f);
            gameOverText.fontSize -= 5;
            yield return new WaitForSeconds(0.3f);

        }
    
    }



}

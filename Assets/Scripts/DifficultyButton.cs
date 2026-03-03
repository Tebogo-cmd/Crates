using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Button button;
    private GameManager gameManagerScript;
   
    public int difficultyLevel;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(HandleButton);
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void HandleButton()
    {
      
        gameManagerScript.StartGame(difficultyLevel);


    }
}

using NUnit.Framework;
using UnityEngine;

public class Target : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody target;
    private GameManager gameManagerScript;
   


    private void Awake()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Start()
    {
        target = GetComponent<Rigidbody>();
        target.AddForce(Vector3.up * random(12, 16), ForceMode.Impulse);
        target.AddTorque(random(), random(), random());
        transform.position = new Vector3(random(-5, 5), 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sensor"))
        {
           
         

            if (!gameManagerScript.isGameOver && !gameObject.CompareTag("Bad"))
            {
                gameManagerScript.UpdateScore(-5); //Everytime player misses a target - 5
                gameManagerScript.pointText.color = gameManagerScript.canvasColors["Subtration"];
                gameManagerScript.showAnimation();
            }
            Destroy(gameObject);

        }

    }


    float random()
    {
        return Random.Range(-10, 10);
    }
    float random(float min, float max)
    {
        return Random.Range(min, max);
    }

}

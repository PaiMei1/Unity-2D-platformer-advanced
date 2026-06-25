using UnityEngine;
using UnityEngine.SceneManagement;
public class ENDScript : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
             SceneManager.LoadScene(0);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created.
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

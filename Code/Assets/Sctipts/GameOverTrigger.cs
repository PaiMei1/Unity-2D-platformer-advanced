using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    
    [SerializeField] private UIManager uiManager;
[SerializeField] private AudioClip gameOverSound;
    public void TriggerGameOver()
    {
        uiManager.GameOver();
SoundManager.instance.PlaySound(gameOverSound);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

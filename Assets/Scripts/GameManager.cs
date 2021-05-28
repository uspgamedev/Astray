using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool playerAlive = true;
    public bool gameIsPaused;
    public GameObject gameOverScreen;
    public GameObject winGameScreen;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Water"), LayerMask.NameToLayer("Shots"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Shots"), LayerMask.NameToLayer("IgnoreBullet"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Water"), LayerMask.NameToLayer("Boss"));
        gameIsPaused = true;
        Time.timeScale = 0f;
    }

    public void GameOver()
    {
        FindObjectOfType<AudioManager>().Play("GameOver");
        gameOverScreen.SetActive(true);
        playerAlive = false;
    }

    public void WinGame()
    {
        FindObjectOfType<AudioManager>().Play("WinGame");
        Time.timeScale = 0f;
        winGameScreen.SetActive(true);
    }
}

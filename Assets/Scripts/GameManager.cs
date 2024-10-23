using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public KeyCode gameOverKeyCode = KeyCode.Escape;
    private Timer timer;
    private ScoreManager scoreManager;
    public TextMeshProUGUI gameOverText;

    private bool gameIsOver = false;
    private bool gameIsWon = false;

    // Add audio variables
    public AudioSource winSound;
    public AudioSource loseSound;
    public AudioSource backgroundMusic;

    private void Start()
    {

        timer = FindObjectOfType<Timer>();
        scoreManager = FindObjectOfType<ScoreManager>();
        gameOverText.gameObject.SetActive(false);
    }

    private void Update()
    {

        if (Input.GetKeyDown(gameOverKeyCode))
        {
            QuitGame();
        }


        LostGame();
        WinGame();

        // If the game is over, listen for the Enter key to restart
        if ((gameIsOver || gameIsWon) && Input.GetKeyDown(KeyCode.Return))
        {
            RestartGame();  // Restart the game
        }
    }

    private void LostGame()
    {

        if (timer != null && timer.timeRemaining <= 0)
        {
            if (!gameIsOver && !gameIsWon)
            {
                Debug.Log("You lost");
                gameOverText.text = "You Lost. Press Enter to try again.";
                gameOverText.color = Color.red;
                gameOverText.gameObject.SetActive(true);
                gameIsOver = true;


                if (backgroundMusic != null && backgroundMusic.isPlaying)
                {
                    backgroundMusic.Stop();
                }

                if (loseSound != null)
                {
                    loseSound.Play();
                }
            }
        }
    }

    private void WinGame()
    {

        if (scoreManager != null && scoreManager.score >= 10 && timer.timeRemaining > 0)
        {
            if (!gameIsOver && !gameIsWon)
            {
                Debug.Log("You won");
                gameOverText.text = "You Won! Press Enter to play again!";
                gameOverText.color = Color.green;
                gameOverText.gameObject.SetActive(true);
                gameIsWon = true;


                if (backgroundMusic != null && backgroundMusic.isPlaying)
                {
                    backgroundMusic.Stop();
                }


                if (winSound != null)
                {
                    winSound.Play();
                }
            }
        }
    }


    void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        // If running in the editor, stop play mode
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }


    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

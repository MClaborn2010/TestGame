using UnityEngine;
using UnityEngine.UI;  // Only needed if you're using UI for displaying the score

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public Text scoreText;  // Link this to a UI Text element in the inspector

    // Method to add points to the score
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
        Debug.Log("Score: " + score);  // Log the score for debugging
    }

    // Update the UI (if using a UI Text element)
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}

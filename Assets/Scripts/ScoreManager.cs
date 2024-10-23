using UnityEngine;
using TMPro;  // Include the TMPro namespace for TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public TextMeshPro scoreText;  // Link this to a TextMeshPro element in the inspector
    public AudioSource scoreSound;

    // Method to add points to the score
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
        Debug.Log("Score: " + score);  // Log the score for debugging
        scoreSound.Play();
    }

    // Update the UI (if using a TextMeshPro element)
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Points: " + score;  // Update the TextMeshPro text
        }
    }
}

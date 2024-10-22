using UnityEngine;

public class TriggerLogger : MonoBehaviour
{
    public ScoreManager scoreManager;  // Drag and drop the ScoreManager into this field in the Inspector
    public int pointsPerGoal = 1;  // Define how many points are awarded per goal

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))  // Ensure only the ball increments the score
        {
            Debug.Log("Ball entered the goal: " + other.gameObject.name);
            scoreManager.AddScore(pointsPerGoal);  // Add points to the score
        }
    }
}

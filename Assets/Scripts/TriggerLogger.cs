using UnityEngine;

public class TriggerLogger : MonoBehaviour
{
    public ScoreManager scoreManager;  // Drag and drop the ScoreManager into this field in the Inspector
    public int pointsPerGoal = 1;  // Define how many points are awarded per goal
    public Transform goalTop;  // Reference to the top of the goal or the trigger

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))  // Ensure only the ball increments the score
        {
            // Check if the ball is above the goal when entering the trigger
            if (other.transform.position.y > goalTop.position.y)
            {
                Debug.Log("Valid score! Ball entered from above.");
                scoreManager.AddScore(pointsPerGoal);  // Add points to the score
            }
            else
            {
                Debug.Log("Invalid score! Ball entered from below.");
            }
        }
    }
}

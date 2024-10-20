using UnityEngine;

public class InteractablesManager : MonoBehaviour
{
    private void Start()
    {
        // Get all child GameObjects
        foreach (Transform child in transform)
        {
            // Check if the child already has an Interactable component
            if (child.GetComponent<Interactable>() == null)
            {
                // Add the Interactable component to the child GameObject
                child.gameObject.AddComponent<Interactable>();
            }
        }
    }
}

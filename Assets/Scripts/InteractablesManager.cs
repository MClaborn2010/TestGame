using UnityEngine;

public class InteractablesManager : MonoBehaviour
{

    // This just takes all items within the InteractableObjects Game Object and assigns them with the Interactable class. 
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

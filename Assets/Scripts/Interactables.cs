using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Interactable Properties")]
    private string interactableName; // Name displayed in logs or UI
    public bool isInteractable = true; // To check if this object can be interacted with

     private void Start()
    {
        // Automatically set interactableName to the GameObject's name
        interactableName = gameObject.name;
    }

    // This method is called to interact with the object
    public virtual void Interact()
    {
        if (isInteractable)
        {
            // Log or handle the interaction
            Debug.Log($"{interactableName} has been interacted with.");
        }
        else
        {
            Debug.Log($"{interactableName} is not interactable.");
        }
    }
}

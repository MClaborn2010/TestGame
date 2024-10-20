using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRange = 3f; // Maximum distance to interact
    public Camera playerCamera; // Reference to the player's camera
    private Interactable currentInteractable; // Reference to the currently interacted object

    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main; // Automatically assign the main camera
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Example interaction key
        {
            Interact();
        }

       
    }

    private void Interact()
    {
        RaycastHit hit;

        // Cast a ray from the camera forward
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionRange))
        {
            // Check if the object hit is an Interactable
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                // Call the Interact method
                interactable.Interact();
                currentInteractable = interactable; // Store the reference to the interactable
            }
             
            // GameObjectUtility.DestroyGameObject(currentInteractable.gameObject); This is an example of how to use a method from another class. Pretty simple. 
                
        }
        
    }
}

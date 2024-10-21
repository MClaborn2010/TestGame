using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRange = 3f; // Maximum distance to interact
    public Camera playerCamera; // Reference to the player's camera
    private Interactable currentInteractable; // Reference to the currently interacted object
    public KeyCode interactKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.G;
    public GameObject heldItemObject;

    [Header("Held Item")]
    public Transform heldItem;
    public GameObject interactableObjects;

    private bool isHeldItem = false; // Track if an item is currently held

    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main; // Automatically assign the main camera
        }
    }

    private void Update()
    {
        // Check if there is an interactable in range
        if (IsInteractableInRange())
        {
            if (Input.GetKeyDown(interactKey)) // Example interaction key
            {
                Interact();
            }
        }

        // Allow dropping the item at any time
        if (Input.GetKeyDown(dropKey) && isHeldItem) // Only drop if an item is held
        {
            DropItem();
        }
    }

    private bool IsInteractableInRange()
    {
        RaycastHit hit;

        // Cast a ray from the camera forward
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionRange))
        {
            // Check if the object hit is an Interactable
            currentInteractable = hit.collider.GetComponent<Interactable>();
            return currentInteractable != null; // Return true if there's an interactable
        }

        currentInteractable = null; // Reset if no interactable found
        return false;
    }

    private void Interact()
    {
        if (currentInteractable != null)
        {
            // Get the item from InteractableObjects
            heldItemObject = currentInteractable.gameObject;

            // Remove the item from InteractableObjects
            heldItemObject.transform.SetParent(null); // Detach from its current parent

            // Remove any Rigidbody to ensure it can be held without physics
            Rigidbody rb = heldItemObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Destroy(rb); // Remove the Rigidbody if it exists
            }

            // Set its parent to HeldItem
            heldItemObject.transform.SetParent(heldItem);

            // Reset position to be relative to HeldItem
            heldItemObject.transform.localPosition = Vector3.zero; // Center it within HeldItem

            // Set isHeldItem to true
            isHeldItem = true;

            // Reset currentInteractable after interaction
            currentInteractable = null; // Reset after interaction
        }
    }

    private void DropItem()
    {
        if (heldItemObject != null)
        {
            // Set the parent to null to remove it from HeldItem
            heldItemObject.transform.SetParent(null);

            // Set its position to where the player is looking, with a small upward offset
            heldItemObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward * 1.5f;
            heldItemObject.transform.position += Vector3.up * 0.5f; // Adjust height to avoid ground clipping

            // Set the parent to InteractableObjects
            heldItemObject.transform.SetParent(interactableObjects.transform);

            // Add Rigidbody if it doesn't exist
            if (heldItemObject.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = heldItemObject.AddComponent<Rigidbody>();
                rb.mass = 1f; // Set mass as needed
            }

            // Reset the reference and isHeldItem
            heldItemObject = null;
            isHeldItem = false;
        }
    }
}

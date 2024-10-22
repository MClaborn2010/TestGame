using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")] // These are Headers. Use these to create categories within your inspector. 
    public float interactionRange = 3f; // Maximum distance to interact
    public float throwPower;

    [Header("Camera")]
    public Camera playerCamera; // Reference to the player's camera

    [Header("Key Codes")] // Key Codes set to default values. Making these public allows you to change them directly from the editor. 
    public KeyCode interactKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.G;
    public KeyCode throwKey = KeyCode.Mouse0;

    [Header("Held Item")]
    public Transform heldItem;

    public GameObject interactableObjects; // A reference to any object in the InteractableObjects Game Object. 

    private bool isHeldItem = false; // Track if an item is currently held

    private Interactable currentInteractable; // Reference to the currently interacted object

    public GameObject heldItemObject; // Reference to currently held object. 


    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main; // Automatically assign the main camera
        }
    }

    private void Update()
    {
        // Simple interaction if interactable is in range
        if (IsInteractableInRange())
        {
            if (Input.GetKeyDown(interactKey))
            {
                Interact();
            }
        }

        // Allow drop item if an item is held. 
        if (Input.GetKeyDown(dropKey) && isHeldItem)
        {
            DropItem(0f);
        }

        if (Input.GetKeyDown(throwKey) && isHeldItem)
        {
            ThrowItem(throwPower); // Call the new ThrowItem method
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
        if (currentInteractable != null && !isHeldItem)
        {
            // Add current interactable to heldItem
            heldItemObject = currentInteractable.gameObject;

            // Remove the item from InteractableObjects
            heldItemObject.transform.SetParent(null);

            // Remove any Rigidbody to ensure it can be held without physics. Feel free to add later for some light physics but it was being wonky. 
            Rigidbody rb = heldItemObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Destroy(rb); // Remove the Rigidbody if it exists
            }

            // Set its parent to HeldItem
            heldItemObject.transform.SetParent(heldItem);

            // Reset position to be relative to, and center it within, HeldItem
            heldItemObject.transform.localPosition = Vector3.zero;

            // Set isHeldItem to true
            isHeldItem = true;

            // Reset currentInteractable after interaction
            currentInteractable = null;
        }
    }

    private void DropItem(float forceAmount)
    {
        if (heldItemObject != null)
        {
            heldItemObject.transform.SetParent(null);

            heldItemObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward * 1.5f;
            heldItemObject.transform.position += Vector3.up * 0.5f;

            heldItemObject.transform.SetParent(interactableObjects.transform);

            Rigidbody rb = heldItemObject.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = heldItemObject.AddComponent<Rigidbody>();
                rb.mass = 1f;
            }

            // Apply force if a value is provided
            if (forceAmount > 0f)
            {
                rb.AddForce(playerCamera.transform.forward * forceAmount, ForceMode.Impulse);
            }

            heldItemObject = null;
            isHeldItem = false;
        }
    }

    private void ThrowItem(float forceAmount)
    {
        DropItem(forceAmount); // Call DropItem with a force value to throw the item
    }



}

using UnityEngine;

public class ItemPosition : MonoBehaviour
{
    // This class ensures a held item is always in the bottom right of your screen. 

    public Camera playerCamera; // Reference to the player's camera
    public Vector3 offset; // Offset from the camera's bottom right corner

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main; // Automatically assign the main camera
        }

        // Set an initial offset for the item
        offset = new Vector3(0.5f, 0.1f, 1f); // Adjust Y and Z as needed
    }

    void Update()
    {
        // Set the position based on the camera's viewport
        Vector3 viewportPosition = new Vector3(1, 0, 0); // Bottom right corner in viewport coordinates
        Vector3 worldPosition = playerCamera.ViewportToWorldPoint(viewportPosition);

        // Apply the offset to position the item
        transform.position = worldPosition + offset;
    }
}

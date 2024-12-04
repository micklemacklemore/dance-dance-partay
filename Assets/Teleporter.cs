using System.Collections;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Vector3 teleportPosition = new Vector3(0, 3, 9); // Position to teleport to
    [SerializeField] private float teleportDelay = 8.448f; // Delay before teleporting (seconds)
    [SerializeField] private GameObject objectToDeactivate; // GameObject to deactivate
    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component from the child GameObject
        audioSource = GetComponentInChildren<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found in child GameObjects.");
        }
    }

    // Function to be called by a button
    public void Trigger()
    {
        if (audioSource != null)
        {
            audioSource.Play(); // Play the audio
        }

        // Start the coroutine to handle the teleportation and deactivation
        StartCoroutine(TeleportAndDeactivate());
    }

    // Coroutine to teleport and deactivate after a delay
    private IEnumerator TeleportAndDeactivate()
    {
        yield return new WaitForSeconds(teleportDelay); // Wait for the specified delay

        // Teleport the GameObject
        transform.position = teleportPosition;

        // Deactivate the specified GameObject and its children
        if (objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false);
        }
    }
}

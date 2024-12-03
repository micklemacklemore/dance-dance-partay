using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroovySpeaker : MonoBehaviour
{
    BeatManager beatManager; 

    private Vector3 baseScale; 

    [SerializeField] private GameObject mesh; 

    // Start is called before the first frame update
    void Start()
    {
        // Find an object with the BeatManager component in the scene
        beatManager = FindObjectOfType<BeatManager>();

        if (beatManager == null)
        {
            Debug.LogError("No BeatManager found in the scene!");
        }

        baseScale = mesh.transform.localScale; 
    }

    // Update is called once per frame
    void Update()
    {
        var modulate = Mathf.Sin(2 * Mathf.PI * beatManager.BeatTime); // will modulate like a bouncing ball
        modulate = Unity.Mathematics.math.remap(-1f, 1f, 0f, 0.1f, modulate); 
        mesh.transform.localScale = baseScale + new Vector3(modulate, modulate, modulate); 
    }
}

using System.Collections;
using System.Collections.Generic;
using Puppet;
using TMPro;
using UnityEngine;

public class DancePartySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _dancers = null; // List of dancer prefabs
    [SerializeField] private TMP_Dropdown dropdown = null; // Reference to dropdown UI
    [SerializeField] private GameObject _sliderContainer = null; 
    [SerializeField] private GameObject _slider = null; 
    private GameObject currentDancer = null; // Store the current dancer as a GameObject
    private int currentIndex = 0; // Current index of the selected dancer
    private bool changeDancer = false; // Flag to trigger dancer change
    private bool newDancer = true; 

    // Start is called before the first frame update
    void Start()
    {
        currentIndex = dropdown.value; 
        SpawnDancer(currentIndex); // Spawn the initial dancer
    }

    // Update is called once per frame
    void Update()
    {
        if (newDancer)
        {
            // populate sliders
            var dancer = currentDancer.GetComponentInChildren<DancerBase>();

            foreach (KeyValuePair<string, SetFloat> entry in dancer.propFloats) {
                // Debug.Log(entry.Key); 
                var newSlider = Instantiate(_slider); 
                newSlider.transform.SetParent(_sliderContainer.transform); 
                newSlider.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f); 
                newSlider.SetActive(true); 
                newDancer = false; 
            }
        }
        // Check if the flag to change dancer is set
        if (changeDancer)
        {
            changeDancer = false; // Reset flag
            SwapDancer(currentIndex); // Call function to handle swapping
        }
    }

    // Function to get the dropdown value and set the change flag
    public void getDropdown()
    {
        currentIndex = dropdown.value; // Update current index based on dropdown selection
        changeDancer = true; // Set flag to change dancer
    }

    // Function to spawn a dancer
    private void SpawnDancer(int index)
    {
        // Instantiate the selected prefab
        var pos = new Vector3(0, 0, 0);
        var rot = Quaternion.identity;
        currentDancer = Instantiate(_dancers[index], pos, rot);

        // Change the color of the new dancer (optional)
        var render = currentDancer.GetComponentInChildren<Renderer>();
        render.material.color = new Color(1f, 0f, 0f, 1f);

        // delete old sliders
        for (int i = 1; i < _sliderContainer.transform.childCount; ++i) {
            Destroy (_sliderContainer.transform.GetChild(i).gameObject); 
        }

        newDancer = true; 
    }

    // Function to swap the current dancer with a new one
    private void SwapDancer(int newIndex)
    {
        // Destroy the current dancer if it exists
        if (currentDancer != null)
        {
            Destroy(currentDancer);
        }

        // Spawn the new dancer
        SpawnDancer(newIndex);
    }

    public void TestSlider(string property, float value) {
        Debug.Log($"Spawner: Property: {property}, Value: {value}");
    }
}
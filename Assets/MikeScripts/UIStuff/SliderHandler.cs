using UnityEngine;
using UnityEngine.UI;

public class SliderHandler : MonoBehaviour
{
    [SerializeField] private string property = "default"; // Static label
    [SerializeField] private Slider slider; // Reference to the slider
    [SerializeField] private DancePartySpawner spawner; 

    public string propertyName {
        set { property = value; }
    }

    void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        if (slider.wholeNumbers) {
            int val = Mathf.RoundToInt(value); 
            spawner.SetDanceProperty(property, val);
            return;  
        }
        spawner.SetDanceProperty(property, value); 
    }
}
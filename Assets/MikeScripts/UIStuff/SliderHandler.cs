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
        spawner.SetDanceProperty(property, value); 
    }
}
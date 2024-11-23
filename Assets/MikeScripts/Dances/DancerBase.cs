using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SetFloat {
    public delegate void SetFloatDelegate(float x);  
    public SetFloatDelegate action; 
    public float min; 
    public float max; 
    public float current; 

    public SetFloat(SetFloatDelegate action, float min, float max, float current) {
        this.action = action;
        this.min = min;
        this.max = max;
        this.current = Mathf.Clamp(current, min, max);
    }

    public void SetValue(float x)
    {
        current = Mathf.Clamp(x, min, max); // Clamp the value within min and max
        action?.Invoke(current);           // Invoke the delegate if it's not null
    }
}

public class DancerBase : MonoBehaviour
{
    
    public Dictionary<string, SetFloat> propFloats = new Dictionary<string, SetFloat>(); 

    public void SetDanceProperty(string name, float value) {
        if (propFloats.TryGetValue(name, out var setfloat))
        {
            setfloat.action(value); 
        }
        else
        {
            Debug.LogWarning($"Property '{name}' not found.");
        }
    }

    public void SetDanceProperty(string name, int value) {
        Debug.LogWarning($"Property '{name}' not found.");
    }
}

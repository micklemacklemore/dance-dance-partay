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

public struct SetInt {
    public delegate void SetIntDelegate(int x);  
    public SetIntDelegate action; 
    public int min; 
    public int max; 
    public int current; 

    public SetInt(SetIntDelegate action, int min, int max, int current) {
        this.action = action;
        this.min = min;
        this.max = max;
        this.current = Mathf.Clamp(current, min, max);
    }

    public void SetValue(int x)
    {
        current = Mathf.Clamp(x, min, max); // Clamp the value within min and max
        action?.Invoke(current);           // Invoke the delegate if it's not null
    }
}

public class DancerBase : MonoBehaviour
{
    protected BeatManager beatManager; 

    public Dictionary<string, SetFloat> propFloats = new Dictionary<string, SetFloat>(); 
    public Dictionary<string, SetInt> propInts = new Dictionary<string, SetInt>(); 

    public virtual void initializeProperties() {
        // Override this to fill propFloats, propInts, etc.
        // Find an object with the BeatManager component in the scene
        beatManager = FindObjectOfType<BeatManager>();

        if (beatManager == null)
        {
            Debug.LogError("No BeatManager found in the scene!");
        }
    }

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
        if (propInts.TryGetValue(name, out var setint))
        {
            setint.action(value); 
        }
        else
        {
            Debug.LogWarning($"Property '{name}' not found.");
        }
    }

    public virtual void BeatTrigger() {
        // override this to reset the beat / step
        Debug.Log("Trigger!!!"); 
    }
}

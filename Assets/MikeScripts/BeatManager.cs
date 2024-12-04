using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [SerializeField] private float _bpm = 118f;
    private float _beatInterval; // Duration of one beat in seconds
    private float _lastBeatTime = 0.0f; // Time of the last detected beat
    private int _beatCounter = 0; // Tracks the beat count
    private bool _newCycle = false; // Tracks whether a new cycle has started
    private bool _resetBeat = false; 
    private float _beatTime = 0; 

    // Getters & Setters
    public bool NewCycle
    {
        get { return _newCycle; }
    }

    public float BeatTime
    {
        get { return _beatTime; }
    }

    public float BeatTimeSmoothStep
    {
        get { return Mathf.SmoothStep(0.0f, 1.0f, _beatTime); }
    }

    public float TimeToNextBeat
    {
        get { return _beatInterval - (Time.time - _lastBeatTime); }
    }

    public float BeatCounter 
    {
        get { return _beatCounter; }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _beatInterval = 60.0f / _bpm;
            // Detect new beat cycles based on BPM

            if (_resetBeat) {
                ResetBeatTime(); 
                _resetBeat = false; 
            }

        DetectBeatCycle();

        // Time t = [0 --> 1] that represents the time between one beat to the next
        _beatTime = (Time.time - _lastBeatTime) / _beatInterval;
    }

    public void ResetBeatTime()
    {
        _lastBeatTime = Time.time;
        _beatTime = 0.0f;
    }

    private void DetectBeatCycle()
    {
        float currentTime = Time.time;

        if (currentTime - _lastBeatTime >= _beatInterval)
        {
            _lastBeatTime += _beatInterval; // Update to the current beat
            _newCycle = true; // A new cycle begins
            _beatCounter++; // Increment beat counter
            //Debug.Log($"New Cycle: {_beatCounter}");
        }
        else
        {
            _newCycle = false; // No new cycle
        }
    }
}

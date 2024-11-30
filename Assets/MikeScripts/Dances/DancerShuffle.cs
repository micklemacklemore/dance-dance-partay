using UnityEngine;
using Unity.Mathematics;
using Klak.Math;
using UnityEngine.Audio;
using Noise = Klak.Math.NoiseHelper;
using math = Unity.Mathematics;
using System;

namespace Puppet
{
    public class DancerShuffle : DancerBase
    {
        [SerializeField] private float bpm = 118f;
        private float beatInterval; // Duration of one beat in seconds
        private float lastBeatTime = 0.0f; // Time of the last detected beat
        private int beatCounter = 0; // Tracks the beat count
        private bool newCycle = false; // Tracks whether a new cycle has started

        [SerializeField] public float _frequency = 1.2f;
        [SerializeField] private AudioSource audioSource = null;

        // Body movement variables
        private Vector3 _bodyPosition;
        private Quaternion _bodyRotation;
        private Quaternion _targetRotation;
        private float _maxJumpingHeight = 0.5f;
        private float _offset = 0.0f;
        private float _targetOffset = 0.0f;
        private float _transitionSpeed = 5.0f;

        // Foot and knee positions
        private Vector3[] _feet = new Vector3[2];
        private Vector3[] _knees = new Vector3[2];

        Animator _animator;

        public override void initializeProperties()
        {
            base.initializeProperties();
            this.propFloats["BPM"] = new SetFloat((x) => bpm = x, 1.0f, 200.0f, bpm); 
            this.propFloats["Max Jumping Height"] = new SetFloat((x) => _maxJumpingHeight = x, 0.2f, 5.0f, _maxJumpingHeight);
        }

        void Start()
        {
            // Initialize beat interval
            beatInterval = 60.0f / bpm;

            _animator = GetComponent<Animator>();
            _bodyPosition = transform.position;
            _bodyRotation = transform.rotation;
            _targetRotation = _bodyRotation;

            // Initialize feet and knee positions
            Vector3 origin = transform.position;
            Vector3 footOffset = transform.right * 0.2f;

            _feet[0] = origin - footOffset;
            _feet[1] = origin + footOffset;

            Vector3 kneeOffset = transform.forward * 0.4f;
            _knees[0] = origin - footOffset + kneeOffset;
            _knees[1] = origin + footOffset + kneeOffset;

            initializeProperties();
        }

        void Update()
        {
            beatInterval = 60.0f / bpm;
            // Detect new beat cycles based on BPM
            DetectBeatCycle();

            // Update position and rotation
            UpdateBodyPosition();
            //UpdateBodyRotation();

            // Play audio on beat
            PlayMetronomeOnBeat();
        }

        /// <summary>
        /// Detects whether a new cycle has started based on the BPM.
        /// </summary>
        private void DetectBeatCycle()
        {
            float currentTime = Time.time;

            if (currentTime - lastBeatTime >= beatInterval)
            {
                lastBeatTime += beatInterval; // Update to the current beat
                newCycle = true; // A new cycle begins
                beatCounter++; // Increment beat counter
                Debug.Log($"New Cycle: {beatCounter}");
            }
            else
            {
                newCycle = false; // No new cycle
            }
        }

        private void UpdateBodyPosition()
        {
            // Calculate time progress in the beat cycle (0 to 1)
            float beatTime = (Time.time - lastBeatTime) / beatInterval;

            // Sine wave modulation based on the beat cycle
            float modulate = Mathf.Sin(2 * Mathf.PI * beatTime);

            if (newCycle)
            {
                // Generate a new target offset for the next jump height
                _targetOffset = (0.5f * UnityEngine.Random.Range(-1.0f, 1.0f) + 1.0f) * _maxJumpingHeight;
            }

            // Smoothly transition _offset towards _targetOffset
            _offset = Mathf.Lerp(_offset, _targetOffset, Time.deltaTime * _transitionSpeed);

            // Calculate the new Y position based on sine wave modulation
            float newY = modulate * _maxJumpingHeight + _offset;

            // Apply the new Y position
            _bodyPosition.y = transform.position.y + 0.5f + newY;
        }

        /// <summary>
        /// Updates the body rotation with smooth transitions.
        /// </summary>
        private void UpdateBodyRotation()
        {
            if (newCycle)
            {
                // Generate a new random target rotation on each cycle
                float randomYaw = UnityEngine.Random.Range(-360.0f, 360.0f); // Random yaw rotation

                Vector3 currentEuler = _bodyRotation.eulerAngles;
                _targetRotation = Quaternion.Euler(currentEuler.x, randomYaw, currentEuler.z);
            }

            // Smoothly interpolate the current rotation towards the target rotation
            _bodyRotation = Quaternion.Slerp(_bodyRotation, _targetRotation, Time.deltaTime * _transitionSpeed);
        }

        /// <summary>
        /// Plays a metronome sound on each beat.
        /// </summary>
        private void PlayMetronomeOnBeat()
        {
            if (newCycle && audioSource != null && audioSource.clip != null)
            {
                audioSource.Play(); // Play the metronome sound
            }
            else if (newCycle && (audioSource == null || audioSource.clip == null))
            {
                Debug.LogWarning("AudioSource or AudioClip is not assigned.");
            }
        }

        void OnAnimatorIK(int layerIndex)
        {
            // Update the feet positions
            _animator.SetIKPosition(AvatarIKGoal.LeftFoot, _feet[0]);
            _animator.SetIKPosition(AvatarIKGoal.RightFoot, _feet[1]);
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
            _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);

            // Update the knee positions
            _animator.SetIKHintPosition(AvatarIKHint.LeftKnee, _knees[0]);
            _animator.SetIKHintPosition(AvatarIKHint.RightKnee, _knees[1]);
            _animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, 1);
            _animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, 1);

            // Update the body position and rotation
            _animator.bodyPosition = _bodyPosition;
            _animator.bodyRotation = _bodyRotation;
        }
    }
}

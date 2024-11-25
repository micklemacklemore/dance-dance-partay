using UnityEngine;
using Unity.Mathematics;
using Klak.Math;
using Noise = Klak.Math.NoiseHelper;
using math = Unity.Mathematics;
using System;

namespace Puppet
{
    public class DancerShuffle : DancerBase
    {
        [SerializeField] public float _footDistance = 0.3f;
        [SerializeField] public Vector3 _bodyPosition;  

        [SerializeField] public float _noiseFrequency = 1.1f;
        [SerializeField] public uint _randomSeed = 123;

        [SerializeField] public float _frequency = 1.2f; 

        [SerializeField] public float _maxJumpingHeight = 0.5f; 


        private Quaternion _bodyRotation; 

        private int _debugCycle = 0; 
        // [SerializeField] private float _rotationSpeed = 0.2f; // Speed for smoothing rotation
        // private Quaternion _currentRotation;  // Current rotation
        private float _offset = 0.0f; 
        private float _targetOffset = 0.0f; 
        private float _transitionSpeed = 5.0f; 

        private Quaternion _targetRotation;   // Target rotation
        

        private bool _newCycle = false; 


        Animator _animator;

        XXHash _hash;
        float2 _noise;

        public override void initializeProperties() {
            base.initializeProperties();
            this.propFloats["Max Jumping Height"] = new SetFloat((x) => _maxJumpingHeight = x, 0.2f, 5.0f, _maxJumpingHeight); 
        }

        // Foot positions
        public Vector3[] _feet = new Vector3[2];
        public Vector3[] _knees = new Vector3[2]; 

        static Vector3 SetY(Vector3 v, float y)
        {
            v.y = y;
            return v;
        }

        void OnValidate()
        {
            _footDistance = Mathf.Max(_footDistance, 0.01f);
        }

        void Start()
        {
            _animator = GetComponent<Animator>();

            // Random number/noise generators
            _hash = new XXHash(_randomSeed);
            _noise = _hash.Float2(-1000, 1000, 0);
            
            UnityEngine.Vector3 pos = transform.position; 

            transform.position = new UnityEngine.Vector3(pos.x, 0.85f, pos.z); 

            _bodyPosition = transform.position; 
            _bodyRotation = transform.rotation;
            _targetRotation = _bodyRotation; 
            
            pos.y = 0.1f; 

            // Initial foot positions
            var origin = pos;
            var foot = transform.right * 0.2f;
            _feet[0] = origin - foot;
            _feet[1] = origin + foot;

            // set the initial knee positions
            pos.y = 0.5f; 
            origin += transform.forward * 0.4f; 

            _knees[0] = origin - foot; 
            _knees[1] = origin + foot; 

            initializeProperties(); 
        }

        #region "Update Functions"

        float fract(float x) {
            return x - Mathf.Floor(x); 
        }

        void UpdateBodyPosition() {

            // Sine wave modulation
            float modulate = Mathf.Sin(2 * Mathf.PI * Time.time * _frequency);

            // Check if modulate goes below the threshold (cycle restart)

            if (_newCycle) {
                // Generate a new target offset for the next jump height
                _targetOffset = (0.5f * noise.snoise(_noise) + 1.0f) * _maxJumpingHeight; 
            }

            // Smoothly transition _offset towards _targetOffset
            _offset = Mathf.Lerp(_offset, _targetOffset, Time.deltaTime * _transitionSpeed);

            // Calculate the new Y position
            float newY = modulate * _maxJumpingHeight + _offset;

            // newY = Mathf.Clamp(newY, -0.5f, 30.0f); 

            // Apply the sine wave to bodyPosition
            _bodyPosition.y = transform.position.y + newY;
        }


        void UpdateBodyRotation()
        {
            if (_newCycle)
            {
                // Generate a new random target rotation
                float randomYaw = UnityEngine.Random.Range(-360.0f, 360.0f); // Random yaw rotation

                Vector3 originalEuler = _bodyRotation.eulerAngles;
                float pitch = originalEuler.x;
                float roll = originalEuler.z;

                _targetRotation = Quaternion.Euler(pitch, randomYaw, roll);
            }
            

            // Smoothly interpolate the current rotation towards the target rotation
            _bodyRotation = Quaternion.Slerp(_bodyRotation, _targetRotation, 0.1f);
        }
        

        void UpdateFeet() {
            // TODO: 
        }

        #endregion

        static float bpm = 118f; 
        float beatInterval = 60.0f / bpm; // Duration of one beat
        float lastBeatTime = 0.0f; // Tracks the time of the last beat
        int beatCounter = 0; // Counter for the beats

        void Update()
        {
                // Noise update
                _noise.x += _noiseFrequency * Time.deltaTime;
                float modulate = Mathf.Sin(2 * Mathf.PI * Time.time * _frequency);

                
                float stepProgress = (Time.time % beatInterval) / beatInterval;

                if (Time.time - lastBeatTime >= beatInterval)
                {
                    lastBeatTime += beatInterval; // Update the last beat time
                    Debug.Log(beatCounter++); // Increment the beat counter
                    
                }
                

                if (modulate < -0.99f)
                {
                    if (!_newCycle) {
                        // Debug.Log(_debugCycle++); 
                        _newCycle = true; 
                    }
                }
                // Reset the flag once modulate goes above the threshold
                else if (modulate >= -0.999f)
                {
                    _newCycle = false; // Allow for the next cycle
                }

                UpdateBodyPosition(); 
                UpdateBodyRotation(); 
                // UpdateFeet(); 
        }

        void OnAnimatorIK(int layerIndex)
        {
            // update the feet
            _animator.SetIKPosition(AvatarIKGoal.LeftFoot, _feet[0]);
            _animator.SetIKPosition(AvatarIKGoal.RightFoot, _feet[1]);
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
            _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);

            _animator.SetIKHintPosition(AvatarIKHint.LeftKnee, _knees[0]); 
            _animator.SetIKHintPosition(AvatarIKHint.RightKnee, _knees[1]);
            
            _animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, 1); 
            _animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, 1); 

            // update the body position and rotation
            _animator.bodyPosition = _bodyPosition;
            _animator.bodyRotation = _bodyRotation;

            // update the rotation of the spine
            // var spine = SpineRotation;
            // _animator.SetBoneLocalRotation(HumanBodyBones.Spine, spine);
            // _animator.SetBoneLocalRotation(HumanBodyBones.Chest, spine);
            // _animator.SetBoneLocalRotation(HumanBodyBones.UpperChest, spine);

            // update the left and right arms
            // _animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandPosition);
            // _animator.SetIKPosition(AvatarIKGoal.RightHand, RightHandPosition);
            // _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            // _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);

            // update the head
            // _animator.SetLookAtPosition(LookAtPosition);
            // _animator.SetLookAtWeight(1);
        }
    }
}

using UnityEngine;
using Klak.Math;
using UnityEngine.Audio;
using Noise = Klak.Math.NoiseHelper;
using math = Unity.Mathematics;
using Unity.Mathematics;

namespace Puppet
{
    public class DancerShuffle : DancerBase
    {
        [SerializeField] public float _frequency = 1.2f;
        [SerializeField] private AudioSource audioSource = null;

        [SerializeField] private bool metronome = false; 

        [SerializeField] private int _seed = 123; 
        private int _currentSeed = 123; 

        private float _transitionSpeed = 5.0f;

        // body rotation vairables
        private Quaternion _bodyRotation;
        private Quaternion _targetRotation;

        // body position variables
        private Vector3 _bodyPosition;
        [SerializeField] private float _maxJumpingHeight = 0.5f;
        private float _bodyPosOffset = 0.0f;
        private float _bodyPosTargetOffset = 0.5f;

        // arm position variables
        private float _armPosTargetOffset = 0.5f;
        private Transform _currentTransform = null; 
        private Vector3[] _hands = new Vector3[2];

        // Foot and knee positions
        private Vector3[] _feet = new Vector3[2];
        private Vector3[] _knees = new Vector3[2];

        // head look position
        [SerializeField] private Vector3 _headLookAtPos; 
        private Vector3 _headPos;

        // spine variables
        private Quaternion _spine; 
        [SerializeField] private int _spineTwistToggle = 0; 

        Animator _animator;

        public override void initializeProperties()
        {
            base.initializeProperties();
            this.propInts["Noise Seed"] = new SetInt((x) => _seed = x, 0, 300, _seed); 
            this.propInts["Spine Twist Toggle"] = new SetInt((x) => _spineTwistToggle = x, 0, 1, _spineTwistToggle); 
            this.propFloats["Max Jumping Height"] = new SetFloat((x) => _maxJumpingHeight = x, 0.2f, 5.0f, _maxJumpingHeight);
        }

        void Start()
        {
            _animator = GetComponent<Animator>();
            _bodyPosition = transform.position;
            _bodyRotation = transform.rotation;
            _targetRotation = _bodyRotation;

            // Initialize feet and knee positions
            Vector3 origin = transform.position;
            Vector3 footOffset = transform.right * 0.2f;

            _feet[0] = origin - footOffset;
            _feet[1] = origin + footOffset;

            Vector3 kneeOffset = transform.forward * 0.5f;
            kneeOffset += transform.up * 0.5f; 
            _knees[0] = origin - footOffset + kneeOffset;
            _knees[1] = origin + footOffset + kneeOffset;

            _currentTransform = transform; 

            _headLookAtPos = new Vector3(0, 0.4f, 0.5f); 

            initializeProperties();
        }

        void Update()
        {
            if (_currentSeed != _seed) {
                _currentSeed = _seed; 
                UnityEngine.Random.InitState(_currentSeed); 
            }

            // Update position and rotation
            UpdateBodyPosition();
            UpdateBodyRotation();
            UpdateSpine(); 
            UpdateArmPosition(); 
            UpdateHeadPosition(); 

            // Play audio on beat
            if (metronome) PlayMetronomeOnBeat();

            // DrawRay(); 
            _currentTransform = transform; 
        }

        private void UpdateHeadPosition()
        {
            var modulate = Mathf.Sin(2 * Mathf.PI * beatManager.BeatTime + 0.5f);
            var pos = _headLookAtPos; 
            pos.y += modulate * 0.5f;
            
            _headPos = _bodyRotation * pos + _bodyPosition;
        }


        private void UpdateSpine()
        {
            var modulate = Mathf.Sin(2 * Mathf.PI * beatManager.BeatTime); // -1 to 1 modulation 
            if (_spineTwistToggle == 1) {
                _spine = Quaternion.AngleAxis(modulate * 20f, Vector3.right);
            }
            else 
            {
                _spine = Quaternion.AngleAxis(modulate * 20f, Vector3.forward);
            }
            //
            
        }


        private void UpdateArmPosition()
        {
            // index = 0 (left) else index = 1 (right)
            for (int index = 0; index < 2; ++index) {
                Vector3 handPosition = new Vector3(0.30f, 0.65f, 0.10f);
                var pos = handPosition; 
                if (index == 0) pos.x *= -1; 

                if (beatManager.NewCycle)
                {
                    // Generate a new target offset for the next jump height
                    _armPosTargetOffset = (0.5f * UnityEngine.Random.Range(-1.0f, 1.0f) + 1.0f);
                }

                //_bodyPosOffset = Mathf.Lerp(_armPosOffset, _armPosTargetOffset, Time.deltaTime * _transitionSpeed);

                var modulate = Mathf.Sin(2 * Mathf.PI * beatManager.BeatTime + 1.0f);
 
                pos.y += modulate * 0.2f;
                pos.z += Unity.Mathematics.math.remap(-1, 1, 0.6f, 0.0f, modulate); 
                _hands[index] = _bodyRotation * pos + _bodyPosition;

            }
        }

        private void UpdateBodyPosition()
        {
            if (beatManager.NewCycle)
            {
                // Generate a new target offset for the next jump height
                _bodyPosTargetOffset = (UnityEngine.Random.Range(0.5f, 1.5f)) * _maxJumpingHeight;
            }

            // Smoothly transition _offset towards _targetOffset
            _bodyPosOffset = Mathf.Lerp(_bodyPosOffset, _bodyPosTargetOffset, Time.deltaTime * _transitionSpeed);
            
            // Sine wave modulation based on the beat cycle
            var modulate = Mathf.Sin(2 * Mathf.PI * beatManager.BeatTime);

            // Calculate the new Y position based on sine wave modulation
            float newY = modulate * _maxJumpingHeight + _bodyPosOffset;

            // Apply the new Y position
            _bodyPosition.y = transform.position.y + 0.5f + newY;
        }

        /// <summary>
        /// Updates the body rotation with smooth transitions.
        /// </summary>
        private void UpdateBodyRotation()
        {
            if (beatManager.NewCycle)
            {
                // Generate a new random target rotation on each cycle
                float randomYaw = UnityEngine.Random.Range(-360.0f, 360.0f); // Random yaw rotation

                Vector3 currentEuler = _bodyRotation.eulerAngles;
                _targetRotation = Quaternion.Euler(currentEuler.x, randomYaw, currentEuler.z);
            }

            // Smoothly interpolate the current rotation towards the target rotation
            _bodyRotation = Quaternion.Slerp(_bodyRotation, _targetRotation, Time.deltaTime * _transitionSpeed);

            // re-update foot and knee positions!!!
            Vector3 origin = transform.position;
            Vector3 footOffset = _bodyRotation * transform.right * 0.2f;

            _feet[0] = origin - footOffset;
            _feet[1] = origin + footOffset;

            Vector3 kneeOffset = _bodyRotation * transform.forward * 0.5f;
            kneeOffset += _bodyRotation * transform.up * 0.5f; 
            _knees[0] = origin - footOffset + kneeOffset;
            _knees[1] = origin + footOffset + kneeOffset;
        }

        /// <summary>
        /// Plays a metronome sound on each beat.
        /// </summary>
        private void PlayMetronomeOnBeat()
        {
            if (beatManager.NewCycle && audioSource != null && audioSource.clip != null)
            {
                audioSource.Play(); // Play the metronome sound
            }
            else if (beatManager.NewCycle && (audioSource == null || audioSource.clip == null))
            {
                Debug.LogWarning("AudioSource or AudioClip is not assigned.");
            }
        }

        void OnDrawGizmos() {
            Gizmos.color = Color.blue; 
            Gizmos.DrawSphere(_knees[0], 0.1f); 
            Gizmos.DrawSphere(_knees[1], 0.1f); 

            Gizmos.color = Color.green; 
            Gizmos.DrawSphere(_hands[0], 0.1f); 
            Gizmos.DrawSphere(_hands[1], 0.1f); 

            Gizmos.color = Color.red; 
            Gizmos.DrawSphere(_headPos, 0.1f); 
        }


        void DrawRay() {
            // Define the origin of the ray (e.g., from the object's position)
            Vector3 origin = _feet[0];

            // Define the direction of the ray (e.g., forward from the object's perspective)
            Vector3 direction = -transform.up;

            // Ray length
            float rayLength = 10f;

            // Visualize the ray in the editor
            Debug.DrawRay(origin, direction * rayLength, Color.red);

            // Perform the raycast
            if (Physics.Raycast(origin, direction, out RaycastHit hitInfo, rayLength))
            {
                // Log what the ray hit
                //Debug.Log("Ray hit: " + hitInfo.collider.name);
            }
        }

        void OnAnimatorIK(int layerIndex)
        {
            // Update the body position and rotation
            _animator.bodyPosition = _bodyPosition;
            _animator.bodyRotation = _bodyRotation;

            // Update the feet positions
            _animator.SetIKPosition(AvatarIKGoal.LeftFoot, _feet[0]);
            _animator.SetIKPosition(AvatarIKGoal.RightFoot, _feet[1]);
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
            _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);

            // foot rotation is based on body rotation
            _animator.SetIKRotation(AvatarIKGoal.LeftFoot, _bodyRotation);
            _animator.SetIKRotation(AvatarIKGoal.RightFoot, _bodyRotation);
            _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1.0f);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1.0f);

            // Update the knee positions
            _animator.SetIKHintPosition(AvatarIKHint.LeftKnee, _knees[0]);
            _animator.SetIKHintPosition(AvatarIKHint.RightKnee, _knees[1]);
            _animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, 1);
            _animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, 1);

            // update hand position
            _animator.SetIKPosition(AvatarIKGoal.LeftHand, _hands[0]);
            _animator.SetIKPosition(AvatarIKGoal.RightHand, _hands[1]);
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);

            // update hand rotation
            _animator.SetBoneLocalRotation(HumanBodyBones.LeftHand, Quaternion.AngleAxis(-60, Vector3.up)); 
            _animator.SetBoneLocalRotation(HumanBodyBones.RightHand, Quaternion.AngleAxis(-60, Vector3.up)); 

            // update spine
            _animator.SetBoneLocalRotation(HumanBodyBones.Spine, _spine);
            _animator.SetBoneLocalRotation(HumanBodyBones.Chest, _spine);
            _animator.SetBoneLocalRotation(HumanBodyBones.UpperChest, _spine);

            // update head position
            _animator.SetLookAtPosition(_headPos); 
            _animator.SetLookAtWeight(1.0f); 
        }
    }
}

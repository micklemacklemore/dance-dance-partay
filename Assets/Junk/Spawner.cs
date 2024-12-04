using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class PrefabEntry
    {
        public GameObject prefab; // The prefab to spawn
        public float weight = 1.0f; // The weight for spawn probability
    }

    [SerializeField] PrefabEntry[] _prefabs; // Array of prefabs with weights
    [SerializeField] int _columns = 10;
    [SerializeField] int _rows = 10;
    [SerializeField] float _interval = 1;

    void Start()
    {
        // Calculate the total weight for probability calculation
        float totalWeight = 0f;
        foreach (var entry in _prefabs)
        {
            totalWeight += entry.weight;
        }

        // Get the spawner's position in the world
        Vector3 spawnerPosition = transform.position;

        for (var i = 0; i < _columns; i++)
        {
            var x = _interval * (i - _columns * 0.5f + 0.5f);

            for (var j = 0; j < _rows; j++)
            {
                var y = _interval * (j - _rows * 0.5f + 0.5f);

                // Add the spawner's position to offset the objects correctly
                var pos = spawnerPosition + new Vector3(x, 0, y);
                var rot = Quaternion.AngleAxis(Random.value * Mathf.PI, Vector2.up);

                // Pick a prefab randomly based on weights
                GameObject selectedPrefab = PickRandomPrefab(totalWeight);

                // Instantiate the selected prefab
                var go = Instantiate(selectedPrefab, pos, rot);

                // Example customization for the prefab
                var renderer = go.GetComponentInChildren<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Random.ColorHSV(0, 1, 0.6f, 0.8f, 0.8f, 1.0f);
                }

                // Handle custom member variables for different prefab types
                {
                    var dancer = go.GetComponent<Puppet.Dancer>();
                    if (dancer != null)
                    {
                        dancer.footDistance *= Random.Range(0.8f, 2.0f);
                        dancer.stepFrequency *= Random.Range(0.4f, 1.6f);
                        dancer.stepHeight *= Random.Range(0.75f, 1.25f);
                        dancer.stepAngle *= Random.Range(0.75f, 1.25f);

                        dancer.hipHeight *= Random.Range(0.75f, 1.25f);
                        dancer.hipPositionNoise *= Random.Range(0.75f, 1.25f);
                        dancer.hipRotationNoise *= Random.Range(0.75f, 1.25f);

                        dancer.spineBend = Random.Range(4.0f, -16.0f);
                        dancer.spineRotationNoise *= Random.Range(0.75f, 1.25f);

                        dancer.handPositionNoise *= Random.Range(0.5f, 2.0f);
                        dancer.handPosition += Random.insideUnitSphere * 0.25f;

                        dancer.headMove *= Random.Range(0.2f, 2.8f);
                        dancer.noiseFrequency *= Random.Range(0.4f, 1.8f);
                        dancer.randomSeed = (uint)Random.Range(0, 0xffffff);
                        continue; 
                    }
                }
                
                {
                    // DancerShuffle
                    var dancer = go.GetComponent<Puppet.DancerShuffle>();
                    if (dancer != null)
                    {
                        dancer._frequency = Random.Range(0.8f, 1.5f);
                        dancer._transitionSpeed = Random.Range(3.0f, 7.0f);
                        dancer._bodyTurnAmount = Random.Range(0f, 360f);
                        dancer._maxJumpingHeight = Random.Range(0.1f, 0.3f);
                        dancer._handModulateY = Random.Range(0.0f, 0.3f);
                        dancer._handModulateZ = Random.Range(0.0f, 0.1f);
                        dancer._headBend = Random.Range(0.0f, 1.0f);
                        dancer._spineBend = Random.Range(0.0f, 14.0f);
                        dancer._seed = Random.Range(0, 300);
                        dancer._bodyTurnRandom = Random.Range(0, 1); 
                        dancer._spineTwistToggle = Random.Range(0, 1); 
                        continue; 
                    }
                }

                {
                    var dancer = go.GetComponent<Puppet.DancerPose>();
                    if (dancer != null)
                    {
                        dancer._frequency = Random.Range(0.5f, 1.2f);
                        dancer._transitionSpeed = Random.Range(2.0f, 6.0f);
                        dancer._bodyTurnAmount = Random.Range(0f, 360f);
                        dancer._maxJumpingHeight = Random.Range(0.0f, 0.7f);
                        dancer._handModulateY = Random.Range(0.05f, 0.3f);
                        dancer._handModulateZ = Random.Range(0.1f, 0.5f);
                        dancer._headBend = Random.Range(0.1f, 0.7f);
                        dancer._spineBend = Random.Range(2.0f, 10.0f);
                        dancer._seed = Random.Range(0, 300);
                        dancer._bodyTurnRandom = Random.Range(0, 1); 
                        dancer._spineTwistToggle = Random.Range(0, 1); 
                        continue; 
                    }
                }
            }
        }
    }

    // Picks a random prefab based on weights
    private GameObject PickRandomPrefab(float totalWeight)
    {
        float randomValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var entry in _prefabs)
        {
            cumulativeWeight += entry.weight;
            if (randomValue <= cumulativeWeight)
            {
                return entry.prefab;
            }
        }

        // Fallback (shouldn't be reached if weights are properly calculated)
        return _prefabs[0].prefab;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoTiler : MonoBehaviour
{
        [SerializeField] GameObject _prefab = null;
        [SerializeField] int _columns = 10;
        [SerializeField] int _rows = 10;

    // Start is called before the first frame update
    void Start()
    {
        for (var i = -_columns / 2; i < _columns / 2; i++) {
            for (var j = -_rows / 2; j < _rows / 2; j++) {
                var pos = transform.position + new Vector3(1.20f * i, 0, 1.20f * j); 
                var rot = Quaternion.identity; 
                var go = Instantiate(_prefab, pos, rot);
                var render = go.GetComponentInChildren<Renderer>();
                if (render.material.HasProperty(Shader.PropertyToID("_Seed"))) {
                    render.material.SetFloat(Shader.PropertyToID("_Seed"), i + j); 
                }
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


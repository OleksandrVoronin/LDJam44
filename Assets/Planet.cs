using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField]
    private Transform _lightSource;

    private Material _material;

    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        float destinationRotation = Vector3.SignedAngle(Vector2.up, (_lightSource.transform.position - transform.position), Vector3.forward);
        _material.SetFloat("_LightAngle", 360 - destinationRotation - 45);
    }
}

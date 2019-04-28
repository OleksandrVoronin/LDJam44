using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField]
    private GameObject _toFollow;

    [SerializeField]
    private float _fallbackRatio = 2f;

    [SerializeField]
    private Vector4 _cameraBounds;

    // Update is called once per frame
    void Update()
    {
        transform.position += (_toFollow.transform.position - transform.position).NewZ(0) / _fallbackRatio;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, _cameraBounds.x, _cameraBounds.z), Mathf.Clamp(transform.position.y, _cameraBounds.y, _cameraBounds.w), transform.position.z);
    }
}

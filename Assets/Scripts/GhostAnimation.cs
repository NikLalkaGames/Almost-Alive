using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimation : MonoBehaviour
{
    public float _idlespeed;

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + _idlespeed / 1000, transform.position.z);
    }
}

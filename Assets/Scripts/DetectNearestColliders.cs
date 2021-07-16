using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DetectNearestColliders : MonoBehaviour
{
    protected List<Collider2D> _nearestColliders = new List<Collider2D>();

    public static event System.Action OnColliderDetectorEnter;
    public static event System.Action OnColliderDetectorExit;

    public List<Collider2D> NearestColliders => _nearestColliders;
    
    public List<Transform> GetListOfTriggerTransforms() => _nearestColliders.Select(x => x.transform).ToList();
}

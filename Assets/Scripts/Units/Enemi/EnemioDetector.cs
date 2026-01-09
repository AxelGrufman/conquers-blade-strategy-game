using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class EnemioDetector : MonoBehaviour
{
    public float detectionRadius = 15f;
    public LayerMask targetLayers;

    public readonly List<Transform> InRange = new List<Transform>();

    private SphereCollider _col;

    private void Awake()
    {
        _col = GetComponent<SphereCollider>();
        _col.isTrigger = true;
        _col.radius = detectionRadius;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayers) == 0) return;

        Transform t = other.transform;
        if (!InRange.Contains(t))
            InRange.Add(t);
    }

    private void OnTriggerExit(Collider other)
    {
        InRange.Remove(other.transform);
    }
}

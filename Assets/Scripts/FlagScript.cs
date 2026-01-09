using System.Collections.Generic;
using UnityEngine;

public class FlagScript : MonoBehaviour
{
    private int enemyCount;
    private int playerCount;
    private int allyCount;

    private bool conditionActive;

    public float startHeight;
    public float endHeight;

    [Min(0.01f)]
    public float secondsToReachEnd = 5f;  

    public GameObject Flag;
    private float currentHeight;

    public WinLooseScript WinLooseScript;

    public float radius = 0.5f;
    public LayerMask detectionMask = ~0;
    private QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Collide;

    private readonly Collider[] _hits = new Collider[128];
    private readonly Dictionary<int, byte> _inside = new Dictionary<int, byte>(128);

    private float unitsPerSecond;          
    private bool lostTriggered;            

    private void Start()
    {
        currentHeight = startHeight;
        RecalculateSpeed();

        if (Flag != null)
        {
            Vector3 pos = Flag.transform.position;
            pos.y = startHeight;
            Flag.transform.position = pos;
        }
    }

    private void RecalculateSpeed()
    {
        float distance = Mathf.Abs(endHeight - startHeight);
        unitsPerSecond = distance / Mathf.Max(0.01f, secondsToReachEnd);
    }

    private void Update()
    {
        if (!conditionActive)
            return;

        float newHeight = Mathf.MoveTowards(currentHeight, endHeight, unitsPerSecond * Time.deltaTime);
        currentHeight = newHeight;

        if (Flag != null)
        {
            Vector3 pos = Flag.transform.position;
            pos.y = currentHeight;
            Flag.transform.position = pos;
        }

        if (Mathf.Approximately(currentHeight, endHeight) && !lostTriggered)
        {
            lostTriggered = true;
            if (WinLooseScript != null)
                WinLooseScript.Loose();
        }
    }

    private void FixedUpdate()
    {
        Physics.SyncTransforms();

        int hitCount = Physics.OverlapSphereNonAlloc(
            transform.position,
            radius,
            _hits,
            detectionMask,
            triggerInteraction
        );

        var seenThisTick = new HashSet<int>();

        for (int i = 0; i < hitCount; i++)
        {
            Collider c = _hits[i];
            if (c == null) continue;

            GameObject go = c.attachedRigidbody ? c.attachedRigidbody.gameObject : c.gameObject;
            if (go == gameObject) continue;

            int id = go.GetInstanceID();
            if (!seenThisTick.Add(id))
                continue;

            if (!_inside.ContainsKey(id))
            {
                byte t = Classify(go);
                if (t != 0)
                {
                    _inside[id] = t;
                    Inc(t);
                }
            }
        }

        List<int> toRemove = null;
        foreach (var kv in _inside)
        {
            if (!seenThisTick.Contains(kv.Key))
                (toRemove ??= new List<int>()).Add(kv.Key);
        }

        if (toRemove != null)
        {
            foreach (int id in toRemove)
            {
                byte t = _inside[id];
                _inside.Remove(id);
                Dec(t);
            }
        }

        if (hitCount > 0 || toRemove != null)
            EvaluateCondition();
    }

    private byte Classify(GameObject go)
    {
        if (go.CompareTag("Enemy")) return 1;
        if (go.CompareTag("Player")) return 2;
        if (go.CompareTag("Ally")) return 3;
        return 0;
    }

    private void Inc(byte t)
    {
        if (t == 1) enemyCount++;
        else if (t == 2) playerCount++;
        else if (t == 3) allyCount++;
    }

    private void Dec(byte t)
    {
        if (t == 1) enemyCount = Mathf.Max(0, enemyCount - 1);
        else if (t == 2) playerCount = Mathf.Max(0, playerCount - 1);
        else if (t == 3) allyCount = Mathf.Max(0, allyCount - 1);
    }

    private void EvaluateCondition()
    {
        conditionActive =
            enemyCount > 0 &&
            playerCount == 0 &&
            allyCount == 0;
    }
}

using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DontDestroy_DuplicateCheck : MonoBehaviour
{
    [Tooltip("Unique identifier for this persistent object. If empty, the GameObject name is used.")]
    [SerializeField] private string instanceId;

    private static readonly Dictionary<string, DontDestroy_DuplicateCheck> Instances = new Dictionary<string, DontDestroy_DuplicateCheck>();
    private string runtimeId;

    private void Awake()
    {
        runtimeId = string.IsNullOrWhiteSpace(instanceId) ? gameObject.name : instanceId;

        if (Instances.TryGetValue(runtimeId, out var existingInstance))
        {
            if (existingInstance != this)
            {
                Debug.LogWarning($"DontDestroy_DuplicateCheck: Duplicate persistent object '{runtimeId}' destroyed.", this);
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            Instances[runtimeId] = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instances.TryGetValue(runtimeId, out var existingInstance) && existingInstance == this)
        {
            Instances.Remove(runtimeId);
        }
    }
}

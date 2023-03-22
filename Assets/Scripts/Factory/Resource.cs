using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private ResourceType _resourceType;

    public ResourceType ResourceType { get => _resourceType; }
}

using UnityEngine;

namespace ScriptableObjects.Resources
{
   
    [CreateAssetMenu(menuName = "Resources/Resource")]
    public class ResourceBase : ScriptableObject
    {
        [SerializeField] private ResourceType _type;
        [SerializeField] private Transform _resourcePrefab;

        public ResourceType Type { get => _type; set => _type = value; }
        public Transform ResourcePrefab { get => _resourcePrefab; set => _resourcePrefab = value; }
    }

}
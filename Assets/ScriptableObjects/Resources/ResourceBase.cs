using UnityEngine;

namespace ScriptableObjects.Resources
{
   
    [CreateAssetMenu(menuName = "Resources/Resource")]
    public class ResourceBase : ScriptableObject
    {
        [SerializeField] private ResourceType _type;
        [SerializeField] private Resource _resourcePrefab;

        public ResourceType Type { get => _type; }
        public Resource ResourcePrefab { get => _resourcePrefab; }
    }

}
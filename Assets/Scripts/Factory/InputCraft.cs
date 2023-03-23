using System.Collections.Generic;
using UnityEngine;

namespace Factory
{
    public class InputCraft : MonoBehaviour
    {
        [SerializeField]private List<ResourceType> _resourcesForProduction;

        public List<ResourceType> ResourcesForProduction { get => _resourcesForProduction;}
    }
}
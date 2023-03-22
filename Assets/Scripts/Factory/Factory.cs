using System.Collections;
using ScriptableObjects.Resources;
using UnityEngine;

namespace Factory
{
    public abstract class Factory : MonoBehaviour
    {
        [SerializeField] protected ResourceBase _resource;
        [SerializeField] protected OutStorage _outStorage;
        public abstract IEnumerator ProduceResource();

    }
}

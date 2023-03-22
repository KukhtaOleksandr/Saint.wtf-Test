using System.Collections;
using UnityEngine;

namespace Factory
{
    public class SelfProducingFactory : Factory
    {
        private bool _isObjectAlive;

        private void Start()
        {
            _isObjectAlive = true;
            StartCoroutine("ProduceResource");
        }
        private void OnDestroy()
        {
            _isObjectAlive = false;
        }
        public override IEnumerator ProduceResource()
        {
            while (_isObjectAlive)
            {
                yield return new WaitForSeconds(1);
                GameObject.Instantiate(_resource.ResourcePrefab, _outStorage.GetNextCell(), Quaternion.Euler(0, 0, -90));
            }

        }
    }
}

using System.Collections;
using UnityEngine;

namespace Factory
{
    public class ProducingFactory : Factory
    {
        [SerializeField] private InStorage _inStorage;
        public override IEnumerator ProduceResource()
        {
            yield return new WaitForSeconds(1);
        }
    }
}

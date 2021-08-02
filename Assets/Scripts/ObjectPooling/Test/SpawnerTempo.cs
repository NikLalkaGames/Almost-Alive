using UnityEngine;

namespace ObjectPooling.Test
{
    public class SpawnerTempo : MonoBehaviour
    {
        public string objectType;

        private void Start()
        {
            PoolManager.Instance.SpawnFromPool(objectType, transform.position);
            Destroy(this.gameObject);
        }
    }
}
using UnityEngine;

namespace ObjectPooling.Test
{
    public class SpawnerTempo : MonoBehaviour
    {
        public float timeToSpawn;
        private float _timer;

        private void Update()
        {
            _timer -= Time.deltaTime;
            
            if (_timer < 0)
            {
                _timer = timeToSpawn;
                PoolManager.Instance.TakeAndPlace("Cube", transform.position);
            }
        }
    }
}
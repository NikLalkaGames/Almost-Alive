using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { get; private set; } = null;
        
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int initialSize;
        }

        [SerializeField] private List<Pool> definedPools;
        private Dictionary<string, Queue<GameObject>> _poolDictionary;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            
            _poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (var pool in definedPools)
            {
                var objectPool = new Queue<GameObject>();
                
                for (var i = 0; i < pool.initialSize; i++)
                {
                    var objectToPool = Instantiate(pool.prefab, transform);     // instantiate as child
                    objectToPool.SetActive(false);
                    objectPool.Enqueue(objectToPool);
                }
                
                _poolDictionary.Add(pool.tag, objectPool);
            }
        }
        
        public GameObject TakeAndPlace(string objectType, Vector2 positionToSpawn, Quaternion quaternion = default)
        {
            if (!_poolDictionary.ContainsKey(objectType))
            {
                Debug.LogError("Object pool with such key does not exist");
            }
            
            var objectToSpawn = _poolDictionary[objectType].Dequeue();
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = positionToSpawn;
            objectToSpawn.transform.rotation = quaternion;

            return objectToSpawn;
        }

        public void Return(string objectType, GameObject objectToReturn)
        {
            if (!_poolDictionary.ContainsKey(objectType))
            {
                Debug.LogError("Object pool with such key does not exist");
            }
            
            objectToReturn.SetActive(false);
            _poolDictionary[objectType].Enqueue(objectToReturn);
        }
        
    }
}

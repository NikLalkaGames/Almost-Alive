using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Serialization;

namespace ObjectPooling
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { get; private set; } = null;
        
        [System.Serializable]
        public class Pool
        {
            public string objectType;
            public GameObject prefab;
            public int initialSize;
        }

        [SerializeField] private List<Pool> definedPools;
        private Dictionary<string, Queue<GameObject>> _poolDictionary;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            
            _poolDictionary = new Dictionary<string, Queue<GameObject>>();
            
            // define names for object pools
            foreach (var pool in definedPools)
            {
                pool.objectType = pool.prefab.name;
            }
            
            foreach (var pool in definedPools)
            {
                var objectPool = new Queue<GameObject>();
                
                for (var i = 0; i < pool.initialSize; i++)
                {
                    var objectToPool = Instantiate(pool.prefab, transform);     // instantiate as child
                    objectToPool.SetActive(false);
                    objectPool.Enqueue(objectToPool);
                }
                
                _poolDictionary.Add(pool.objectType, objectPool);
            }
        }

        public void WarmPool(string objectType, GameObject prefab, int size)
        {
            var objectPool = new Queue<GameObject>();
                
            for (var i = 0; i < size; i++)
            {
                var objectToPool = Instantiate(prefab, transform);     // instantiate as child
                objectToPool.SetActive(false);
                objectPool.Enqueue(objectToPool);
            }
                
            _poolDictionary.Add(objectType, objectPool);
        }

        public void WarmPool(Pool pool)
        {
            var objectPool = new Queue<GameObject>();
                
            for (var i = 0; i < pool.initialSize; i++)
            {
                var objectToPool = Instantiate(pool.prefab, transform);     // instantiate as child
                objectToPool.SetActive(false);
                objectPool.Enqueue(objectToPool);
            }
                
            _poolDictionary.Add(pool.objectType, objectPool);
        }

        public GameObject SpawnFromPool(string objectType, Vector2 positionToSpawn)
        {
            if (!_poolDictionary.ContainsKey(objectType))
            {
                Debug.LogError("Object pool with such key does not exist");
            }
            
            var objectToSpawn = _poolDictionary[objectType].Dequeue();
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = positionToSpawn;

            return objectToSpawn;
        }

        public void ReturnToPool(string objectType, GameObject objectToReturn)
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

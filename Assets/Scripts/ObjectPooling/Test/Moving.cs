using UnityEngine;

namespace ObjectPooling.Test
{
    public class Moving : MonoBehaviour
    {
        public string category;
        private Vector3 _startPos;
        
        
        private void Start()
        {
            _startPos = transform.position;
        }

        private void Update()
        {
            transform.Translate(Vector3.right * 2 * Time.deltaTime);
            
            if (Vector3.Distance(transform.position, _startPos) > 10f)
            {
                PoolManager.Instance.ReturnToPool(category, this.gameObject);
            }
            
        }
    }
}
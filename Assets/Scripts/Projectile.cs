using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private Vector3 direction;
    public float speed;
    private Vector2 projectileDistance;

    private Vector2 positionStart;
    private Vector3 positionEnd;

    private float distance = 0f;
    private float t = 0f;
    private Vector3 yVelocity = Vector3.zero;


    void Start()
    {
        positionStart = transform.position;
        
    }

    void Update()
    {
        /* transform.position = Vector2.Lerp(positionStart,positionEnd, distance);
        if (distance)
        distance += Time.deltaTime * 0.5f; */
    /*     transform.position = Vector2.Lerp(positionStart, positionEnd, distance);
        if(distance >= 0.8)
        {
            distance = Mathf.SmoothDamp(distance, 1f, ref yVelocity, 1f, 1f);
        }
        distance += Time.deltaTime; */

        /* transform.position = Vector3.SmoothDamp(transform.position, positionEnd, ref yVelocity, 0.5f, speed); */

        var step = speed * Time.fixedDeltaTime;
        transform.position += direction * step;


    }



    public void SetDirection(Vector2 internalDirection)
    {
        direction = internalDirection; 
    }

    public void SetTarget(Vector2 mouseTarget)
    {
        positionEnd = mouseTarget;
    }



        private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Consumable")
        {
            Debug.Log("KILL?");
            other.GetComponent<ConsumableBehaviour>().Kill();
            Destroy(this.gameObject);
        }
    }

}

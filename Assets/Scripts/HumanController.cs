using System.Collections;
using System.Collections.Generic;
using GhostBehaviours;
using UnityEngine;

public class HumanController : MonoBehaviour
{

    public float speed = 0.8f;
    public float retreatDistance = 1;
    public float retreatRunDistance = 4;


    float x, y, t, step;
    // Start is called before the first frame update
    void Start()
    {
        x = 0;
        y = 0;
        t = 0;
        step = 0.005f;
    }

    // Update is called once per frame
    void Update()
    {

        float distance = Vector2.Distance(GhostMovement.Instance.transform.position, transform.position);

        if (distance > retreatRunDistance + 7)
        {
            transform.position = Vector2.MoveTowards(transform.position, transform.position, 0);

        }
        else if (distance > retreatRunDistance)
        {
            float tempX = x;
            float tempY = y;
            x = (2 * Mathf.Cos(t) + Mathf.Cos(2 * t)) / 5;
            y = (2 * Mathf.Sin(t) - Mathf.Sin(2 * t)) / 5;
            Vector3 direction;
            direction.x = x + tempX;
            direction.y = y + tempY;
            direction.z = this.transform.position.z;
            transform.position += (direction * Time.deltaTime);

            t += step * Mathf.PI;
            if ((t > 2 * Mathf.PI) || (t < 0))
            {
                step = -step;
            }
        }
        else if (distance > retreatDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, GhostMovement.Instance.transform.position, -speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, GhostMovement.Instance.transform.position, -0.7f * speed * Time.deltaTime);
        }

    }

}

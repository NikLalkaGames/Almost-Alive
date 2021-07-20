using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IColliderDetector
{
    void OnTriggerEnter2D(Collider2D other);
    void OnTriggerExit2D(Collider2D other);
}

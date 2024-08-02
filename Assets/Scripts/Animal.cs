using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public void MoveTo(Vector2 targetPosition)
    {
        gameObject.transform.position = targetPosition;
    }

    public void EnablePhysics()
    {
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
}

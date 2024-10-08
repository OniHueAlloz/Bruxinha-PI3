using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Throwable";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")||collision.gameObject.CompareTag("Liftable") || collision.gameObject.CompareTag("Throwable"))
        {
            gameObject.tag = "Throwable";
        }
    }
}

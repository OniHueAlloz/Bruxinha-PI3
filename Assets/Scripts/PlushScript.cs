using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlushScript : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 150f;
    [SerializeField] private float floatDistance = 0.3f;
    [SerializeField] private float floatSpeed = 2f;

    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        float newHeight = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatDistance;
        transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
    }
}

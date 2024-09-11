using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -10);

    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
         GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Não tem Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void LateUpdate()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + offset;
        }
    }
}

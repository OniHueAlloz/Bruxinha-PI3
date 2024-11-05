using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    [SerializeField] private float groundCheckRadius = 0.5f;
    [SerializeField] private float groundCheckDistance = 0.6f;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Throwable";
    }

    // Update is called once per frame
    void Update()
    {
        if(CheckIfGrounded())
        {
            gameObject.tag = "Throwable";
        }
    }

    private bool CheckIfGrounded()
    {
        int groundLayer = 1 << LayerMask.NameToLayer("Default");
        return Physics.SphereCast(transform.position, groundCheckRadius, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer);
    }

    //debug 
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * groundCheckDistance, groundCheckRadius);
    }
}

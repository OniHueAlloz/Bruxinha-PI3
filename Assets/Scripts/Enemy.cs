using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class Enemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float patrolSpeed = 5f;
    private Vector3 currentTarget; 

    [Header("Detection Settings")]
    private bool isAggressive = false;
    private Transform player;

    [Header("Combat Settings")]
    //[SerializeField] private int energy = 1;
    [SerializeField] private float stunDuration = 5f;
    private bool isStunned = false;
    private float stunTimer = 0f;

    [Header("Sound Settings")]

    [SerializeField] private AudioClip crySound;
    [SerializeField] private AudioClip stepSound;
    private AudioSource audioSource;

    enum State {Patrol, Pursue, Stun}
    State state;

    // Start is called before the first frame update
    void Start()
    {
        //fixa a posição vertical e define o destino como a origem
        currentTarget = pointA.position;
        state = State.Patrol;

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1.0f;
        audioSource.loop = false;
        audioSource.minDistance = 495f;
        audioSource.maxDistance = 500f;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.Stun: StartStun(); break;
            case State.Patrol: Patrol(); break;
            case State.Pursue: PursuePlayer(); break;
        }
    }

    private void Patrol()
    {
        //move o inimigo para o ponto definido
        Vector3 targetPosition = new Vector3(currentTarget.x, currentTarget.y, currentTarget.z);
        transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y, transform.position.z), targetPosition, patrolSpeed * Time.deltaTime);

        //checa se o inimigo alcançou o destino
        if (Vector3.Distance(new Vector3(transform.position.x, transform.position.y, transform.position.z), targetPosition) < 0.1f)
        {
            //muda o destino de A para B e vice-versa
            currentTarget = currentTarget == pointA.position ? pointB.position : pointA.position;
        }

        //roda o inimigo
        Vector3 direction = new Vector3(currentTarget.x, currentTarget.y, currentTarget.z) - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * patrolSpeed);
        }

        //transições
        if(isStunned)
        {
            state = State.Stun;
        }
        else if(isAggressive)
        {
            audioSource.PlayOneShot(crySound);
            Debug.Log("Enemy cry played"); 
            state = State.Pursue;
        }
    }

    private void PursuePlayer()
    {
        //garante que conseguiu pegar a localização do player
        if (player != null)
        {
            //segue o player
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * patrolSpeed * Time.deltaTime;
            //gira na direção do player
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * patrolSpeed);
        }

        //transições
        if(isStunned)
        {
            state = State.Stun;
        }
    }

    private void StartStun()
    {
        //reinicializa o timer
        if (stunTimer <= 0)
        {
            stunTimer = stunDuration;
        }

        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0)
        {   
            //finaliza o atordoamento
            isStunned = false;

            //transições
            state = State.Patrol;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Thrown"))
        {
            //quando atingido por um objeto arremessado, fica atordoado
            isStunned = true;
            isAggressive = false;

            audioSource.PlayOneShot(crySound);
            Debug.Log("Enemy cry played"); 

            state = State.Stun;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isAggressive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isAggressive = false;
        }
    }

    /* private void Die()
    {
        //desativa o rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }
        //desativa o collider
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        //Aqui toca a animação de morte
        //if(animação terminou)
        //{
        gameObject.SetActive(false);
        //}
    }*/

}

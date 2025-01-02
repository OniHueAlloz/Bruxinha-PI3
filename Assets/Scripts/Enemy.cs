using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    enum State {Patrol, Seek}
    private State currentState = State.Patrol;

    [Header("Movement Settings")]
    private NavMeshAgent enemy;
    [SerializeField] private Transform pointA = null;
    [SerializeField] private Transform pointB = null;
    [SerializeField] private Transform player = null;
    [SerializeField] private float patrolSpeed = 5f;
    [SerializeField] private float chaseSpeed = 8f;
    [SerializeField] private float detectionRange = 10f;

    private Transform currentTarget;
    private Rigidbody rb;

    [Header("Combat Settings")]
    //[SerializeField] private int energy = 1;
    [SerializeField] private float stunDuration = 5f;
    private bool isStunned = false;
    private float stunTimer = 0f;

    [Header("Sound Settings")]
    public AudioClip crySound;
    public AudioClip stepSound;
    public AudioSource audioSource;

    [Header("Aniamtion Settings")]
    public Animator animator;


    // Start is called before the first frame update
    void Awake()
    {
        enemy = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        currentTarget = pointA;
    }

    void Start()
    {
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1.0f;
        audioSource.loop = false;
        audioSource.minDistance = 495f;
        audioSource.maxDistance = 500f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStunned)
        {
            StunBehaviour();
            return;
        }

        switch(currentState)
        {
            case State.Patrol:
                PatrolBehaviour();
                break;
            
            case State.Seek:
                SeekBehaviour();
                break;
            
            default:
                break;
        }
    }

    void PatrolBehaviour()
    {
        enemy.speed = patrolSpeed;
        enemy.SetDestination(currentTarget.position);

        animator.SetInteger("idAnimation", 1);

        if (Vector3.Distance(transform.position, currentTarget.position) < 1f)
        {
            Transform nextTarget = currentTarget == pointA ? pointB : pointA;

            Vector3 direction = (nextTarget.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            currentTarget = nextTarget;
        }

        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            PlayCrySound();
            currentState = State.Seek;
            currentTarget = player;
        }
    }

    void SeekBehaviour()
    {
        enemy.speed = chaseSpeed;
        enemy.SetDestination(player.position);

        animator.SetInteger("idAnimation", 2);

        if (Vector3.Distance(transform.position, player.position) > detectionRange)
        {
            currentState = State.Patrol;
            currentTarget = pointA;
        }
    }

    void StunBehaviour()
    {
        stunTimer += Time.deltaTime;

        if(stunTimer >= stunDuration)
        {
            isStunned = false;
            stunTimer = 0f;
            currentState = State.Patrol;
            currentTarget = pointA;
            rb.constraints = (RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY);
            enemy.isStopped = false;

            animator.SetTrigger("EndStun");
        }
    }

    public void GetStunned()
    {
        isStunned = true;
        stunTimer = 0f;
        enemy.isStopped = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        enemy.ResetPath();

        animator.SetTrigger("Stun");
        PlayCrySound();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Thrown"))
        {
            GetStunned();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    public void PlayStepSound()
    {
        audioSource.PlayOneShot(stepSound);
        Debug.Log("Enemy step played"); 
    }

    public void PlayCrySound()
    {
        audioSource.PlayOneShot(crySound);
        Debug.Log("Enemy cry played"); 
    }

}

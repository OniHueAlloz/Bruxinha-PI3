using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float patrolSpeed = 5f;
    private Vector3 currentTarget; 
    private float yPosition;

    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 5f;
    private bool isAggressive = false;
    private Transform player;
    private float timeSincePlayer = 0f;
    [SerializeField] private float loseAggroTime = 2f;

    [Header("Combat Settings")]
    [SerializeField] private int energy = 1;

    // Start is called before the first frame update
    void Start()
    {
        //fixa a posição vertical e define o destino como a origem
        yPosition = transform.position.y;

        currentTarget = pointA.position;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
        }
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //verifica se encontra o player
        if (DetectPlayer())
        {
            isAggressive = true;
            //zera a contagem de tempo longe do player
            timeSincePlayer = 0f;
            //faz seguir o player
            PursuePlayer();
        }
        else
        {
            //atualiza o tempo
            timeSincePlayer += Time.deltaTime;

            //compara o tempo com o limite
            if (timeSincePlayer >= loseAggroTime)
            {
                isAggressive = false;
            }
        }

        if (!isAggressive)
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        //move o inimigo para o ponto definido
        Vector3 targetPosition = new Vector3(currentTarget.x, yPosition, currentTarget.z);
        transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, yPosition, transform.position.z), targetPosition, patrolSpeed * Time.deltaTime);

        //checa se o inimigo alcançou o destino
        if (Vector3.Distance(new Vector3(transform.position.x, yPosition, transform.position.z), targetPosition) < 0.1f)
        {
            //muda o destino de A para B e vice-versa
            currentTarget = currentTarget == pointA.position ? pointB.position : pointA.position;
        }

        //roda o inimigo
        Vector3 direction = new Vector3(currentTarget.x, 0, currentTarget.z) - new Vector3(transform.position.x, 0, transform.position.z);
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * patrolSpeed);
        }
    }

    private bool DetectPlayer()
    {
        //cria o raycast e define a origem
        RaycastHit hit;
        Vector3 origin = transform.position;

        //procura o player
        if (Physics.SphereCast(origin, detectionRadius, transform.forward, out hit, 2*detectionRadius))
        {
            if (hit.collider.CompareTag("Player"))
            {
                //achou o player, pega a localização dele
                player = hit.transform;
                return true;
            }
        }
        //não achou o player
        player = null;
        return false;
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Thrown"))
        {
            //se um objeto jogado acertar o inimigo, reduz a vida
            energy--;
            //se zerar a vida, morre
            if (energy <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
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
    }

}

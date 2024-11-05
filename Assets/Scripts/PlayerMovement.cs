using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Generals")]
    private Rigidbody PlayerRb;
    [SerializeField] private int energy = 3;
    [SerializeField] private int life = 2;

    [Header("Movement Settings")]  
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float runSpeed = 20f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private float groundCheckRadius = 0.5f;
    [SerializeField] private float groundCheckDistance = 0.6f;
    [SerializeField] private float glideFallSpeed = 1f;

    [Header("Interaction Settings")]
    [SerializeField] private float rayDistance = 5f;
    [SerializeField] private float heightCorrection = 0.5f;
    [SerializeField] private float throwForce = 15f;
    [SerializeField] private Vector3 holdPositionOffset = new Vector3 (0, 1, 2);
    private GameObject holdPosition;
    private GameObject liftedObject;
    private bool isHolding = false;
    private bool isThrowable = false;
    private bool isLiftable = false;
    private int coinCount = 0;
    private bool gotPlush = false;
    private bool gotObjective = false;

    // Start is called before the first frame update
    void Start()
    {
        //Associando o Rigidbody
        PlayerRb = GetComponent<Rigidbody>();

        //Criando um objeto vazio e colocando ele numa posição relativa ao player com offset, para usar como referência depois
        holdPosition = new GameObject("HoldPosition");
        holdPosition.transform.SetParent(transform);
        holdPosition.transform.localPosition = holdPositionOffset;
    }

    // Update is called once per frame
    void Update()
    {
        //Chamando os métodos de comandos
        MovementCommands();
        JumpCommands();
        if (Input.GetKeyDown(KeyCode.E)) DetectLiftableObject();
    }

    private void MovementCommands()
    {
        //pegando o input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0, moveZ).normalized;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            //corre se apertar shift
            PlayerRb.MovePosition(transform.position + movement * runSpeed * Time.deltaTime);

            //roda
            if (movement.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, runSpeed * Time.deltaTime);
            }
            
        }
        else 
        {
            //se não apertar shift anda
            PlayerRb.MovePosition(transform.position + movement * walkSpeed * Time.deltaTime);


            //roda
            if (movement.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, walkSpeed * Time.deltaTime);
            }
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

    private void JumpCommands()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            //pula se estiver no chão
            PlayerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else if (!isGrounded && Input.GetButton("Jump"))
        {
            if (PlayerRb.velocity.y < 0)
            {
                //pula se estiver fora do chão (o if ali em cima garante que a redução de velocidade do glide só afeta a descida e não a subida)
                PlayerRb.velocity = new Vector3(PlayerRb.velocity.x, -glideFallSpeed, PlayerRb.velocity.z);
            }
        }
    }

    private void DetectLiftableObject()
    {
        if (isHolding)
        {
            if (isLiftable)
            {
                isLiftable = false;
                DropObject();
            }
            else if (isThrowable)
            {
                isThrowable = false;
                ThrowObject();
            }
            
        }
        else
        {
            //se não, a gente vai procurar um objeto pra segurar
            RaycastHit hit;
            Vector3 rayDirection = transform.forward;
            Vector3 rayOrigin = transform.position + Vector3.up * heightCorrection;

            if (Physics.Raycast(rayOrigin, transform.forward, out hit, rayDistance))
            {
                //mostra o raycast para debug
                Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red, 2f);

                if (hit.collider.CompareTag("Liftable"))
                {
                    //uma vez que o objeto foi encontrado, a gente chama o método que vai levantar ele
                    isLiftable = true;
                    LiftObject(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("Throwable"))
                {
                    //uma vez que o objeto foi encontrado, a gente chama o método que vai levantar ele
                    isThrowable = true;
                    LiftObject(hit.collider.gameObject);
                }
            }
        }
    }

    private void LiftObject(GameObject obj)
    {
        //associa, avisa que ta segurando e pega o rigidbody do objeto segurado
        liftedObject = obj;
        isHolding = true;

        Rigidbody objRb = liftedObject.GetComponent<Rigidbody>();

        //altera ele pra evitar problemas
        if (objRb != null)
        {
            objRb.useGravity = false;
            objRb.isKinematic = true;
        }

        //coloca o objeto na posição de levantado e garante que se moverá com o player
        liftedObject.transform.position = holdPosition.transform.position;
        liftedObject.transform.SetParent(holdPosition.transform);
    }

    private void ThrowObject()
    {
        if (liftedObject != null)
        {
            //desassocia o objeto levantado do objeto pai e pega seu rigidbody
            liftedObject.transform.SetParent(null);

            Rigidbody objRb = liftedObject.GetComponent<Rigidbody>();

            if (objRb != null)
            {
                //desfaz as mudanças e dispara o objeto para a frente
                objRb.useGravity = true;
                objRb.isKinematic = false;
                liftedObject.tag = "Thrown";
                objRb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
            }

            //garante que a variável possa ser reutilizada
            liftedObject = null;
            isHolding = false;
        }
    }

    private void DropObject()
    {
        if (liftedObject != null)
        {
            //objeto levantado deixa de ser filho
            liftedObject.transform.SetParent(null);
        

            Rigidbody objRb = liftedObject.GetComponent<Rigidbody>();
            if (objRb != null)
            {
                //reativa a física do objeto
                objRb.useGravity = true;
                objRb.isKinematic = false;
            }

            //reseta os bools
            liftedObject = null;
            isHolding = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // verifica que o jogador tocou o chão
        if (collision.gameObject.tag == "Enemy")
        {
            energy--;
            if (energy <= 0)
            {
                life--;
                if (life < 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            } 
        }

        isGrounded = CheckIfGrounded();
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = CheckIfGrounded();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            coinCount++;
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.tag == "Plush")
        {
            gotPlush = true;
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.tag == "Objective")
        {
            gotObjective = true;
            other.gameObject.SetActive(false);
        }
    }
}

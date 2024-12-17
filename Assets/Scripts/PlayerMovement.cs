using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Generals")]
    [SerializeField] public static int maxEnergy = 5;
    //Informações de Interface
    [SerializeField] public static int energy = 5;
    [SerializeField] public static int life = 3;
    private Rigidbody PlayerRb;

    [Header("Movement Settings")]  
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float runSpeed = 20f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private bool isGrounded = false;
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
    private Vector3 initialPosition;

    //Pedido Concluído:
    public static int coinCount = 0;
    public static bool gotPlush = false;
    public static bool gotObjective = false;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip glideSound;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip liftSound;
    [SerializeField] private AudioClip throwSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip floatSound;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //Associando o Rigidbody
        PlayerRb = GetComponent<Rigidbody>();
        initialPosition = transform.position;

        //Criando um objeto vazio e colocando ele numa posição relativa ao player com offset, para usar como referência depois
        holdPosition = new GameObject("HoldPosition");
        holdPosition.transform.SetParent(transform);
        holdPosition.transform.localPosition = holdPositionOffset;

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
        //Chamando os métodos de comandos
        isGrounded = CheckIfGrounded();
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
            isGrounded = false;
            audioSource.PlayOneShot(jumpSound);
        }
        else if (!isGrounded && Input.GetButton("Jump"))
        {
            if (PlayerRb.velocity.y < 0)
            {
                //pula se estiver fora do chão (o if ali em cima garante que a redução de velocidade do glide só afeta a descida e não a subida)
                PlayerRb.velocity = new Vector3(PlayerRb.velocity.x, -glideFallSpeed, PlayerRb.velocity.z);
                audioSource.PlayOneShot(glideSound);
                Debug.Log("glideSound played"); 
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
                else if (hit.collider.CompareTag("Throwable") || hit.collider.CompareTag("Thrown"))
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

        audioSource.PlayOneShot(liftSound);
        Debug.Log("liftSound played");

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
                audioSource.PlayOneShot(throwSound);
                Debug.Log("throwSound played");
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
            audioSource.PlayOneShot(liftSound);
            Debug.Log("liftSound played");

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

            audioSource.PlayOneShot(damageSound);
            Debug.Log("damageSound played"); 

            if (energy <= 0)
            {
                life--;
                if (life < 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            } 
        }
    }

    /*private void OnCollisionStay(Collision collision)
    {
        isGrounded = CheckIfGrounded();
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = CheckIfGrounded();
    } */

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            coinCount++;
        }
        else if (other.gameObject.tag == "Wing")
        {
            life++;
        }
        else if (other.gameObject.tag == "Radish")
        {
            if (energy == maxEnergy)
            {
                coinCount += 10;
            }
            else if (energy == (maxEnergy-1))
            {
                energy = maxEnergy;
            }
            else
            {
                energy += 2;
            }
        }
        else if (other.gameObject.tag == "Plush")
        {
            gotPlush = true;
        }
        else if (other.gameObject.tag == "Objective")
        {
            gotObjective = true;
        }
        else if (other.gameObject.tag == "Death")
        {
            life--;
            if (life < 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                transform.position = initialPosition;
                PlayerRb.velocity = Vector3.zero;
            }
        }
    }

    public void PlayFloatSound()
    {
        audioSource.PlayOneShot(floatSound);
        Debug.Log("floatSound played"); 
    }
}

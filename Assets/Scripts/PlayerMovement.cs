using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]  
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float glideFallSpeed = 1f;

    [Header("Interaction Settings")]
    [SerializeField] private float rayDistance = 4f;
    [SerializeField] private float throwForce = 15f;
    [SerializeField] private Vector3 holdPositionOffset = new Vector3 (0, 1, 2);
    private GameObject holdPosition;
    private GameObject liftedObject;
    private bool isHolding = false;

    [Header("Player Generals")]
    public Rigidbody PlayerRb;

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

        Vector3 movement = new Vector3(moveX, 0, moveZ);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            //corre se apertar shift
            PlayerRb.MovePosition(transform.position + movement * runSpeed * Time.deltaTime);
        }
        else 
        {
            //se não apertar shift anda
            PlayerRb.MovePosition(transform.position + movement * walkSpeed * Time.deltaTime);
        }
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
            //se um objeto estiver sendo segurado, ele será arremessado
            ThrowObject();
        }
        else
        {
            //se não, a gente vai procurar um objeto pra segurar
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance))
            {
                if (hit.collider.CompareTag("Liftable"))
                {
                    //uma vez que o objeto foi encontrado, a gente chama o método que vai levantar ele
                    LiftObject(hit.collider.gameObject);
                }
            }
        }
    }

    private void LiftObject(GameObject obj)
    {
        liftedObject = obj;
        isHolding = true;

        Rigidbody objRb = liftedObject.GetComponent<Rigidbody>();

        if (objRb != null)
        {
            objRb.useGravity = false;
            objRb.isKinematic = true;
        }
    }

    private void OnCollisionEnter (Collision collision)
    {
        //checa se tocou o chão
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true; 
        }
    }

    private void OnCollisionExit (Collision collision)
    {
        //checa se saiu do chão
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false; 
        }
    }
}

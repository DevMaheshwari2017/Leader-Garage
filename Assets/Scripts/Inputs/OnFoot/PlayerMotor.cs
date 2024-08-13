using UnityEngine;
using UnityEngine.AI;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private string walkableAreaTag = "Ground";

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
    }

    public void ProcessMovement(Vector2 input) 
    {

        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        //for horizontal movement

        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        if (moveDirection.z > 0)
        {
            animator.SetFloat("Speed", 0.5f);
        }
        else if (moveDirection.z < 0)
        {
            animator.SetFloat("Speed", -0.5f);
        }
        else if (moveDirection.x < 0)
        {
            animator.SetFloat("Speed", 1.5f);
        }
        else if (moveDirection.x > 0)
        {
            animator.SetFloat("Speed", 2.5f);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
        playerVelocity.y += gravity * Time.deltaTime;

        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;

        //for vertical movement
        controller.Move(playerVelocity * Time.deltaTime);
    }

    //sets the playervelocity for jumpibng action - controller move the player verticaly 
    public void Jump() 
    {
        if (isGrounded) 
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * gravity * -3.0f);
            animator.SetTrigger("Jump");
        }
    }
}

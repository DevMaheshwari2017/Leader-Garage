using UnityEngine;
using UnityEngine.AI;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private Animator animator;
    [SerializeField] private string walkableAreaTag = "Ground";

    private NavMeshAgent navMeshAgent;
    //private RaycastHit hit;
    //private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;


    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        //controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                MoveToTouchPosition(touch.position);
            }
        }

        // Check for mouse input (for testing on PC)
        if (Input.GetMouseButtonDown(0))
        {
            MoveToTouchPosition(Input.mousePosition);
        }

        //isGrounded = controller.isGrounded;
        //// Ensure the agent stops when it reaches the destination
        //if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        //{
        //    animator.SetFloat("Speed", 0);
        //    // Stop animation or any other necessary actions when the player reaches the target
        //}
    }

    void MoveToTouchPosition(Vector3 touchPosition)
    {
        // Convert screen position to ray
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        // Check if the ray hits something
        if (Physics.Raycast(ray, out hit))
        {
            // Move the player to the hit point
            navMeshAgent.SetDestination(hit.point);
        }
    }

    public void ProcessMovement(Vector2 input) 
    {
        // Process touch input
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = cam.ScreenPointToRay(input);
        //    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        //    {
        //        if (hit.collider.CompareTag(walkableAreaTag))
        //        {
        //            agent.SetDestination(hit.point);
        //            Debug.Log("Agent moving to: " + hit.point);
        //        }
        //    }
        //}

        //Vector3 moveDirection = Vector3.zero;
        //moveDirection.x = input.x;
        //moveDirection.z = input.y;

        //Debug.Log("Pos : " + moveDirection);
        ////for horizontal movement

        //controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        //if (moveDirection.z > 0)
        //{
        //    animator.SetFloat("Speed", 0.5f);
        //}
        //else if (moveDirection.z < 0)
        //{
        //    animator.SetFloat("Speed", -0.5f);
        //}
        //else if (moveDirection.x < 0)
        //{
        //    animator.SetFloat("Speed", 1.5f);
        //}
        //else if (moveDirection.x > 0)
        //{
        //    animator.SetFloat("Speed", 2.5f);
        //}
        //else
        //{
        //    animator.SetFloat("Speed", 0);
        //}
        //playerVelocity.y += gravity * Time.deltaTime;

        //if (isGrounded && playerVelocity.y < 0)
        //    playerVelocity.y = -2f;

        ////for vertical movement
        //controller.Move(playerVelocity * Time.deltaTime);
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

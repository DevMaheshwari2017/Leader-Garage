using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float lookRotationSpeed;
    [SerializeField] private float interactRange = 2f;

    [Header("Scripts Refrence")]
    [SerializeField] Interactable interactable;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private const string IDLE = "Idle";
    [SerializeField] private const string WALK = "Walk";

    [Header("Movement")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private LayerMask clickAbleLayers;
    [SerializeField] private ParticleSystem clickEffect;
    [SerializeField] private float smoothFactor;
    [SerializeField] private float moveBackDistance= 5f;

    private PlayerInput input;
    private float holdDuration = 2.0f;

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
    private void Awake()
    {
        input = new PlayerInput();
        AssignInputs();
    }

    private void Update()
    {
        FaceTarget();
        SetAnimation();
        CheckForInteractables();
    }

    private void CheckForInteractables()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Stationary)
            {
                Debug.Log("Holding down");
                holdDuration -= Time.deltaTime;

                if (holdDuration <= 0)
                {
                    Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
                    foreach (Collider collider in colliderArray)
                    {
                        if (collider.TryGetComponent(out Interactable interactable))
                        {
                            Debug.Log("Found a interactable object");
                            interactable.DoorInteraction();
                        }

                    }
                }

            }
        }
        else if (Input.GetMouseButton(0)) // Left mouse button
        {
            Debug.Log("Holding down with mouse");
            holdDuration -= Time.deltaTime;
            Debug.Log("Hold duration is " + holdDuration);
            if (holdDuration <= 0)
            {

                Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
                foreach (Collider collider in colliderArray)
                {
                    if (collider.TryGetComponent(out Interactable interactable))
                    {
                        Debug.Log("Found a interactable object");
                        interactable.DoorInteraction();
                        //could use a fill counter and show it on UI that UI will be the sign to show it's interactable.
                    }

                }
            }
        }
        else { holdDuration = 2f; }
        // Check for mouse click or screen tap
        //if (Input.GetMouseButtonDown(1))
        //{
        //    Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        //    foreach (Collider collider in colliderArray)
        //    {
        //        if (collider.TryGetComponent(out Interactable interactable))
        //        {
        //            Debug.Log("Found a interactable object");
        //            interactable.DoorInteraction();
        //        }

        //    }
        //}
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Interactables"))
        {
            interactable.ShowInteractionPanel();
        }
        else
        {
            Debug.Log("colliding with " + collision.gameObject);
            interactable.HideInteractionPanel();
        }
        if ((clickAbleLayers.value & (1 << collision.gameObject.layer)) == 0)
        {
            agent.isStopped = true;
            Debug.Log("Agent is stopped");
            //StartCoroutine(MoveBackAfterCollision());
        }
    }
    private void FaceTarget() 
    {
        Vector3 direction = (agent.destination - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
        }
    }
    private void AssignInputs()
    {
        input.Touch.TouchMovement.performed += ctx => ClickToMove();
    }

    private void ClickToMove()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, 100, clickAbleLayers))
        {
            agent.isStopped = false;
            agent.destination = hit.point;
            if (clickEffect != null)
            {
                ParticleSystem _clickEffect = Instantiate(clickEffect, hit.point += new Vector3(0, 1f, 0), clickEffect.transform.rotation);
                StartCoroutine(DestroyClickEffect(_clickEffect.gameObject));
            }
        }
    }
    private IEnumerator MoveBackAfterCollision()
    {
        Vector3 moveBackDirection = -transform.forward * moveBackDistance;
        Vector3 targetPosition = transform.position + moveBackDirection;

        float elapsedTime = 0f;
        while (elapsedTime < smoothFactor)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, (elapsedTime / smoothFactor));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
    private IEnumerator DestroyClickEffect(GameObject _clickEffect) 
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(_clickEffect);
    }

    private void SetAnimation()
    {
        if(agent.velocity == Vector3.zero) 
        {
            animator.SetFloat("Speed", 0f);
        }
        else 
        {
            animator.SetFloat("Speed", 1f);
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerModel model;
    [SerializeField] private PlayerInputs playerinputs;

    private Interactable currentInteractable;
    //[SerializeField] private Camera cam;
    //[SerializeField] private float lookRotationSpeed;
    //[SerializeField] private float interactRange = 2f;

    //[Header("UI")]
    //[SerializeField] private Image interactionTimeWheel;

    //[Header("Scripts Refrence")]
    //[SerializeField] Interactable interactable;

    //[Header("Animation")]
    //[SerializeField] private Animator animator;

    //[Header("Movement")]
    //[SerializeField] private NavMeshAgent agent;
    //[SerializeField] private LayerMask clickAbleLayers;
    //[SerializeField] private ParticleSystem clickEffect;
    //[SerializeField] private float smoothFactor;
    //[SerializeField] private float moveBackDistance= 5f;



    private PlayerInput input;

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
        if (playerinputs.isHoldingDownOver(ref model.holdDuration)) {
            Debug.Log("touch ended");
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, model.interactRange);

            Debug.Log("The range is " + model.interactRange);
            if (model != null) { Debug.Log("Model is not null"); }
            //Debug.Log("Number of objects in range: " + colliderArray.Length);
            foreach (Collider collider in colliderArray)
            {
                Debug.Log("Found object: " + collider.gameObject.name);
                if (collider.TryGetComponent(out Interactable interactable))
                {
                    Debug.Log("Found an interactable object: " + collider.gameObject.name);
                    interactable.DoorInteraction();
                }
                else
                {
                   // Debug.Log("No Interactable component found on: " + collider.gameObject.name);
                }

            }
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Interactables"))
        {
            Interactable interactable = collision.GetComponent<Interactable>();
            if (interactable != null)
            {
                // Store and show the interaction panel for the current interactable object
                currentInteractable = interactable;
                currentInteractable.ShowInteractionPanel();
            }
        }
        if ((model.clickAbleLayers.value & (1 << collision.gameObject.layer)) == 0)
        {
            model.agent.isStopped = true;
            //StartCoroutine(MoveBackAfterCollision());
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (currentInteractable != null && collision.gameObject.CompareTag("Interactables"))
        {
            Interactable interactable = collision.GetComponent<Interactable>();
            if (interactable != null && interactable == currentInteractable)
            {
                // Hide the interaction panel and reset currentInteractable
                currentInteractable.HideInteractionPanel();
                currentInteractable = null;
            }
        }
    }
    private void FaceTarget() 
    {
        Vector3 direction = (model.agent.destination - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * model.lookRotationSpeed);
        }
    }
    private void AssignInputs()
    {
        input.Touch.TouchMovement.performed += ctx => ClickToMove();
    }

    private void ClickToMove()
    {
        RaycastHit hit;

        if (Physics.Raycast(model.cam.ScreenPointToRay(Input.mousePosition), out hit, 25, model.clickAbleLayers))
        {
                model.agent.isStopped = false;
                model.agent.destination = hit.point;
                if (model.clickEffect != null)
                {
                    ParticleSystem _clickEffect = Instantiate(model.clickEffect, hit.point += new Vector3(0, 1f, 0), model.clickEffect.transform.rotation);
                    StartCoroutine(DestroyClickEffect(_clickEffect.gameObject));
                }
        }
        else
        {
            model.agent.isStopped = true;
            // Debug.Log("ray collided with " + hit.collider.gameObject.name);
        }

    }
    private IEnumerator DestroyClickEffect(GameObject _clickEffect) 
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(_clickEffect);
    }

    private void SetAnimation()
    {
        if(model.agent.velocity == Vector3.zero) 
        {
            model.animator.SetFloat("Speed", 0f);
        }
        else 
        {
            model.animator.SetFloat("Speed", 1f);
        }
    }
}

using UnityEngine;

public class PlayerController
{
    private TouchInputs touchInputs;
    private PlayerModel model;
    private PlayerView view;
    private Interactable currentInteractable;


   public PlayerController(PlayerModel _model, PlayerView _view) 
    {
        model = _model;
        view = _view;

        view.SetPlayerController(this);
    }

    public void UpdatePlayer(Transform transform) 
    {
        UpdateMovementData();
        SetAnimation();
        CheckForInteractables(transform);
        FaceTarget(transform);
    }
    public void CheckForInteractables(Transform transform)
    {
        if (view.IsHoldingInput(ref model.holdDuration)) {
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, model.interactRange);
            foreach (Collider collider in colliderArray)
            {
                    Debug.Log("Found object with collider " + collider.gameObject.name);
                if (collider.TryGetComponent(out Interactable interactable))
                {
                    Debug.Log("Found object to interact with");
                    interactable.DoorInteraction();
                }
            }
        }
    }
    public void OnEnterInteractable(Collider collision)
    {
        if (collision.CompareTag("Interactables"))
        {
            Interactable interactable = collision.GetComponent<Interactable>();
            if (interactable != null)
            {
                // Store and show the interaction panel for the current interactable object
                currentInteractable = interactable;
                currentInteractable.ShowInteractionPanel();
            }
        }
        view.StopPlayerMovement(collision);
    }

    public void OnExitInteractable(Collider collision)
    {
        if (currentInteractable != null && collision.CompareTag("Interactables"))
        {
                // Hide the interaction panel and reset currentInteractable
                currentInteractable.HideInteractionPanel();
                currentInteractable = null;
        }
    }
    public void FaceTarget(Transform transform) 
    {
        if (model.agentVelocity.magnitude > 0.1f)  // Check if player is moving
        {
            Vector3 direction = (model.agentDestination - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                if (Vector3.Distance(transform.position, model.agentDestination) < 0.2f)
                {
                    // Stop rotating when player is close enough to the destination
                    return;
                }
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * model.lookRotationSpeed);
            }
        }
    }

    public void ClickToMove()
    {
        RaycastHit hit;

        if (view.RaycastFromScreen(out hit))
        {
            model.agentDestination = hit.point;
            view.MoveAgentToPosition(model.agentDestination);
            view.PlayClickEffect(hit.point);
        }
    }

    private void SetAnimation()
    {
        view.SetAnimationSpeed(model.agentVelocity == Vector3.zero ? 0f : 1f);
    }

    private void UpdateMovementData() 
    {
        model.agentDestination = view.GetAgentDestination();
        model.agentVelocity = view.GetAgentVelocity();
    }
}

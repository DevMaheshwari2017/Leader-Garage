using System;
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
        if (view.IsHoldingInput(ref model.holdDuration))
        {
            // Don't allow movement when holding input
            view.StopPlayerMovement();
            SetAnimation(); // Optionally, you can stop animations or set to idle here.
            CheckForInteractables(transform); // You can still check for interactables during hold.
        }
        else
        {
            // Resume movement when not holding input
            UpdateMovementData();
            SetAnimation();
            FaceTarget(transform);
        }
    }
    public void CheckForInteractables(Transform transform)
    {
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, model.interactRange);
            foreach (Collider collider in colliderArray)
            {
                Debug.Log("Found object with collider " + collider.gameObject.name);
                if (collider.TryGetComponent(out Interactable interactable) && collider.CompareTag("GarageDoorButton"))
                {
                    Debug.Log("Found object to interact with");
                    interactable.GarageDoorInteraction();
                }
                else if (collider.TryGetComponent(out NPC_Controller nPC))
                {
                    Debug.Log("Collided with NPC");
                    nPC.ShowConversation();
                }
            }
    }
    public void OnEnterInteractable(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Interactables"))
        {
            Interactable interactable = collision.GetComponent<Interactable>();
            if (interactable != null)
            {
                // Store and show the interaction panel for the current interactable object
                currentInteractable = interactable;
                currentInteractable.ShowInteractionPanel();
                view.StopPlayerMovement();
            }
        }
    }

    public void OnExitInteractable(Collider collision)
    {
        if (currentInteractable != null && collision.gameObject.layer == LayerMask.NameToLayer("Interactables"))
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

        RaycastHit[] hits = view.RaycastAllFromScreen();

        // Sort hits based on distance to ensure we process the closest object first
        Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

        foreach (RaycastHit hit in hits)
        {
            // Check if the hit object is within the clickable layers
            if (((1 << hit.collider.gameObject.layer) & view.GetClickableLayers()) != 0)
            {
                // Proceed with movement if it's a clickable layer
                model.agentDestination = hit.point;
                view.MoveAgentToPosition(model.agentDestination);
                view.PlayClickEffect(hit.point);
                return; // Exit after processing the first valid hit
            }
            else
            {
                // If we hit a non-clickable object, stop the movement and return
                Debug.Log("Hit object " + hit.collider.gameObject.name +"is not on clickable layers. Ignoring movement.");
                return;
            }
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

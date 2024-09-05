using System.Collections;
using UnityEngine;

public class PlayerController
{
    private TouchInputs touchInputs;
    private PlayerModel model;
    private PlayerView view;
    private Interactable currentInteractable;


    PlayerController(PlayerModel _model, PlayerView _view) 
    {
        model = _model;
        view = _view;

        view.SetPlayerController(this);
        model.SetPlayerController(this);
    }

    private void Update()
    {
        SetAnimation();   
    }

    public void CheckForInteractables(Transform transform)
    {
        if (touchInputs.isHoldingDownOver(ref model.holdDuration)) {
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, model.interactRange);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out Interactable interactable))
                {
                    interactable.DoorInteraction();
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
    public void FaceTarget(Transform transform) 
    {
        Vector3 direction = (model.agent.destination - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * model.lookRotationSpeed);
        }
    }

    public void ClickToMove()
    {
        RaycastHit hit;

        if (Physics.Raycast(model.cam.ScreenPointToRay(Input.mousePosition), out hit, 25, model.clickAbleLayers))
        {
                model.agent.isStopped = false;
                model.agent.destination = hit.point;
                if (model.clickEffect != null)
                {
                   ParticleSystem _clickEffect = GameObject.Instantiate(model.clickEffect, hit.point += new Vector3(0, 1f, 0), model.clickEffect.transform.rotation);
                   //StartCoroutine(DestroyClickEffect(_clickEffect.gameObject));
                }
        }
        else
        {
            model.agent.isStopped = true;
        }

    }
    private IEnumerator DestroyClickEffect(GameObject _clickEffect) 
    {
        yield return new WaitForSeconds(0.5f);
       // Destroy(_clickEffect);
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

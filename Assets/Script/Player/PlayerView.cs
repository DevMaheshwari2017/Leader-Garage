using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Image interactionTimeWheel;
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private ParticleSystem clickEffect;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask clickAbleLayers;

    private TouchInputs touchInputs;
    private PlayerController playerController;
    private PlayerInput input;

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public void SetPlayerController(PlayerController playerController) 
    {
        this.playerController = playerController;
    }

    private void Awake()
    {
        touchInputs = GetComponent<TouchInputs>();
        PlayerModel model = new PlayerModel();
        PlayerController controller = new PlayerController(model, this);
        input = new PlayerInput();
        AssignInputs();
    }
    private void Update()
    {
        playerController.UpdatePlayer(transform);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (playerController != null)
        { 
          playerController.OnEnterInteractable(collision);
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if(playerController != null) 
        {
          playerController.OnExitInteractable(collision);
        }
    }

    public bool RaycastFromScreen(out RaycastHit hit)
    {
        return Physics.Raycast(cam.ScreenPointToRay(UnityEngine.Input.mousePosition), out hit, 25f, clickAbleLayers);
    }

    public void StopPlayerMovement(Collider collision) 
    {
        if ((clickAbleLayers.value & (1 << collision.gameObject.layer)) == 0)
        {
            agent.isStopped = true;
            //StartCoroutine(MoveBackAfterCollision());
        }
    }

    // Moves the NavMeshAgent to the destination
    public void MoveAgentToPosition(Vector3 destination)
    {
        agent.isStopped = false;
        agent.destination = destination;
    }

    // Plays the click effect at the given position
    public void PlayClickEffect(Vector3 position)
    {
        if (clickEffect != null)
        {
           ParticleSystem _clickEffect = Instantiate(clickEffect, position, Quaternion.identity);
            StartCoroutine(DestroyClickEffect(_clickEffect.gameObject));
        }
    }

    private IEnumerator DestroyClickEffect(GameObject _clickEffect)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(_clickEffect);
    }

    public Vector3 GetAgentDestination()
    {
        return agent.destination;
    }

    public Vector3 GetAgentVelocity()
    {
        return agent.velocity;
    }

    public void SetAnimationSpeed(float speed)
    {
        animator.SetFloat("Speed", speed);
    }

    // Helper method to check for holding input and increment hold duration
    public bool IsHoldingInput(ref float holdDuration)
    {
        return touchInputs.isHoldingDownOver(ref holdDuration);
    }

    private void AssignInputs()
    {
        input.Touch.TouchMovement.performed += ctx => playerController.ClickToMove();
    }
}

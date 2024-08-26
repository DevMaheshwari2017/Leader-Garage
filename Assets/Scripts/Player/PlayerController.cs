using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private const string IDLE = "Idle";
    [SerializeField] private const string Walk = "Walk";

    [Header("Movement")]
    [SerializeField] private UnityEngine.AI.NavMeshAgent navMeshAgent;
    [SerializeField] private LayerMask clickAbleLayers;
    [SerializeField] private ParticleSystem clickEffect;
    PlayerMovement input;


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
        input = new PlayerMovement();
        AssignInputs();
    }

    private void AssignInputs()
    {
        input.Touch.TouchMove.performed += ctx => ClickToMove();
    }

    private void ClickToMove()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, 100, clickAbleLayers))
        {
            navMeshAgent.destination = hit.point;
            if (clickEffect != null)
            {
                Instantiate(clickEffect, hit.point += new Vector3(0, 1f, 0), clickEffect.transform.rotation);
            }
        }

    }

    private void ChangerAnimationState()
    {

    }
    private void SetAnimation()
    {

    }
}

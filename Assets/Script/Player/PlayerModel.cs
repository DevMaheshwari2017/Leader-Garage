using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PlayerModel
{
    public Camera cam;
    public float lookRotationSpeed; 

    public float interactRange   = 3f;
    public Image interactionTimeWheel;

    public Animator animator;

    public NavMeshAgent agent;
    public LayerMask clickAbleLayers;
    public ParticleSystem clickEffect;

    public float holdDuration = 2f;

    private PlayerController playerController;

    public void SetPlayerController(PlayerController playerController) 
    {
        this.playerController = playerController;
    }

}

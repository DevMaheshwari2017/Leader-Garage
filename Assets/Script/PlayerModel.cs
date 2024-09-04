using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PlayerModel : MonoBehaviour
{
    public Camera cam;
    public float lookRotationSpeed; 

    public float interactRange   = 50f;
    public Image interactionTimeWheel;

    public Animator animator;

    public NavMeshAgent agent;
    public LayerMask clickAbleLayers;
    public ParticleSystem clickEffect;

    public float holdDuration = 2f;

}

using UnityEngine;
using TMPro;
using System.Collections;

public class NPC_Controller : MonoBehaviour
{
    // a function that shows messages linearly - story npc

    //For future NPC  --
    //Can place waypoints all over the city and then can use physics.OverlapSphere to check if there's any waypoint ahead of us and move the NPC to the that waypoint
    // can use same system to choose between different pathways - if more than one waypoint in collision array && localRotation is different for them too then random function to choose 

    public enum NPCType
    {
        StaticNPC,
        RandomConverstionalNPC,
        StoryNPC,
        SideQuestNPC,
        StaticTalkingNPC,
        RomingNPC
    }

    public NPCType type;

    [TextArea(5,20)]
    [SerializeField] private string[] randomMessages;
    [TextArea(5,20)]
    [SerializeField] private string[] CollisionMessages;
    [SerializeField] private TextMeshProUGUI messageToDisplay;
    [SerializeField] private Animator animator;

    [Header("Roming NPC")]
    [SerializeField] private GameObject waypointContainer;
    [SerializeField] private Transform[] waypoints;

    private int index = 0;
    private int minDistance = 5;
    private PlayerView view;
    private bool hasNPCCollide;
    private bool isWaiting;

    private void Start()
    {
        if (type == NPCType.RomingNPC)
        {
            waypoints = new Transform[waypointContainer.transform.childCount]; 
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = waypointContainer.transform.GetChild(i);
            }
        }
    }
    private void Update()
    {
        SetNPCAnimation(type);
        if (type == NPCType.RomingNPC && !hasNPCCollide)
        {
            SetNPCAnimation(type, "Walk");
        }
    }

    private void FixedUpdate()
    {
            if (type == NPCType.RomingNPC && !hasNPCCollide)
            {
                MoveNPC();
            }
       
    }

    private void OnTriggerEnter(Collider collision)
    {
        view = collision.GetComponent<PlayerView>();
        if (view != null && type == NPCType.RomingNPC)
        {
            Debug.Log("entered Collison with player");
            hasNPCCollide = true;
            SetNPCAnimation(type, "Idle");
            ShowCollisionMessages(0);
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        view = collision.GetComponent<PlayerView>();
        hasNPCCollide = true;
        if (view != null && type == NPCType.RomingNPC && !isWaiting)  
        {
           StartCoroutine( WaitToShowMessageAgain(type));         
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        view = collision.GetComponent<PlayerView>();
        if (view != null && type == NPCType.RomingNPC) 
        {
            Debug.Log("exited Collison with player");
            hasNPCCollide = false;
            SetNPCAnimation(type, "Walk");
            ShowCollisionMessages(2);
        }
    }

    private IEnumerator WaitToShowMessageAgain(NPCType type) 
    {
        isWaiting = true;
        yield return new WaitForSeconds(5.0f);
        Debug.Log("Trying to call stay collision message");
        if (type == NPCType.RomingNPC && hasNPCCollide == true) 
        {
            ShowCollisionMessages(1);
        }
        isWaiting = false;
    } 
    private void MoveNPC() 
    {
        if (Vector3.Distance(transform.position, waypoints[index].position) < minDistance)
        {
            if (index >= 0 && index < waypoints.Length - 1 )
            {
                index ++;
            }
            else
            {
                index = 0;
            }
        }

        transform.position = Vector3.Lerp(transform.position, waypoints[index].position, Time.fixedDeltaTime);
        transform.localRotation = waypoints[index].localRotation;
        
    }

    public void ShowConversation() 
    {
        switch (type) 
        {
            case NPCType.RandomConverstionalNPC:
                GenrateRandomMessage();
            break;

            case NPCType.StoryNPC:
                Debug.Log("Collision with story NPC");
            break;

            case NPCType.SideQuestNPC:
                Debug.Log("Collision with side quest NPC");
            break;
            case NPCType.RomingNPC:
                Debug.Log("Collision with roming NPC");
            break;

            case NPCType.StaticTalkingNPC: 
                GenrateRandomMessage();
            break;

            default: Debug.Log("Staic NPC : Can't interact");
            break;
        }
    }

    private void GenrateRandomMessage() 
    {
        int randomNumber = Random.Range(0, randomMessages.Length);
        messageToDisplay.text = randomMessages[randomNumber];
        StartCoroutine(ClearMessage());
    }

    private void ShowCollisionMessages(int index) 
    {
        messageToDisplay.text = CollisionMessages[index];
        Debug.Log("Showing collison message : " + CollisionMessages[index]);  
        StartCoroutine(ClearMessage());
    }

    private IEnumerator ClearMessage() 
    {
        yield return new WaitForSeconds(2.0f);
        messageToDisplay.text = string.Empty;
    }
    private void SetNPCAnimation(NPCType type,string animationState = "Idle") 
    {
        //float damTime = 0.2f;
        switch (type) 
        {
            case NPCType.RomingNPC:
                if (animationState == "Walk")
                {

                    animator.SetFloat("Speed_NPC", 1);
                }
                else if (animationState == "Idle")
                {
                    animator.SetFloat("Speed_NPC", 0);
                }
            break;
            case NPCType.StaticTalkingNPC: 
                animator.SetBool("IsNPCStatic", true);
            break;

        }
    }
}

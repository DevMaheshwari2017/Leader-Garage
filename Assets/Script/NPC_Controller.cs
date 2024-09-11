using UnityEngine;
using TMPro;

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

    [SerializeField] private string[] randomMessages;
    [SerializeField] private string[] storyMessages;
    [SerializeField] private TextMeshProUGUI messageToDisplay;
    [SerializeField] private Animator animator;

    [Header("Roming NPC")]
    [SerializeField] private GameObject waypointContainer;
    [SerializeField] private Transform[] waypoints;

    private int index = 0;
    private int minDistance = 5;

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
        SetNPCAnimation();

            if (type == NPCType.RomingNPC )
            {
                MoveNPC();
            }
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
            transform.position = Vector3.Lerp(transform.position, waypoints[index].position, Time.deltaTime);
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
    }
    private void SetNPCAnimation() 
    {
        if (type == NPCType.RomingNPC) 
        {
            animator.SetFloat("Speed_NPC", 1);
        }
        else if(type == NPCType.StaticTalkingNPC)
        {
            animator.SetBool("IsNPCStatic", true);
        }
    }
}

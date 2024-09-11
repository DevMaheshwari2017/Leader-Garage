using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshSurface cityNavMeshSurface;

    private bool canPlayerInteract;

    // 1 = open, 0 = close 
    [SerializeField]private int doorOpenClose = 1;

    //setter
    public void SetCanPlayerInteract(bool can) => canPlayerInteract = can; 
    public void ShowInteractionPanel() 
    {
        interactionPanel.SetActive(true);
        canPlayerInteract = true;
    }
    public void HideInteractionPanel() 
    {
        interactionPanel.SetActive(false);
        canPlayerInteract = false;
    }

    public void GarageDoorInteraction()
    {
        if (!canPlayerInteract) return;
            
            // Perform your logic on the interactable object
            if (doorOpenClose == 1)
            {
                OpenGarageDoor();
                //houseGameObject.SetActive(true);
            }
            else if (doorOpenClose == 0) 
            {
                CloseGarageDoor();
            }
    }

    private void OpenGarageDoor()
    {
            animator.SetInteger("DoorOpenClose", doorOpenClose);
            doorOpenClose = 0;
            cityNavMeshSurface.enabled = true;
    }

    private void CloseGarageDoor() 
    {

        animator.SetInteger("DoorOpenClose", doorOpenClose);
        doorOpenClose = 1;
        cityNavMeshSurface.enabled = false;
    }
}

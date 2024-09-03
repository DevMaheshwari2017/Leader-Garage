using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject houseGameObject;

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

    public void DoorInteraction()
    {
        if (!canPlayerInteract) return;
            
            // Perform your logic on the interactable object
            if (doorOpenClose == 1)
            {
            Debug.Log("Opening door");
                OpeneDoor();
                //houseGameObject.SetActive(true);
            }
            else if (doorOpenClose == 0) 
            {
                CloseDoor();
            }
    }

    private void OpeneDoor()
    {
            animator.SetInteger("DoorOpenClose", doorOpenClose);
            doorOpenClose = 0;
            Debug.Log("Door opened");
    }

    private void CloseDoor() 
    {

        animator.SetInteger("DoorOpenClose", doorOpenClose);
        doorOpenClose = 1;
        Debug.Log("Door closed");
        houseGameObject.SetActive(false);
    }
}

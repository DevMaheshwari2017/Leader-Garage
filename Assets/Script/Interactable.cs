using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject houseGameObject;

    private bool canPlayerTap;

    // 1 = open, 0 = close 
    private int doorOpenClose = 1;

    public void ShowInteractionPanel() 
    {
        interactionPanel.SetActive(true);
        canPlayerTap = true;
    }
    public void HideInteractionPanel() 
    {
        interactionPanel.SetActive(false);
        canPlayerTap = false;
    }

    public void DoorInteraction()
    {
        if (!canPlayerTap) return;
            
            // Perform your logic on the interactable object
            if (doorOpenClose == 1)
            {
                OpeneDoor();
                houseGameObject.SetActive(true);
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

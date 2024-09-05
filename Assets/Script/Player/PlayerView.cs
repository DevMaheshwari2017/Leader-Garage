using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private PlayerController playerController;
    public void SetPlayerController(PlayerController playerController) 
    {
        this.playerController = playerController;
    }

    private void Update()
    {
        playerController.FaceTarget(transform);
        playerController.CheckForInteractables(transform);
    }
}

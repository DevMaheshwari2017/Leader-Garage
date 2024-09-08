using UnityEngine;

public class TouchInputs : MonoBehaviour
{

    public bool isHoldingDownOver(ref float holdDuration, bool isPlyaerHoldingdown = false) 
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Stationary && holdDuration > 0)
            {
                holdDuration -= Time.deltaTime;
                Debug.Log("Holding duration " + holdDuration);
                isPlyaerHoldingdown = true;
                return false;
            }
            if (holdDuration <= 0 && touch.phase == TouchPhase.Ended)
            {
                isPlyaerHoldingdown = false;
                return true;
            }
            return false;
        }

        else
        {
            holdDuration = 2f; 
            return false;
        }
    }
}

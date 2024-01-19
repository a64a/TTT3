using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DoubleClickButton : MonoBehaviour, IPointerClickHandler
{
    public float doubleClickTimeThreshold = 0.2f;
    private float lastClickTime;
    public System.Action<Button> doubleClickCallback;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Time.time - lastClickTime < doubleClickTimeThreshold)
        {
            HandleDoubleClick();
            lastClickTime = 0f;
        }
        else
        {
            // Single click
            lastClickTime = Time.time;
        }
    }

    private void HandleDoubleClick()
    {
        // Code to execute on double click
        if (doubleClickCallback != null)
        {
            doubleClickCallback.Invoke(GetComponent<Button>());
        }
    }
}

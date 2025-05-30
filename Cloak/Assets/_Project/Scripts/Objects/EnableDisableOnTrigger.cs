using UnityEngine;

public class EnableDisableOnTrigger2D : MonoBehaviour
{
    public GameObject targetObject; // The GameObject to enable/disable
    public string triggerTag = "Player"; // Optional: Tag to filter triggers

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check for the tag if you want to filter specific triggers
        if (string.IsNullOrEmpty(triggerTag) || other.CompareTag(triggerTag))
        {
            if (targetObject != null)
            {
                targetObject.SetActive(true); // Enable the target object
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check for the tag if you want to filter specific triggers
        if (string.IsNullOrEmpty(triggerTag) || other.CompareTag(triggerTag))
        {
           if (targetObject != null)
            {
               targetObject.SetActive(false); // Disable the target object
            }
        }
    }
}
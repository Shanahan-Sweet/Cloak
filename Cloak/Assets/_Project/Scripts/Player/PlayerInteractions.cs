using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] PlayerInput inputScript;


    [SerializeField] Transform interactHighlight;

    bool hasTarget = false;
    List<Transform> currentCollisions = new List<Transform>();
    IInteractable interactTarget;
    Transform interactTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactHighlight.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Find nearest target
        if (currentCollisions.Count == 0)
        {
            if (hasTarget)//remove target
            {
                hasTarget = false;
                interactTarget = null;
                interactHighlight.gameObject.SetActive(false);
            }
            return;
        }

        IInteractable nearestTarget = null;
        float nearestDist = 100;
        foreach (Transform trans in currentCollisions)
        {
            if (trans != null && trans.TryGetComponent(out IInteractable interactable) && (trans.position - transform.position).magnitude < nearestDist)
            {
                nearestTarget = interactable;
                interactTransform = trans;
                nearestDist = (trans.position - transform.position).magnitude;
            }
        }
        if (nearestTarget != null)
        {
            if (!hasTarget)//set has target
            {
                hasTarget = true;
                interactHighlight.gameObject.SetActive(true);
            }

            if (interactTarget != nearestTarget)//is new target
            {
                interactTarget = nearestTarget;
            }
        }
    }

    void LateUpdate()
    {
        if (hasTarget) interactHighlight.position = interactTransform.position + new Vector3(0, 1, 0);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!currentCollisions.Contains(collision.transform))
        {
            currentCollisions.Add(collision.transform);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (currentCollisions.Contains(collision.transform))
        {
            currentCollisions.Remove(collision.transform);
        }
    }

    //Interact
    public void InteractAction()
    {
        if (hasTarget) interactTarget.PlayerInteract(this);
    }



    //Connections
    public void Teleport(Vector2 newPos)
    {
        inputScript.transform.position = newPos;
    }
}

using UnityEngine;

public class Connection : MonoBehaviour, IInteractable
{
    [SerializeField] Connection myConnection;
    public InteractableType interactType { get { return interactType; } }
    public void PlayerInteract(PlayerInteractions playerInteractions)
    {
        playerInteractions.Teleport(myConnection.transform.position + new Vector3(0, -1, 0));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

}

using UnityEngine;

public class Connection : MonoBehaviour, IInteractable
{
    [SerializeField] Connection myConnection;
    public InteractableType interactType { get { return interactType; } }
    public void PlayerInteract(PlayerInteractions playerInteractions)
    {
        playerInteractions.StartRoomTransition(myConnection.transform.position + new Vector3(0, -.75f, 0));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

}

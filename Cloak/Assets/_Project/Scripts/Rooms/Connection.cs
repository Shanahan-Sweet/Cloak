using UnityEngine;

public class Connection : MonoBehaviour, IInteractable
{
    [SerializeField] Connection myConnection;
    Room myRoom;
    public InteractableType interactType { get { return interactType; } }
    public void PlayerInteract(PlayerInteractions playerInteractions)
    {
        playerInteractions.StartRoomTransition(myConnection.transform.position + new Vector3(0, -.75f, 0), myConnection.myRoom);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRoom = GetComponentInParent<Room>();
        if (myRoom == null) print("No Parent Room Error!!!");
    }

}

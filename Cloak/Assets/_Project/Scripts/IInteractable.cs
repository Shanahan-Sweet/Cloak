using UnityEngine;

public interface IInteractable
{
    public InteractableType interactType { get { return interactType; } }
    public void PlayerInteract(PlayerInteractions playerInteractions) { }
}

public enum InteractableType
{
    Simple, Connection
}
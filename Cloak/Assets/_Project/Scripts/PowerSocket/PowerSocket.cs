using UnityEngine;

public class PowerSocket : MonoBehaviour
{
    [SerializeField] Transform playerHoldPoint;
    [SerializeField] GameObject powerLight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        powerLight.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMovement player))
        {
            player.SocketCollision(this, playerHoldPoint);
        }
    }

    public void SetPowered()//catch player
    {
        powerLight.SetActive(true);
    }

    public void PowerDown()//releasePlayer
    {
        powerLight.SetActive(false);
    }
}

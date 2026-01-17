using System.Collections.Generic;
using UnityEngine;

public class PowerSocket : MonoBehaviour
{
    [SerializeField] Transform playerHoldPoint;
    [SerializeField] GameObject powerLight;

    [SerializeField] List<IPowerObject> powerObjects = new List<IPowerObject>();


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

        foreach (IPowerObject powerObject in powerObjects)
        {
            powerObject.SetPowerState(true);
        }
    }

    public void PowerDown()//releasePlayer
    {
        powerLight.SetActive(false);

        foreach (IPowerObject powerObject in powerObjects)
        {
            powerObject.SetPowerState(false);
        }
    }

    public void SetPlayerInput(Vector2 inputDir)//player input
    {
        foreach (IPowerObject powerObject in powerObjects)
        {
            powerObject.SetPlayerInput(inputDir);
        }
    }
}

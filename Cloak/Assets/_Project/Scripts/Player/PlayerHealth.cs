using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    bool isDead = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }


    public void KillPlayer()
    {
        if (isDead) return;

        print("Kill Player");

        isDead = true;
        StartCoroutine(KillSequence());
    }

    IEnumerator KillSequence()
    {
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);
    }
}

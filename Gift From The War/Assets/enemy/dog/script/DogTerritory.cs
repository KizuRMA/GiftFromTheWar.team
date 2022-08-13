using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogTerritory : MonoBehaviour
{
    [SerializeField] public EnemyManager owner;
    private GameObject player;
    public bool isPlayerJoin;

    // Start is called before the first frame update
    void Start()
    {
        isPlayerJoin = false;
        player = owner.player;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Player") return;

        isPlayerJoin = true;
        Debug.Log(isPlayerJoin);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player") return;

        isPlayerJoin = false;
        Debug.Log(isPlayerJoin);
    }
}

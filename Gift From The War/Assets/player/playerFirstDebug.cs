using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFirstDebug : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] public List<Transform> positions;

    private void Start()
    {

    }

    private void Update()
    {
        for (int i = 1; i < 10; i++)
        {
            KeyUpdate(i);
        }
    }

    private void KeyUpdate(int i)
    {
        if (Input.GetKeyDown(i.ToString("0")) && positions.Count >= i)
        {
            controller.enabled = false;
            controller.transform.position = positions[i - 1].position;
            controller.enabled = true;
        }
    }
}

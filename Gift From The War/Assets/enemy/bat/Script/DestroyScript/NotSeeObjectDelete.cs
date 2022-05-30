using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotSeeObjectDelete : MonoBehaviour
{
    float elapsedTime;
    Renderer targetRenderer;

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0;
        targetRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= 20 && targetRenderer.isVisible == false)
        {
            Destroy(gameObject);
        }
    }
}

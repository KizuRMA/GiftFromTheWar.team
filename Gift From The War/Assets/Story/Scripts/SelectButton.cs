using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButton : MonoBehaviour
{

    Layer layer;

    // Start is called before the first frame update
    void Start()
    {
        layer = GameObject.Find("ScenarioManager").GetComponent<Layer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Buttton()
    {
        layer.ClosePanel();
        ScenarioManager.Instance.RequestNextLine();
    }
}
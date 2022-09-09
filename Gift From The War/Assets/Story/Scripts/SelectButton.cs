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

    public void Buttton(string command)
    {
        if (command == "Root1")
        {
            layer.ClosePanel();
            ScenarioManager.Instance.RequestNextLine();
        }
        else if (command == "Root2")
        {
            layer.ClosePanel();
          
            ScenarioManager.Instance.UpdateLines("Scenario1-1");
            ScenarioManager.Instance.currentLine=0;

            ScenarioManager.Instance.RequestNextLine();

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionScript : MonoBehaviour
{
    private GameObject[] ChildObjects;
    // Start is called before the first frame update
    void Start()
    {
        ChildObjects = new GameObject[gameObject.transform.childCount];

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            ChildObjects[i] = gameObject.transform.GetChild(i).gameObject;
            ChildObjects[i].AddComponent<Rigidbody>();
            ChildObjects[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            ChildObjects[i].AddComponent<MeshCollider>();
            ChildObjects[i].AddComponent<NotSeeObjectDelete>();
            ChildObjects[i].GetComponent<MeshCollider>().convex = true;
        }

        var random = new System.Random();
        var min = -3;
        var max = 3;

        foreach (GameObject game in ChildObjects)
        {
            Rigidbody r = game.GetComponent<Rigidbody>();

            r.transform.SetParent(null);
            r.isKinematic = false;

            var vect = new Vector3(random.Next(min, max), random.Next(0, max), random.Next(min, max));
            r.AddForce(vect, ForceMode.Impulse);
            r.AddTorque(vect, ForceMode.Impulse);
        }

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

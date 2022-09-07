using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawDestructionScript : MonoBehaviour
{
    private GameObject[] ChildObjects;

    private void Awake()
    {
        ChildObjects = new GameObject[gameObject.transform.childCount];

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            ChildObjects[i] = gameObject.transform.GetChild(i).gameObject;
            //ChildObjects[i].AddComponent<Rigidbody>();
            //ChildObjects[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            //ChildObjects[i].AddComponent<BoxCollider>();
            Destroy(ChildObjects[i],5.0f);
            //ChildObjects[i].GetComponent<MeshCollider>().convex = true;
            //ChildObjects[i].GetComponent<BoxCollider>().convex = true;
        }

        var random = new System.Random();
        var min = -5;
        var max = 5;

        foreach (GameObject game in ChildObjects)
        {
            Rigidbody r = game.GetComponent<Rigidbody>();

            r.transform.SetParent(null);
            r.isKinematic = false;

            var vect = new Vector3(random.Next(min, max), random.Next(0, max), random.Next(min, max));
            r.AddForce(vect, ForceMode.Impulse);
            r.AddTorque(vect, ForceMode.Impulse);
        }
    }

    private void Start()
    {
        //AudioManager.Instance.PlaySE("BatDead", isLoop: false);
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 1.0f);
    }
}

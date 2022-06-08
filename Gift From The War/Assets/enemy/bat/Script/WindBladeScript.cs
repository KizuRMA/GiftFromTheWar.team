using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBladeScript : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    private float speed;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnParticleCollision(GameObject other)
    {
        var newParticle = Instantiate(particle);
        newParticle.transform.position = transform.position;
        newParticle.transform.rotation = transform.rotation;
    }
}

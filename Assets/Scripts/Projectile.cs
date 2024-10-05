using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 _initPosition;
    private Quaternion _initRotation;
    private int _velocity = 1;
    // Start is called before the first frame update
    void Start()
    {
        _initPosition = transform.position;
        _initRotation = transform.rotation;
        Invoke(nameof(DestroySelf), 4f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.position), 100);
        transform.position += transform.forward * _velocity;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Terrain"))
        {
            Destroy(this.gameObject);
        }
    }

    private void DestroySelf() {
        Destroy(this.gameObject);
    }

}

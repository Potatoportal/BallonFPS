using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartMonkey : MonoBehaviour
{
    [SerializeField] private GameObject projectile;

    private bool _hasFired = false;
    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponentInChildren<Animator>();
        animator.Play("Sit");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (_hasFired) {
            return;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(other.transform.position - transform.position), 100);
            
            Instantiate(projectile, new Vector3(transform.position.x, transform.position.y+0.5f, transform.position.z), transform.rotation);
            _hasFired = true;
            Invoke(nameof(ResetHasFired), 1.5f);
        }
        

    }

    private void ResetHasFired() {
        _hasFired = false;
    }

}

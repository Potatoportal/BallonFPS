using System;
using UnityEngine;

public class MOAB : Bloon
{
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        _player = target.GetComponent<Player>();
    }

    void Update()
    {

        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), 100);
        Vector3 playerPos = target.transform.position;
        playerPos.y = transform.position.y;
        transform.LookAt(playerPos);
        Vector3 forward = transform.position + (transform.forward * moveSpeed * Time.deltaTime);
        float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
        transform.position = new Vector3(forward.x, terrainHeight + 5f, forward.z);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (popped) {
            return;
        }
        if (other.CompareTag("Player"))
        {
            _player.takeDamage(health);
            Destroy(this.gameObject);
            BloonPopped?.Invoke(this, EventArgs.Empty);
        }
        else if (other.CompareTag("Projectile"))
        {
            Destroy(other.gameObject);
            health--;
            if (health <= 0) {
                popped = true;
                OnBloonPopped();
            }
        }
        else if (other.CompareTag("Explosion")) {
            //other.gameObject.GetComponent<SphereCollider>();
            health--;
            if (health <= 0)
            {
                popped = true;
                OnBloonPopped();
            }
        }
        
    }

    public new event EventHandler BloonPopped;
    public new event EventHandler<NextBloonEventArgs> NextBloon;

    protected new void OnBloonPopped()
    {
        AudioSource.PlayClipAtPoint(popClip, transform.position);
        if (next != null)
        {
            Vector3 position = transform.position;
            GameObject go = Instantiate(next, new Vector3(position.x + UnityEngine.Random.Range(-5, 5), position.y, position.z + UnityEngine.Random.Range(-5, 5)), transform.rotation);
            GameObject go1 = Instantiate(next, new Vector3(position.x + UnityEngine.Random.Range(-5, 5), position.y, position.z + UnityEngine.Random.Range(-5, 5)), transform.rotation);
            GameObject go2 = Instantiate(next, new Vector3(position.x + UnityEngine.Random.Range(-5, 5), position.y, position.z + UnityEngine.Random.Range(-5, 5)), transform.rotation);
            GameObject go3 = Instantiate(next, new Vector3(position.x + UnityEngine.Random.Range(-5, 5), position.y, position.z + UnityEngine.Random.Range(-5, 5)), transform.rotation);
            NextBloon?.Invoke(this, new NextBloonEventArgs { Next = go.GetComponent<Bloon>() });
            NextBloon?.Invoke(this, new NextBloonEventArgs { Next = go1.GetComponent<Bloon>() });
            NextBloon?.Invoke(this, new NextBloonEventArgs { Next = go2.GetComponent<Bloon>() });
            NextBloon?.Invoke(this, new NextBloonEventArgs { Next = go3.GetComponent<Bloon>() });
        }
        Destroy(this.gameObject);
        _player.AddMoney(100);
        
    }

}
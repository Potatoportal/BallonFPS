using System;
using UnityEngine;

public class Bloon : MonoBehaviour
{
    [SerializeField] internal GameObject target;
    [SerializeField] internal float moveSpeed = 1;
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] internal int health = 1;
    [SerializeField] internal GameObject next;
    [SerializeField] internal AudioClip popClip;

    internal Player _player;
    private bool _invincibility = true;
    internal bool popped = false;

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        _player = target.GetComponent<Player>();
        Invoke(nameof(ResetInvincibility), 0.1f);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    
    void Update()
    {
        
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), 100);
        Vector3 targetPosition = target.transform.position;
        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition);

        Vector3 forward = transform.position + (transform.forward * moveSpeed * Time.deltaTime);
        float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
        transform.position = new Vector3(forward.x, terrainHeight+1f, forward.z);
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (_invincibility || popped) {
            return;
        }
        if (other.CompareTag("Player"))
        {
            //Debug.Log("hit: " + other.gameObject.name);
            _player.takeDamage(health);
            Destroy(this.gameObject);
            BloonPopped?.Invoke(this, EventArgs.Empty);
        }
        else if (other.CompareTag("Projectile"))
        {
            Destroy(other.gameObject);
            OnBloonPopped();
        }
        else if (other.CompareTag("Explosion")) {
            other.gameObject.GetComponent<SphereCollider>();
            OnBloonPopped();
        }
        
    }
    private void ResetInvincibility() {
        _invincibility = false;
    }

    public event EventHandler BloonPopped;
    public event EventHandler<NextBloonEventArgs> NextBloon;

    protected virtual void OnBloonPopped()
    {
        AudioSource.PlayClipAtPoint(popClip, transform.position);
        if (next != null)
        {
            GameObject go = Instantiate(next, transform.position, transform.rotation);
            NextBloon?.Invoke(this, new NextBloonEventArgs { Next = go.GetComponent<Bloon>() });
        }
        else {
            //Debug.Log("invoke next");
            BloonPopped?.Invoke(this, EventArgs.Empty);
        }
        popped = true;
        Destroy(this.gameObject);
        _player.AddMoney(1);
        
    }

    public class NextBloonEventArgs : EventArgs
    {
        public Bloon Next { get; set; }
    }

}
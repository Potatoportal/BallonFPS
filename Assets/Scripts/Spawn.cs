using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private GameObject[] bloons;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject SpawnBloon(int health) {
        Vector3 position = transform.position;

        return Instantiate(bloons[health], new Vector3(position.x + UnityEngine.Random.Range(-5,5),position.y, position.z + UnityEngine.Random.Range(-5, 5)), Quaternion.identity);
    }


}


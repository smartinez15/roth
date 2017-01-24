using UnityEngine;
using System.Collections;

public class Finish : MonoBehaviour {

    public GameObject fireworks;

	void OnTriggerEnter(Collider other)
    {
        Instantiate(fireworks, transform.position, Quaternion.identity);
    }
}

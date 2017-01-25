using UnityEngine;
using System.Collections;

public class PlayerControler : MonoBehaviour {

    public float speed;
    public float jumpForce;

    private Rigidbody rb;
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	
	void Update ()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0.0f, vertical)*speed*Time.deltaTime;

        rb.MovePosition(transform.position + movement);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up*jumpForce, ForceMode.Impulse);
        }

    }
}

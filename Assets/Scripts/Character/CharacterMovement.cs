using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{

    #region Component Variables
    Rigidbody rigidBody;
    Animator anim;
    CapsuleCollider capCol;
    [SerializeField]
    PhysicMaterial zFriction;
    [SerializeField]
    PhysicMaterial mFriction;
    Transform cam;
    #endregion

    [SerializeField]
    float speed = 4;
    [SerializeField]
    float turnSpeed = 5;
    [SerializeField]
    float jumPower = 5;

    Vector3 directionPos;
    Vector3 storeDir;

    float horizontal;
    float vertical;
    bool jumpInput;
    bool onGround;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        capCol = GetComponent<CapsuleCollider>();
        SetupAnimator();
    }

    void Update()
    {
        HandleFriction();
    }

    void FixedUpdate()
    {
        //Input
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        jumpInput = Input.GetButtonDown("Jump");
        bool walkToggle = Input.GetKey(KeyCode.LeftShift);

        float walkMultiplier = 1;

        if (walkToggle)
        {
            walkMultiplier = 1;
        }
        else
        {
            walkMultiplier = 0.5f;
        }

        //if (horizontal == 0)
        //{
        storeDir = cam.right;
        //}

        if (onGround)
        {
            rigidBody.AddForce(((storeDir * horizontal) + (cam.forward * vertical)) * speed / Time.deltaTime);

            if (jumpInput)
            {
                anim.SetTrigger("Jump");
                rigidBody.AddForce(Vector3.up * jumPower, ForceMode.Impulse);
            }
        }

        directionPos = transform.position + (storeDir * horizontal) + (cam.forward * vertical);

        Vector3 dir = directionPos - transform.position;
        dir.y = 0;

        float animValue = Mathf.Abs(vertical) + Mathf.Abs(horizontal);

        anim.SetFloat("Forward", animValue * walkMultiplier, 0.1f, Time.deltaTime);

        if (horizontal != 0 || vertical != 0)
        {
            float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(dir));

            if (angle != 0)
                rigidBody.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        //TODO Check if it's the ground
        onGround = true;
        rigidBody.drag = 5;
        anim.SetBool("onAir", false);
    }

    void OnCollisionExit(Collision other)
    {
        onGround = false;
        rigidBody.drag = 0;
        anim.SetBool("onAir", true);
    }

    void SetupAnimator()
    {
        anim = GetComponent<Animator>();
        foreach (var child in GetComponentsInChildren<Animator>())
        {
            if (child != anim)
            {
                anim.avatar = child.avatar;
                Destroy(child);
                break;
            }
        }
    }

    void HandleFriction()
    {
        if (horizontal == 0 && vertical == 0)
        {
            capCol.material = mFriction;
        }
        else
        {
            capCol.material = zFriction;
        }
    }

}

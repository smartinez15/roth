using UnityEngine;
using System.Collections;

public class ActiveBlock : MonoBehaviour
{

    public bool active;
    public float space;
    public Cube cube;
    public GameObject wall;
    public Material material;

    private bool player;
    private bool initial;
    private Vector3 initialPos;
    private ParticleController wallUp, wallDown, wallLeft, wallRight;
    private Renderer rend;

    void Start()
    {
        wallUp = wallDown = wallLeft = wallRight = null;
    }

    public void setProperties(Vector3 pos, bool active)
    {
        this.active = active;
        if(active)
        {
            rend = GetComponent<Renderer>();
            rend.sharedMaterial = material;
        }
        initialPos = pos;
        initial = true;
        transform.localPosition = new Vector3(pos.x, -10f, pos.z);
    }

    public bool isOnTop()
    {
        return transform.localPosition.y > space;
    }

    public bool isPlayer()
    {
        return player;
    }

    public Vector3 getPos()
    {
        return transform.localPosition;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = true;
            cube.checkPath();
        }
    }

    void Update()
    {
        if(initial)
        {
            float currentPos = transform.localPosition.y;

            float pos = Mathf.MoveTowards(currentPos, initialPos.y, Time.deltaTime * 20);
            transform.localPosition = new Vector3(initialPos.x, pos, initialPos.z);
            if(pos-currentPos==0.0f)
            {
                initial = false;
            }
        }
        else if (isPlayer())
        {
            cube.checkPath();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = false;
            if (wallUp != null)
            {
                wallUp.kill();
                wallUp = null;
            }
            if (wallDown != null)
            {
                wallDown.kill();
                wallDown = null;
            }
            if (wallLeft != null)
            {
                wallLeft.kill();
                wallLeft = null;
            }
            if (wallRight != null)
            {
                wallRight.kill();
                wallRight = null;
            }
        }
    }

    public void putBounds(bool up, bool down, bool left, bool right)
    {
        if (!active)
        {
            return;
        }
        Quaternion spawnRotation = Quaternion.identity;
        Vector3 spawnPosition = new Vector3(0, 1, 0);
        if (!up)
        {
            if (wallUp == null)
            {
                spawnPosition.x = 0.0f;
                spawnPosition.z = 0.55f;
                spawnRotation = Quaternion.identity;
                GameObject ob = Instantiate(wall, spawnPosition, spawnRotation) as GameObject;
                ob.transform.parent = transform;
                ob.transform.localPosition = spawnPosition;
                wallUp = ob.GetComponent<ParticleController>();

            }
        }
        else
        {
            if (wallUp != null)
            {
                wallUp.kill();
                wallUp = null;
            }
        }
        if (!down)
        {
            if (wallDown == null)
            {
                spawnPosition.x = 0.0f;
                spawnPosition.z = -0.55f;
                spawnRotation = Quaternion.identity;
                GameObject ob = Instantiate(wall, spawnPosition, spawnRotation) as GameObject;
                ob.transform.parent = transform;
                ob.transform.localPosition = spawnPosition;
                wallDown = ob.GetComponent<ParticleController>();
            }
        }
        else
        {
            if (wallDown != null)
            {
                wallDown.kill();
                wallDown = null;
            }
        }
        if (!left)
        {
            if (wallLeft == null)
            {
                spawnPosition.x = -0.55f;
                spawnPosition.z = 0.0f;
                spawnRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                GameObject ob = Instantiate(wall, spawnPosition, spawnRotation) as GameObject;
                ob.transform.parent = transform;
                ob.transform.localPosition = spawnPosition;
                wallLeft = ob.GetComponent<ParticleController>();
            }
        }
        else
        {
            if (wallLeft != null)
            {
                wallLeft.kill();
                wallLeft = null;
            }
        }
        if (!right)
        {
            if (wallRight == null)
            {
                spawnPosition.x = 0.55f;
                spawnPosition.z = 0.0f;
                spawnRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                GameObject ob = Instantiate(wall, spawnPosition, spawnRotation) as GameObject;
                ob.transform.parent = transform;
                ob.transform.localPosition = spawnPosition;
                wallRight = ob.GetComponent<ParticleController>();
            }
        }
        else
        {
            if (wallRight != null)
            {
                wallRight.kill();
                wallRight = null;
            }
        }
    }
}

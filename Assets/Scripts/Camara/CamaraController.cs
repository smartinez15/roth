using UnityEngine;
using System.Collections;

public class CamaraController : MonoBehaviour
{
    public GameObject player;


    private float smoothSize, smoothPos;
    private Camera camera;
    private Vector3 offset;
    private int count;
    private bool playerMode, changing, posNo;
    private float targetSize;
    private Vector3 targetPos;

    // Use this for initialization
    void Start()
    {
        offset = transform.position - player.transform.position;
        playerMode = true;
        changing = false;
        camera = GetComponent<Camera>();
        count = 0;
    }

    public void changeMode()
    {
        playerMode = !playerMode;
        changing = true;
    }

    public void changeScope(float s)
    {
        targetSize = s;
        smoothSize = Mathf.Abs((camera.orthographicSize - s) / 1.5f);
    }

    public void ancle(Transform t)
    {
        count++;
        if (t != null)
        {
            targetPos = t.position;
            smoothPos = Mathf.Abs((transform.position.x - t.position.x) / 1.5f);

        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (changing)
        {
            float currentSize = camera.orthographicSize;
            float size = Mathf.MoveTowards(currentSize, targetSize, Time.deltaTime * smoothSize);
            camera.orthographicSize = size;
            float deltaSize = size - currentSize;

            Vector3 deltaPos = Vector3.zero;
            if (count != 2)
            {
                Vector3 currentPos = transform.position;
                Vector3 pos = Vector3.MoveTowards(currentPos, targetPos, Time.deltaTime * smoothPos);
                transform.position = pos;
                deltaPos = pos - currentPos;
            }

            if (deltaSize == 0.0f && deltaPos == Vector3.zero)
            {
                changing = false;
            }
        }
        if (playerMode && player != null)
            transform.position = player.transform.position + offset;
    }
}

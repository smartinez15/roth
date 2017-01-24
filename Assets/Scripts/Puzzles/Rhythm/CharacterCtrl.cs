using UnityEngine;
using System.Collections;

public class CharacterCtrl : MonoBehaviour
{
    private static float degree = 30f;
    private KeyCode left;
    private KeyCode right;
    private KeyCode down;

    private Vector3 pos;
    private Vector3 posOg;
    private Vector3 scale;
    private Quaternion rot;
    private char but;
    private bool running;
    private Vector3 velocity;

    public Transform character;
    public float speed;

    void Start()
    {
        Vector3 posi = transform.position;
        if (degree == 30)
        {
            transform.position = new Vector3(posi.x, Mathf.Sqrt(2 + Mathf.Sqrt(3)) * 2, posi.z);
        }
        posOg = pos = new Vector3(0, 0.7f - transform.position.y, 0);
        scale = character.localScale;
        rot = Quaternion.identity;

        left = KeyCode.A;
        right = KeyCode.D;
        down = KeyCode.S;
        but = '0';
        running = false;
    }

    void FixedUpdate()
    {
        if (Input.GetKey(left) || Input.GetKey(right))
        {
            if (Input.GetKey(left) && Input.GetKey(right))
            {
                switch (but)
                {
                    case 'L':
                        rot = Quaternion.Euler(0, 0, degree);
                        break;
                    case 'R':
                        rot = Quaternion.Euler(0, 0, -degree);
                        break;
                    case '0':
                        rot = Quaternion.Euler(0, 0, 180);
                        break;
                }
            }
            else if (Input.GetKey(left) && !Input.GetKey(right))
            {
                rot = Quaternion.Euler(0, 0, -degree);
                but = 'L';
            }
            else if (!Input.GetKey(left) && Input.GetKey(right))
            {
                rot = Quaternion.Euler(0, 0, degree);
                but = 'R';
            }
        }
        else
        {
            rot = Quaternion.identity;
            but = '0';
        }


        if (Input.GetKey(down))
        {
            pos = new Vector3(posOg.x, posOg.y - 0.5f, posOg.z);
            scale = new Vector3(1.8f, 0.2f, 1f);
        }
        else
        {
            pos = posOg; new Vector3(0, -1.75f, 0);
            scale = new Vector3(1, 1, 1);
        }

        float step = speed * velocity.z * Time.deltaTime;

        transform.rotation = Quaternion.Slerp(transform.rotation, rot, step);
        character.localPosition = Vector3.Lerp(character.localPosition, pos, step);
        character.localScale = Vector3.Lerp(character.localScale, scale, step * 1.2f);

        if (running)
        {
            transform.position += velocity * Time.deltaTime;
        }
    }

    public void Run(float beatTime)
    {
        running = true;
        velocity = new Vector3(0, 0, 2f / beatTime);
    }
}

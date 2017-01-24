using UnityEngine;
using System.Collections;

public class Bar : MonoBehaviour
{
    public int bar;
    public IBeat[] beats;

    private int status = 0; //0 - Inactive, 1 - Rising, 2 - Ready
    private float beatTime;
    private Vector3 pos;

    void Start()
    {
        pos = transform.localPosition;
        transform.localPosition = new Vector3(pos.x, pos.y - 20, pos.z);
    }

    public void Beat(int pBar, int beat)
    {
        if (pBar == (bar - 2))
        {
            if (beat == 1)
            {
                status = 1;
                for (int i = 0; i < beats.Length; i++)
                {
                    beats[i].Activate(beatTime);
                }
            }
            for (int i = 0; i < beats.Length; i++)
            {
                beats[i].Pulse();
            }
        }
        else if (pBar == (bar - 1))
        {
            if (beat == beats.Length)
            {
                beats[0].Warning();
            }
            for (int i = 0; i < beats.Length; i++)
            {
                beats[i].Pulse();
            }
        }
        else if (pBar == bar)
        {
            status = 2;
            if (beat != beats.Length)
                beats[beat].Warning();
            beats[beat - 1].Action();
            for (int i = 0; i < beats.Length; i++)
            {
                beats[i].Pulse();
            }
        }
    }

    void Update()
    {
        if (status == 1)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, pos, 1.5f / beatTime * Time.deltaTime);
        }
    }

    public void setBeatSpeed(float speed)
    {
        beatTime = speed;
    }
}

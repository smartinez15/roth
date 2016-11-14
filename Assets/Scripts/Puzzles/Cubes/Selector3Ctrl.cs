using UnityEngine;
using System.Collections;

public class Selector3Ctrl : MonoBehaviour {


    public Cube cube;
    private int state; //1-Active, 2-Moving
    private char axis;
    private Vector2 playerPos;

	void Start ()
    {
        state = 1;
        axis = 'L';
        transform.localPosition = new Vector3(-1.1f, 0f, 0f);
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void setPlayerPos(Vector2 pos)
    {
        playerPos = pos;
    }

    public void setCube(Cube c)
    {
        cube = c;
        state = 1;
    }
	
	void Update ()
    {
	    if(state==1)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                if(axis=='W' || axis == 'D' || axis == 'U')
                {
                    cube.changeRotation(axis, true);
                }
                else
                {
                    switch(axis)
                    {
                        case 'L':
                            axis = 'M';
                            transform.localPosition = new Vector3(0f, 0f, 0f);
                            break;
                        case 'M':
                            axis = 'R';
                            transform.localPosition = new Vector3(1.1f, 0f, 0f);
                            break;
                        case 'R':
                            axis = 'F';
                            transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
                            transform.localPosition = new Vector3(0f, 0f, -1.1f);
                            break;
                        case 'F':
                            axis = 'N';
                            transform.localPosition = new Vector3(0f, 0f, 0f);
                            break;
                        case 'N':
                            axis = 'B';
                            transform.localPosition = new Vector3(0f, 0f, 1.1f);
                            break;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (axis == 'W' || axis == 'D' || axis == 'U')
                {
                    cube.changeRotation(axis, false);
                }
                else
                {
                    switch (axis)
                    {
                        case 'B':
                            axis = 'N';
                            transform.localPosition = new Vector3(0f, 0f, 0f);
                            break;
                        case 'N':
                            axis = 'F';
                            transform.localPosition = new Vector3(0f, 0f, -1.1f);
                            break;
                        case 'F':
                            axis = 'R';
                            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                            transform.localPosition = new Vector3(1.1f, 0f, 0f);
                            break;
                        case 'R':
                            axis = 'M';
                            transform.localPosition = new Vector3(0f, 0f, 0f);
                            break;
                        case 'M':
                            axis = 'L';
                            transform.localPosition = new Vector3(-1.1f, 0f, 0f);
                            break;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                if (axis == 'R' || axis == 'M' || axis == 'L' || axis == 'F' || axis == 'N' || axis == 'B')
                {
                    cube.changeRotation(axis, false);
                }
                else
                {
                    switch (axis)
                    {
                        case 'D':
                            axis = 'W';
                            transform.localPosition = new Vector3(0f, 0f, 0f);
                            break;
                        case 'W':
                            axis = 'U';
                            transform.localPosition = new Vector3(0f, 1.1f, 0f);
                            break;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (axis == 'R' || axis == 'M' || axis == 'L' || axis == 'F' || axis == 'N' || axis == 'B')
                {
                    cube.changeRotation(axis, true);
                }
                else
                {
                    switch (axis)
                    {
                        case 'U':
                            axis = 'W';
                            transform.localPosition = new Vector3(0f, 0f, 0f);
                            break;
                        case 'W':
                            axis = 'D';
                            transform.localPosition = new Vector3(0f, -1.1f, 0f);
                            break;
                    }
                }
            }
            else if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(axis == 'L' || axis=='B')
                {
                    axis = 'D';
                    transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
                    transform.localPosition = new Vector3(0f, -1.1f, 0f);
                }
                else if (axis == 'N' || axis == 'M')
                {
                    axis = 'W';
                    transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
                    transform.localPosition = new Vector3(0f, 0f, 0f);
                }
                else if (axis == 'R' || axis == 'F')
                {
                    axis = 'U';
                    transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
                    transform.localPosition = new Vector3(0f, 1.1f, 0f);
                }
                else
                {
                    switch (axis)
                    {
                        case 'U':
                            axis = 'R';
                            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                            transform.localPosition = new Vector3(1.1f, 0f, 0f);
                            break;
                        case 'W':
                            axis = 'M';
                            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                            transform.localPosition = new Vector3(0f, 0f, 0f);
                            break;
                        case 'D':
                            axis = 'L';
                            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                            transform.localPosition = new Vector3(-1.1f, 0f, 0f);
                            break;
                    }
                }
            }
        }
        else if(state==2)
        {

        }
	}
}

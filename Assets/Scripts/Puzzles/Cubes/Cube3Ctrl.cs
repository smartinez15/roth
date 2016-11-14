using UnityEngine;
using System.Collections;

public class Cube3Ctrl : Cube {

    public float rotationSpeed;
    public Transform[] centers, corners, edges, rotationPoints;
    public Transform core;
    public ActiveBlock[] blocks;
    public bool manual;

    private Vector2 playerPos;
    private string playerLayers;
    private bool[,] path;

    private const float space = 1.0f;

    private int state; //0 - Inactive, 1 - Active, 2 - Rotating
    private char ax;
    private bool reverse;
    private Transform[] currentRotating;
    private Transform current;

    void Start()
    {
        state = 1;
        reverse = false;
        ax = 'R';
        currentRotating = new Transform[9];

        playerPos.x = -1;
        playerPos.y = -1;
        playerLayers = "0:0";

        path = new bool[3, 3];

        checkPath();
    }

    void Update()
    {
        if (state == 2)
        {
            float angle = (reverse ? -90.0f : 90.0f);

            float deltaAngle = 10.0f;
            var rotationVector = current.rotation.eulerAngles;
            if (ax == 'R' || ax == 'L' || ax == 'M') //Rotacion en X
            {
                deltaAngle = rotationVector.x;
                rotationVector.x = Mathf.MoveTowardsAngle(rotationVector.x, angle, Time.deltaTime * rotationSpeed);
                deltaAngle -= rotationVector.x;
            }
            else if (ax == 'U' || ax == 'D' || ax == 'W') //Rotacion en Y
            {
                deltaAngle = rotationVector.y;
                rotationVector.y = Mathf.MoveTowardsAngle(rotationVector.y, angle, Time.deltaTime * rotationSpeed);
                deltaAngle -= rotationVector.y;
            }
            else if (ax == 'B' || ax == 'F' || ax == 'N') //Rotacion en z
            {
                deltaAngle = rotationVector.z;
                rotationVector.z = Mathf.MoveTowardsAngle(rotationVector.z, angle, Time.deltaTime * rotationSpeed);
                deltaAngle -= rotationVector.z;
            }
            current.rotation = Quaternion.Euler(rotationVector);
            if (deltaAngle == 0f || deltaAngle == 360f)
            {
                for (int i = 0; i < currentRotating.Length; i++)
                {
                    //currentRotating[i].rotation = Quaternion.identity;
                    currentRotating[i].SetParent(transform, true);
                }
                current.rotation = Quaternion.identity;
                reverse = false;
                state=1;
            }
        }
        else if(manual)
        {
            reverse = Input.GetKey(KeyCode.LeftShift);

            if (Input.GetKeyDown(KeyCode.R))
            {
                changeRotation('R', reverse);
            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                changeRotation('U', reverse);
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                changeRotation('B', reverse);
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                changeRotation('L', reverse);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                changeRotation('D', reverse);
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                changeRotation('F', reverse);
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                changeRotation('M', reverse);
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                changeRotation('N', reverse);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                changeRotation('W', reverse);
            }
        }
    }

    public override void changeRotation(char axis, bool rev)
    {
        ax = axis;
        checkPath();
        reverse = rev;

        if (state == 1 && playerLayers != "0:0" && axis != '0' && !playerLayers.Contains("" + axis))
        {
            int j = 0;
            char a = '0';

            if (axis == 'U' || axis == 'R' || axis == 'B')
            {
                switch (axis)
                {
                    case 'R':
                        a = 'x';
                        break;
                    case 'U':
                        a = 'y';
                        break;
                    case 'B':
                        a = 'z';
                        break;
                }

                bool found = false;
                for (int i = 0; i < rotationPoints.Length && !found; i++)
                {
                    if (getPositionAxis(rotationPoints[i], a) > space)
                    {
                        current = rotationPoints[i];
                        found = true;
                    }
                }

                for (int i = 0; i < centers.Length; i++)
                {
                    if (getPositionAxis(centers[i], a) > space)
                    {
                        centers[i].SetParent(current, true);
                        currentRotating[j] = centers[i];
                        j++;
                    }
                }

                for (int i = 0; i < corners.Length; i++)
                {
                    if (getPositionAxis(corners[i], a) > space)
                    {
                        corners[i].SetParent(current, true);
                        currentRotating[j] = corners[i];
                        j++;
                    }
                }

                for (int i = 0; i < edges.Length; i++)
                {
                    if (getPositionAxis(edges[i], a) > space)
                    {
                        edges[i].SetParent(current, true);
                        currentRotating[j] = edges[i];
                        j++;
                    }
                }
            }
            else if (axis == 'L' || axis == 'F' || axis == 'D')
            {
                switch (axis)
                {
                    case 'L':
                        a = 'x';
                        break;
                    case 'D':
                        a = 'y';
                        break;
                    case 'F':
                        a = 'z';
                        break;
                }

                bool found = false;
                for (int i = 0; i < rotationPoints.Length && !found; i++)
                {
                    if (getPositionAxis(rotationPoints[i], a) < -space)
                    {
                        current = rotationPoints[i];
                        found = true;
                    }
                }

                for (int i = 0; i < centers.Length; i++)
                {
                    if (getPositionAxis(centers[i], a) < -space)
                    {
                        centers[i].SetParent(current, true);
                        currentRotating[j] = centers[i];
                        j++;
                    }
                }

                for (int i = 0; i < corners.Length; i++)
                {
                    if (getPositionAxis(corners[i], a) < -space)
                    {
                        corners[i].SetParent(current, true);
                        currentRotating[j] = corners[i];
                        j++;
                    }
                }

                for (int i = 0; i < edges.Length; i++)
                {
                    if (getPositionAxis(edges[i], a) < -space)
                    {
                        edges[i].SetParent(current, true);
                        currentRotating[j] = edges[i];
                        j++;
                    }
                }
            }
            else if (axis == 'M' || axis == 'N' || axis == 'W')
            {
                switch (axis)
                {
                    case 'M':
                        a = 'x';
                        break;
                    case 'W':
                        a = 'y';
                        break;
                    case 'N':
                        a = 'z';
                        break;
                }

                bool found = false;
                for (int i = 0; i < rotationPoints.Length && !found; i++)
                {
                    if (rotationPoints[i].localPosition.x == 0.0f && rotationPoints[i].localPosition.y == 0.0f && rotationPoints[i].localPosition.z == 0.0f)
                    {
                        current = rotationPoints[i];
                        found = true;
                    }
                }

                for (int i = 0; i < centers.Length; i++)
                {
                    if (getPositionAxis(centers[i], a) < 0.1f && getPositionAxis(centers[i], a) > -0.1f)
                    {
                        centers[i].SetParent(current, true);
                        currentRotating[j] = centers[i];
                        j++;
                    }
                }

                for (int i = 0; i < corners.Length; i++)
                {
                    if (getPositionAxis(corners[i], a) < 0.1f && getPositionAxis(corners[i], a) > -0.1f)
                    {
                        corners[i].SetParent(current, true);
                        currentRotating[j] = corners[i];
                        j++;
                    }
                }

                for (int i = 0; i < edges.Length; i++)
                {
                    if (getPositionAxis(edges[i], a) < 0.1f && getPositionAxis(edges[i], a) > -0.1f)
                    {
                        edges[i].SetParent(current, true);
                        currentRotating[j] = edges[i];
                        j++;
                    }
                }

                currentRotating[j] = core;
                j++;
            }
            if (j == 9)
            {
                state=2;
            }
        }
    }

    float getPositionAxis(Transform t, char a)
    {
        switch (a)
        {
            case 'x':
                return t.localPosition.x;
            case 'y':
                return t.localPosition.y;
            case 'z':
                return t.localPosition.z;
        }
        return 0.0f;
    }

    public override void checkPath()
    {
        int count = 0;
        bool playerFound = false;
        int boundX = -1;
        int boundZ = -1;
        ActiveBlock current = null;
        for (int i = 0; i < blocks.Length; i++)
        {
            int x = -1;
            int z = -1;

            if (blocks[i].isOnTop())
            {
                count++;
                float posX = blocks[i].getPos().x;
                if (posX < -space)
                {
                    x = 0;
                }
                else if (posX > -0.1f && posX < 0.1f)
                {
                    x = 1;
                }
                else if (posX > space)
                {
                    x = 2;
                }

                float posZ = blocks[i].getPos().z;
                if (posZ < -space)
                {
                    z = 2;
                }
                else if (posZ > -0.1f && posZ < 0.1f)
                {
                    z = 1;
                }
                else if (posZ > space)
                {
                    z = 0;
                }

                if (x >= 0 && z >= 0)
                {
                    path[x, z] = blocks[i].active;
                }

                if (blocks[i].isPlayer())
                {
                    playerFound = true;
                    playerPos.x = x;
                    playerPos.y = z;
                    char f = '0';
                    char s = '0';
                    switch (x)
                    {
                        case 0:
                            f = 'L';
                            break;
                        case 1:
                            f = 'M';
                            break;
                        case 2:
                            f = 'R';
                            break;
                    }
                    switch (z)
                    {
                        case 0:
                            s = 'B';
                            break;
                        case 1:
                            s = 'N';
                            break;
                        case 2:
                            s = 'F';
                            break;
                    }

                    playerLayers = "" + f + ":" + s;
                    boundX = x;
                    boundZ = z;
                    current = blocks[i];
                }
            }
        }
        if (!playerFound)
        {
            playerLayers = "0:0";
            playerPos.x = -1;
            playerPos.y = -1;
        }
        else
        {
            setBounds(boundX, boundZ, current);
        }
    }

    void setBounds(int x, int z, ActiveBlock b)
    {
        bool up, down, left, right;

        up = (z == 0) ? true : (path[x, (z - 1)]);

        down = (z == 2) ? true : (path[x, (z + 1)]);

        left = (x == 0) ? true : (path[(x - 1), z]);

        right = (x == 2) ? true : (path[(x + 1), z]);

        b.putBounds(up, down, left, right);
    }
}

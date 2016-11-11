using UnityEngine;
using System.Collections;
using System;

public class Cube3: Cube
{
    public float rotationSpeed;
    public GameObject[] templates;
    public Vector3[] positions;
    public Transform[] rotationPoints;

    private int iBlock, iTurn;
    private int iCenters, iCorners, iEdges, iActiveBlocks;
    private Transform[] centers, corners, edges;
    private Transform core;
    private ActiveBlock[] blocks;

    private Vector2 playerPos;
    private string playerLayers;
    private bool[,] path;
    private bool initial;
    private Vector3 entry;
    private const float space = 1.0f;

    private int state; //-1 - Initial, 0 - Inactive, 1 - Selecting, 2 - rotating, 3 - deselecting
    private float select;
    private char ax;
    private bool reverse;
    private Transform[] currentRotating;
    private Transform current;

    void Start()
    {
        state = 0;
        select = 1.5f;
        reverse = false;
        initial = true;
        ax = 'R';

        centers = new Transform[6];
        corners = new Transform[8];
        edges = new Transform[12];
        rotationPoints = new Transform[7];
        currentRotating = new Transform[9];
        blocks = new ActiveBlock[26];
        iTurn = 1;
        iBlock = 0;
        iCenters = iCorners = iEdges = iActiveBlocks = 0;

        playerPos.x = -1;
        playerPos.y = -1;
        playerLayers = "0:0";

        path = new bool[3, 3];

    }

    IEnumerator create()
    {
        state = -1;

        while (iBlock != 27)
        {
            for (int i = 0; i < iTurn && iBlock<27; i++)
            {
                Vector3 pos = positions[iBlock];
                iBlock++;
                int number = UnityEngine.Random.Range(0, templates.Length);
                GameObject b = templates[number];
                GameObject block = (GameObject)Instantiate(b);
                block.transform.SetParent(transform, true);
                ActiveBlock ab = block.GetComponent<ActiveBlock>();
                if (pos == entry)
                {
                    ab.setProperties(pos, true);
                }
                else if (UnityEngine.Random.value > 0.8f)
                {
                    ab.setProperties(pos, true);
                }
                else
                {
                    ab.setProperties(pos, false);
                }

                if (pos == Vector3.zero)
                {
                    core = block.transform;
                }
                else if (pos.x != 0.0f && pos.y != 0.0f && pos.z != 0.0f)
                {
                    corners[iCorners] = block.transform;
                    blocks[iActiveBlocks] = ab;
                    iCorners++;
                    iActiveBlocks++;
                }
                else if ((pos.x == 0.0f && pos.y != 0.0f && pos.z != 0.0f) || (pos.x != 0.0f && pos.y == 0.0f && pos.z != 0.0f) || (pos.x != 0.0f && pos.y != 0.0f && pos.z == 0.0f))
                {
                    edges[iEdges] = block.transform;
                    blocks[iActiveBlocks] = ab;
                    iEdges++;
                    iActiveBlocks++;
                }
                else if ((pos.x != 0.0f && pos.y == 0.0f && pos.z == 0.0f) || (pos.x == 0.0f && pos.y != 0.0f && pos.z == 0.0f) || (pos.x == 0.0f && pos.y == 0.0f && pos.z != 0.0f))
                {
                    centers[iCenters] = block.transform;
                    blocks[iActiveBlocks] = ab;
                    iCenters++;
                    iActiveBlocks++;
                }
            }

            if (iBlock == 6 || iBlock == 12 || iBlock == 18)
            {
                iTurn++;
            }

            yield return new WaitForSeconds(0.5f);
        }
        state = 0;
        initial = false;
    }

    void selecting(ref float pos, ref float deltaPos)
    {
        float currentPos = 0.0f;
        char a = '0';
        if (ax == 'R' || ax == 'L' || ax == 'M') //Rotacion en X
        {
            a = 'y';
        }
        else if (ax == 'U' || ax == 'D' || ax == 'W') //Rotacion en Y
        {
            a = 'z';
        }
        else if (ax == 'B' || ax == 'F' || ax == 'N') //Rotacion en Z
        {
            a = 'x';
        }

        currentPos = getPositionAxis(currentRotating[1], a);
        if (currentPos < 0)
            currentPos *= -1;

        pos = Mathf.MoveTowards(currentPos, select, Time.deltaTime * 1.4f);
        deltaPos = pos - currentPos;
    }

    void Update()
    {
        if (state == 1 || state == 3)
        {
            float deltaPos = 0.0f;
            float pos = 0.0f;
            selecting(ref pos, ref deltaPos);
            if (ax == 'R' || ax == 'L' || ax == 'M') //Rotacion en X
            {
                for (int i = 0; i < currentRotating.Length; i++)
                {
                    float y = pos;
                    float z = pos;
                    if (currentRotating[i].localPosition.y > -0.5f && currentRotating[i].localPosition.y < 0.5f)
                    {
                        y = 0;
                    }
                    else if (currentRotating[i].localPosition.y < 0)
                    {
                        y = -1 * y;
                    }
                    if (currentRotating[i].localPosition.z > -0.5f && currentRotating[i].localPosition.z < 0.5f)
                    {
                        z = 0;
                    }
                    else if (currentRotating[i].localPosition.z < 0)
                    {
                        z = -1 * z;
                    }
                    currentRotating[i].localPosition = new Vector3(currentRotating[i].localPosition.x, y, z);
                }
            }
            else if (ax == 'U' || ax == 'D' || ax == 'W') //Rotacion en Y
            {
                for (int i = 0; i < currentRotating.Length; i++)
                {
                    float x = pos;
                    float z = pos;
                    if (currentRotating[i].localPosition.x > -0.5f && currentRotating[i].localPosition.x < 0.5f)
                    {
                        x = 0;
                    }
                    else if (currentRotating[i].localPosition.x < 0)
                    {
                        x = -1 * x;
                    }
                    if (currentRotating[i].localPosition.z > -0.5f && currentRotating[i].localPosition.z < 0.5f)
                    {
                        z = 0;
                    }
                    else if (currentRotating[i].localPosition.z < 0)
                    {
                        z = -1 * z;
                    }
                    currentRotating[i].localPosition = new Vector3(x, currentRotating[i].localPosition.y, z);
                }
            }
            else if (ax == 'B' || ax == 'F' || ax == 'N') //Rotacion en Z
            {
                for (int i = 0; i < currentRotating.Length; i++)
                {
                    float x = pos;
                    float y = pos;
                    if (currentRotating[i].localPosition.y > -0.5f && currentRotating[i].localPosition.y < 0.5f)
                    {
                        y = 0;
                    }
                    else if (currentRotating[i].localPosition.y < 0)
                    {
                        y = -1 * y;
                    }
                    if (currentRotating[i].localPosition.x > -0.5f && currentRotating[i].localPosition.x < 0.5f)
                    {
                        x = 0;
                    }
                    else if (currentRotating[i].localPosition.x < 0)
                    {
                        x = -1 * x;
                    }
                    currentRotating[i].localPosition = new Vector3(x, y, currentRotating[i].localPosition.z);
                }
            }
            if (deltaPos == 0.0f)
            {
                state++;
                select = 1.1f;
                if (state > 3)
                {
                    state = 0;
                    for (int i = 0; i < currentRotating.Length; i++)
                    {
                        currentRotating[i].rotation = Quaternion.identity;
                        currentRotating[i].SetParent(transform, true);
                    }
                    current.rotation = Quaternion.identity;
                    select = 1.5f;
                }
            }
        }
        if (state == 2)
        {
            float angle = (reverse ? -90.0f : 90.0f);

            float deltaAngle = 0.0f;
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
            for (int i = 0; i < currentRotating.Length; i++)
            {
                currentRotating[i].rotation = Quaternion.identity;
            }
            if (deltaAngle == 0.0f)
            {
                reverse = false;
                state++;
            }
        }
        else
        {
            reverse = Input.GetKey(KeyCode.LeftShift);

            if (Input.GetKeyDown(KeyCode.R))
            {
                ax = 'R';
                changeRotation(ax, reverse);
            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                ax = 'U';
                changeRotation(ax, reverse);
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                ax = 'B';
                changeRotation(ax, reverse);
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                ax = 'L';
                changeRotation(ax, reverse);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                ax = 'D';
                changeRotation(ax, reverse);
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                ax = 'F';
                changeRotation(ax, reverse);
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                ax = 'M';
                changeRotation(ax, reverse);
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                ax = 'N';
                changeRotation(ax, reverse);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                ax = 'W';
                changeRotation(ax, reverse);
            }
            else if(Input.GetKeyDown(KeyCode.C))
            {
                entry = new Vector3(1.1f, 1.1f, 1.1f);
                StartCoroutine("create");
            }
        }
    }

    public override void changeRotation(char axis, bool rev)
    {
        checkPath();

        if (!initial && state == 0 && axis != '0' && !playerLayers.Contains("" + axis))
        {
            int j = 0;
            char a = '0';
            ax = axis;
            reverse = rev;

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
                state++;
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
        for (int i = 0; i < blocks.Length && !initial; i++)
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
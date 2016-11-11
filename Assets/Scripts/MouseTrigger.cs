using UnityEngine;
using System.Collections;

public class MouseTrigger : MonoBehaviour
{
    public char up, down, leftUp, leftDown, rightUp, rightDown;
    public Cube cube;
    public float max, min;

    private bool reverse;
    private Vector3 mouseP;

    void OnMouseDown()
    {
        mouseP = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        char axis = '0';
        Vector3 delta = Input.mousePosition - mouseP;
        if (delta.y > max && (delta.x < min && delta.x > -min))
        {
            axis = (up);
        }
        else if (delta.y < -max && (delta.x < min && delta.x > -min))
        {
            axis = (down);
        }
        else if (delta.x > max && (delta.y < (min * 10) && delta.y > 0))
        {
            axis = (rightUp);
        }
        else if (delta.x > max && (delta.y > -(min * 10) && delta.y < 0))
        {
            axis = (rightDown);
        }
        else if (delta.x < -max && (delta.y < (min * 10) && delta.y > 0))
        {
            axis = (leftUp);
        }
        else if (delta.x < -max && (delta.y > -(min * 10) && delta.y < 0))
        {
            axis = (leftDown);
        }
        if (axis != 0)
        {
            if (char.IsUpper(axis))
            {
                reverse = false;
            }
            else
            {
                reverse = true;
                axis = char.ToUpper(axis);
            }

            cube.changeRotation(axis, reverse);
        }
    }
}

using UnityEngine;
using System.Collections;

public class Selector : MonoBehaviour
{
    public char axis;
    public Cube cube;
    private bool mouseOver = false;

    void Update()
    {
        if(mouseOver)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                cube.changeRotation(axis, false);
            else if (Input.GetKeyDown(KeyCode.Mouse1))
                cube.changeRotation(axis, true);
        }
    }

    void OnMouseEnter()
    {
        mouseOver = true;
        cube.select(axis);
    }

    void OnMouseExit()
    {
        mouseOver = false;
        cube.deselect();
    }
}

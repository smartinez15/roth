using UnityEngine;
using System.Collections;

abstract public class Cube : MonoBehaviour
{
    abstract public void select(char axis);

    abstract public void deselect();

    abstract public void changeRotation(char axis, bool reverse);

    abstract public void checkPath();
}

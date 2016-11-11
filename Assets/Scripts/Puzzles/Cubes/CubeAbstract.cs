using UnityEngine;
using System.Collections;

abstract public class Cube : MonoBehaviour
{
    abstract public void changeRotation(char axis, bool reverse);

    abstract public void checkPath();
}

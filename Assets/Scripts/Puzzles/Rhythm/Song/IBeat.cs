using UnityEngine;
using System.Collections;

abstract public class IBeat : MonoBehaviour
{
    abstract public void Activate(float beatSpeed);

    abstract public void Pulse();

    abstract public void Warning();

    abstract public void Action();

    abstract public void Deactivate();
}

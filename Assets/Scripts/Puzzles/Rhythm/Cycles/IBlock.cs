using UnityEngine;
using System.Collections;

abstract public class IBlock : MonoBehaviour
{
    abstract public void Warning(float warningTime);

    abstract public void Action(float actionTime);

    abstract public void Retreat();

    abstract public void pulse(int cycle);

    abstract public void setCycleSpeed(float speed);
}

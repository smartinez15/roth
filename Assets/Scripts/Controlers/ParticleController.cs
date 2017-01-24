using UnityEngine;
using System.Collections;

public class ParticleController : MonoBehaviour
{

    public ParticleSystem ps;
    public float lifeTime;

    public void kill()
    {
        ps.Stop();
        Destroy(gameObject, lifeTime);
    }
}

using UnityEngine;
using System.Collections;

public class TriggerController : MonoBehaviour {

    private GameController gc;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(gc==null)
            {
                GameObject controller = GameObject.FindWithTag("GameController");
                if (controller != null)
                {
                    gc = controller.GetComponent<GameController>();
                }
                else
                {
                    Debug.Log("Cannot find 'GameController' script");
                }
            }
            gc.action();
            Destroy(gameObject);
        }
    }
}

using UnityEngine;
using System.Collections;

public class DestroyByBounds : MonoBehaviour
{ 
    void OnTriggerExit(Collider other)
    {  
        if(other.gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
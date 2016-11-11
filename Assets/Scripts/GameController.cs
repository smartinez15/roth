using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public GameObject trigger;
    public CamaraController camera;
    public Transform[] camaraPositions, triggerPositions;
    public float[] camaraSize;

    private int currentCamaraPos, currentTriggerPos, currentCamaraSize;

    void Start()
    {
        currentCamaraPos = currentTriggerPos = 0;
      //  Instantiate(trigger, triggerPositions[currentTriggerPos].position, Quaternion.identity);
        currentTriggerPos++;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void action()
    {
        if (currentCamaraPos < camaraPositions.Length)
            camera.ancle(camaraPositions[currentCamaraPos]);
        if (currentCamaraSize < camaraSize.Length)
            camera.changeScope(camaraSize[currentCamaraSize]);
        camera.changeMode();
        currentCamaraPos++;
        currentCamaraSize++;

        if (currentTriggerPos < triggerPositions.Length)
         //Instantiate(trigger, triggerPositions[currentTriggerPos].position, Quaternion.identity);
        currentTriggerPos++;
    }
}

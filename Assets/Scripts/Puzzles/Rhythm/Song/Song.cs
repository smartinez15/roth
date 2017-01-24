using UnityEngine;
using System.Collections;

public class Song : MonoBehaviour
{
    #region Song Options
    public int bpm;
    public int metric;
    public Bar[] bars;
    public bool autoRun;
    #endregion
    public CharacterCtrl character;

    private bool running;
    private float clock;
    private float beatTime;
    private int bar;
    private int beat;
    private int globalBeat;

    void Start()
    {
        running = autoRun;
        clock = 0;
        bar = -2;
        beat = globalBeat = 1;
        beatTime = (60f / bpm);
        for (int i = 0; i < bars.Length; i++)
        {
            bars[i].setBeatSpeed(beatTime);
        }
    }

    public void Run()
    {
        running = true;
    }

    void Update()
    {
        if (running)
        {
            float nBeatTime = (60f / bpm);
            if (nBeatTime != beatTime)
            {
                beatTime = nBeatTime;
                for (int i = 0; i < bars.Length; i++)
                {
                    bars[i].setBeatSpeed(beatTime);
                }
            }

            clock += Time.deltaTime;
            if (clock > globalBeat * beatTime)
            {
                globalBeat++;
                beat++;
                if (beat > metric)
                {
                    beat = 1;
                    bar++;
                    if (bar == 0)
                    {
                        character.Run(beatTime);
                    }
                }
                Debug.Log("Bar: " + bar + " Beat: " + beat + " GlobalBeat: " + globalBeat);
                for (int i = 0; i < bars.Length; i++)
                {
                    bars[i].Beat(bar, beat);
                }
            }
        }
    }
}

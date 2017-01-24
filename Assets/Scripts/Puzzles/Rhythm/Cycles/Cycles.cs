using UnityEngine;
using System.Collections;

public class Cycles : MonoBehaviour
{
    #region Cycles Options
    public int bpm;
    public int cycles;
    #endregion
    #region Action options
    public Vector2[] actions;
    public float actionProbability;
    public bool singleProbability;
    #endregion
    #region Warning Options
    public float warningTime;
    #endregion
    public bool autoRun;

    public IBlock[] blocks;

    private bool running;
    private float clock;
    private float cycleTime;
    private int cycle = 0;
    private bool actionGlobal;
    private bool[] action;
    private bool[] alertSent;
    private int iAction;
    private int actionCycle;
    private int retreatCycle;

    void Start()
    {
        running = autoRun;
        clock = 0;
        cycle = 1;
        cycleTime = (60f / bpm);
        alertSent = new bool[blocks.Length];
        action = new bool[blocks.Length];
        iAction = 0;
        actionCycle = (int)actions[iAction].x;
        retreatCycle = (int)actions[iAction].y;
        if (singleProbability)
        {
            for (int i = 0; i < action.Length; i++)
            {
                float n = Random.value;
                action[i] = (n <= actionProbability);
                blocks[i].setCycleSpeed(cycleTime);
            }
        }
        else
        {
            float n = Random.value;
            actionGlobal = (n <= actionProbability);
            for (int i = 0; i < action.Length; i++)
            {
                blocks[i].setCycleSpeed(cycleTime);
            }
        }
    }

    public void Run()
    {
        running = true;
    }

    void Update()
    {
        if(running)
        {
            cycleTime = (60f / bpm);
            clock += Time.deltaTime;
            if (clock > cycle * cycleTime)
            {
                cycle++;
                if (cycle > cycles)
                {
                    clock = 0;
                    cycle = 1;
                }
                for (int i = 0; i < blocks.Length; i++)
                {
                    blocks[i].pulse(cycle);
                }
            }

            checkAlert();
            char act = '0';
            if (actionCycle == cycle)
            {
                act = 'a';
            }
            else if (retreatCycle == cycle)
            {
                act = 'r';
            }

            switch (act)
            {
                case 'a':
                    for (int i = 0; i < blocks.Length; i++)
                    {
                        if ((!singleProbability && actionGlobal) || action[i])
                            blocks[i].Action(cycleTime);
                    }

                    break;
                case 'r':
                    for (int i = 0; i < blocks.Length; i++)
                    {
                        if ((!singleProbability && actionGlobal) || action[i])
                        {
                            blocks[i].Retreat();
                            alertSent[i] = false;
                        }
                    }
                    if (singleProbability)
                    {
                        for (int i = 0; i < action.Length; i++)
                        {
                            float n = Random.value;
                            action[i] = (n <= actionProbability);
                        }
                    }
                    else
                    {
                        float n = Random.value;
                        actionGlobal = (n <= actionProbability);
                    }
                    iAction++;
                    if (iAction == actions.Length)
                        iAction = 0;
                    actionCycle = (int)actions[iAction].x;
                    retreatCycle = (int)actions[iAction].y;
                    break;
            }
        }
    }

    void checkAlert()
    {
        float nextCycle = (cycle < actionCycle) ? (((actionCycle - 1) * cycleTime) - clock) : (((actionCycle - 1) * cycleTime) + (cycles * cycleTime - clock));
        if (nextCycle < warningTime)
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                if ((!singleProbability && actionGlobal) || action[i])
                {
                    if (!alertSent[i])
                    {
                        blocks[i].Warning(warningTime);
                        alertSent[i] = true;
                    }
                }

            }
        }
    }
}

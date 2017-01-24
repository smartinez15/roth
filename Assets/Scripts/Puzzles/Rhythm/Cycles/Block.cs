using UnityEngine;
using System.Collections;
using System;

public class Block : IBlock
{
    Vector3 pos;

    GameObject alert;
    GameObject pul;
    Vector3 alertScale;

    #region Action options
    public float actionLength;
    public float retreatLength;
    public bool characterGuard;
    public bool pulses;
    #endregion
    #region Warning Options
    public Material alertMat;
    public Material pulseMat;
    #endregion

    private int status = 0; //0- Neutral, 1 - OnAlert, 2 - Action, 3 - Retreat
    private float warningSpeed;
    private float actionSpeed;
    private float retreatSpeed;
    private float actionSize;
    private bool character;
    private bool guard;

    void Start()
    {
        createAlert();
        status = 0;
        actionSize = 2;
        pos = transform.position;
        character = false;
    }

    void createAlert()
    {
        alert = GameObject.CreatePrimitive(PrimitiveType.Quad);
        alert.GetComponent<MeshRenderer>().material = alertMat;
        alert.transform.rotation = Quaternion.Euler(90, 0, 0);
        alert.transform.SetParent(transform);
        alert.transform.localPosition = new Vector3(0, 0.52f, 0);
        alert.transform.localScale = new Vector3(0, 0, 0);
        Destroy(alert.GetComponent<MeshCollider>());

        pul = GameObject.CreatePrimitive(PrimitiveType.Quad);
        pul.GetComponent<MeshRenderer>().material = pulseMat;
        pul.transform.rotation = Quaternion.Euler(90, 0, 0);
        pul.transform.SetParent(transform);
        pul.transform.localPosition = new Vector3(0, 0.51f, 0);
        pul.transform.localScale = new Vector3(0, 0, 0);
        Destroy(pul.GetComponent<MeshCollider>());
    }

    void FixedUpdate()
    {
        pul.transform.localScale = Vector3.MoveTowards(pul.transform.localScale, Vector3.zero, actionSpeed * 2f * Time.deltaTime);

        if (status == 1)
        {
            alert.transform.localScale = Vector3.MoveTowards(alert.transform.localScale, alertScale, warningSpeed * Time.deltaTime);
        }
        else if (status == 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, actionSpeed * actionSize * Time.deltaTime);
        }
        if (status == 3)
        {
            float delta = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, pos, retreatSpeed * actionSize * Time.deltaTime);
            delta -= transform.position.y;
            if (delta == 0)
            {
                status = 0;
                transform.position = pos;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (status == 0 && other.gameObject.CompareTag("Player"))
        {
            character = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            character = false;
        }
    }

    public override void Warning(float warningTime)
    {
        if (status == 0)
        {
            status = 1;
            alertScale = new Vector3(1f, 1f, 1f);

            warningSpeed = 2f / warningTime;
        }
    }

    public override void Action(float actionTime)
    {
        if (status == 1 || status == 0)
        {
            status = 2;
            alert.transform.localScale = new Vector3(0, 0, 0);

            if (!characterGuard || !character)
            {
                pos.y += actionSize;
            }
            else
            {
                guard = true;
            }

            actionSpeed = 1 / (actionTime * actionLength);
            retreatSpeed = 1 / (actionTime * retreatLength);
        }
    }

    public override void Retreat()
    {
        if (status == 2)
        {
            status = 3;
            if (guard)
            {
                guard = false;
            }
            else
            {
                pos.y -= actionSize;
            }
        }
    }

    public override void pulse(int cycle)
    {
        if (pulses)
        {
            pul.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public override void setCycleSpeed(float speed)
    {
        actionSpeed = 1 / (speed * actionLength);
        retreatSpeed = 1 / (speed * retreatLength);
    }
}

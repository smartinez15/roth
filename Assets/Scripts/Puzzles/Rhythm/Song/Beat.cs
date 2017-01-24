using UnityEngine;
using System.Collections;
using System;

public class Beat : IBeat
{
    #region Hazard Setup
    public Transform leftHazard;
    public Transform centerHazard;
    public Transform rightHazard;
    #endregion
    #region Obstacle Selection
    public bool leftDown;
    //public bool leftUp;
    public bool centerDown;
    //public bool centerUp;
    public bool rightDown;
    //public bool rightUp;
    #endregion

    private int status; // 0 - Inactive, 1 - Active, 2 - Warning, 3 - Hazard
    private GameObject[] alert;
    public Material alertMat;
    private GameObject[] pul;
    public Material pulseMat;
    private float beatTime;

    void Start()
    {
        createAlert();
    }

    void createAlert()
    {
        alert = new GameObject[3];
        pul = new GameObject[2];

        for (int i = 0; i < alert.Length; i++)
        {
            int angle = (i == 0) ? 60 : (i == 1) ? 90 : 120;
            Vector3 posi = Vector3.zero;
            switch (i)
            {
                case 0:
                    posi = new Vector3(2, 0.65f, 0);
                    break;
                case 1:
                    posi = new Vector3(0, 0.05f, 0);
                    break;
                case 2:
                    posi = new Vector3(-2, 0.65f, 0);
                    break;
            }
            alert[i] = GameObject.CreatePrimitive(PrimitiveType.Quad);
            alert[i].GetComponent<MeshRenderer>().material = alertMat;
            alert[i].transform.rotation = Quaternion.Euler(angle, 90, 90);
            alert[i].transform.SetParent(transform);
            alert[i].transform.localPosition = posi;
            alert[i].transform.localScale = new Vector3(0, 0, 0);
            Destroy(alert[i].GetComponent<MeshCollider>());
        }

        for (int i = 0; i < pul.Length; i++)
        {
            pul[i] = GameObject.CreatePrimitive(PrimitiveType.Quad);
            pul[i].GetComponent<MeshRenderer>().material = pulseMat;
            pul[i].transform.rotation = Quaternion.Euler(0, (i == 0) ? 90 : -90, 0);
            pul[i].transform.SetParent(transform);
            pul[i].transform.localPosition = new Vector3((i == 0) ? 2.95f : -2.95f, 2.1f, 0);
            pul[i].transform.localScale = new Vector3(0, 0, 0);
            Destroy(pul[i].GetComponent<MeshCollider>());
        }
    }

    void Update()
    {
        if (status != 0 && status != 3)
        {
            for (int i = 0; i < pul.Length; i++)
            {
                pul[i].transform.localScale = Vector3.MoveTowards(pul[i].transform.localScale, Vector3.zero, beatTime * 2f * Time.deltaTime);
            }
        }
        if (status == 2)
        {
            if (leftDown)
            {
                alert[0].transform.localScale = Vector3.MoveTowards(alert[0].transform.localScale, new Vector3(1.5f, 1.5f, 1.5f), 2f / beatTime * Time.deltaTime);
            }
            if (centerDown)
            {
                alert[1].transform.localScale = Vector3.MoveTowards(alert[1].transform.localScale, new Vector3(1.5f, 1.5f, 1.5f), 2f / beatTime * Time.deltaTime);
            }
            if (rightDown)
            {
                alert[2].transform.localScale = Vector3.MoveTowards(alert[2].transform.localScale, new Vector3(1.5f, 1.5f, 1.5f), 2f / beatTime * Time.deltaTime);
            }
        }
        if (status == 3)
        {
            if (leftDown)
            {
                leftHazard.localPosition = Vector3.Lerp(leftHazard.localPosition, new Vector3(2, 1, 0), beatTime * 10 * Time.deltaTime);
            }
            if (centerDown)
            {
                centerHazard.localPosition = Vector3.Lerp(centerHazard.localPosition, new Vector3(0, 1, 0), beatTime * 10 * Time.deltaTime);
            }
            if (rightDown)
            {
                rightHazard.localPosition = Vector3.Lerp(rightHazard.localPosition, new Vector3(-2, 1, 0), beatTime * 10 * Time.deltaTime);
            }
        }
    }

    public override void Activate(float beatSpeed)
    {
        beatTime = beatSpeed;
        status = 1;
    }

    public override void Pulse()
    {
        for (int i = 0; i < pul.Length; i++)
        {
            pul[i].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }

    public override void Action()
    {
        status = 3;
    }

    public override void Warning()
    {
        status = 2;
    }

    public override void Deactivate()
    {
        status = 0;
    }
}

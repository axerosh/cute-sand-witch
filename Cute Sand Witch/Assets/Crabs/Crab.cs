using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    public float bobAmp;

    public float bobFreq;

    public float maxPhaseOffset;

    private float bobPhase;

    public Transform model;

    public void SetBobPhase(float phase)
    {
        bobPhase = phase * maxPhaseOffset;
    }

    // Update is called once per frame
    void Update()
    {
        model.transform.localPosition = new Vector3(0, Mathf.Abs(bobAmp * Mathf.Sin(bobFreq * Time.realtimeSinceStartup + bobPhase)), 0);
    }
}

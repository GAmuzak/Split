using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disolver : MonoBehaviour
{
    private Renderer cloneRenderer;
    private bool isAnimOngoing;
    private float targetVal, currentVal, time;
    private static readonly int ScaleValue = Shader.PropertyToID("_scaleValue");
    private static readonly int Enabled = Shader.PropertyToID("_enabled");

    private void Start()
    {
        cloneRenderer = transform.GetChild(0).GetComponent<Renderer>();
    }

    private void Update()
    {
        if (!isAnimOngoing) return;
        currentVal = Mathf.Lerp(currentVal, targetVal, time);
        cloneRenderer.material.SetFloat(ScaleValue, currentVal);
        if (!Mathf.Approximately(currentVal, targetVal)) return;
        isAnimOngoing = false;
        cloneRenderer.material.SetInt(Enabled, 0);
    }

    public void Dissolve(float _targetVal, float _time)
    {
        currentVal = 1 - _targetVal;
        targetVal = _targetVal;
        time = _time;
        // cloneRenderer.material.SetFloat(Enabled, 1);
        isAnimOngoing = true;
    }
}

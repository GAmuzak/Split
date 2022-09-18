using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disolver : MonoBehaviour
{
    [SerializeField]private Renderer cloneRenderer;
    [Range(0,1)][SerializeField] private float currentVal;
    
    
    private bool isAnimOngoing;
    private float targetVal, time;
    private float currentTime;
    private static readonly int ScaleValue = Shader.PropertyToID("_scaleValue");
    private static readonly int Enabled = Shader.PropertyToID("_enabled");

    private void Start()
    {
        // cloneRenderer = transform.GetChild(0).GetComponent<Renderer>();
    }

    private void Update()
    {
        // if (!isAnimOngoing) return;
        // currentTime += Time.deltaTime;
        // currentVal = Mathf.Lerp(currentVal, targetVal, currentTime/time);
        cloneRenderer.material.SetFloat(ScaleValue, currentVal);
        // Debug.Log(currentVal);
        // if (!Mathf.Approximately(currentVal, targetVal)) return;
        // cloneRenderer.material.SetInt(Enabled, 1);
        // currentTime = 0;
        // isAnimOngoing = false;
    }

    public void Dissolve(float _targetVal, float _time)
    {
        currentVal = 1 - _targetVal;
        targetVal = _targetVal;
        time = 100*_time;
        cloneRenderer.material.SetInt(Enabled, 0);
        isAnimOngoing = true;
    }
}

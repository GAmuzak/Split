using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    [SerializeField] private GameObject circleBoi;
    [SerializeField] private float loadTime=1.2f;
    
    private Vector3 originalScale;
    private void Start()
    {
        originalScale = circleBoi.transform.localScale;
        LeanTween.scale(circleBoi, Vector3.zero, loadTime).setEase(LeanTweenType.easeInOutExpo);
    }

    private void OnEnable()
    {
        Goal.EndLevel += EndLevel;
    }

    private void OnDisable()
    {
        Goal.EndLevel -= EndLevel;
    }

    private void EndLevel()
    {
        LeanTween.scale(circleBoi, originalScale, loadTime).setEase(LeanTweenType.easeInOutExpo);
    }
    
    
}

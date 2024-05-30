using System;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float maxValue;
    public float startValue;
    public float regenRate;
    public float decayRate;
    public Image uiBar;
    public Image uiBg;
    public Color color;

    void Start()
    {
        curValue = startValue;
        color = uiBg.color;
    }
     
    void Update()
    {
        uiBar.fillAmount = GetPercentage();

        if (GetPercentage() < 1f)
        {
            uiBg.color = color;
            uiBar.color = color;
        }
        else
        {
            uiBg.color = Color.clear;
            uiBar.color = Color.clear;
        }
    }

    private float GetPercentage()
    {
        return curValue / maxValue;
    }

    public void Add(float amount)
    {   
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }
}

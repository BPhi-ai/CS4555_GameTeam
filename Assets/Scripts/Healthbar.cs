using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider slider;
    public string entityName;

    public void SetMaxHealth(long val)
    {
        slider.maxValue = val;
    }

    public void SetHealth(long val)
    {
        slider.value = val;
    }
}

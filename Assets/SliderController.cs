using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public BrewingStation brewingStation;
    public Slider slider;

    void Update()
    {
        if (brewingStation != null && slider != null)
        {
            slider.normalizedValue = brewingStation.BrewingProgress;
        }
    }
}

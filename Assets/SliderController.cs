using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ITaskProgress
{
    float Progress { get; }
}

public class SliderController : MonoBehaviour
{
    public ITaskProgress progress;
    public GameObject trackedGameobject;
    public Slider slider;

    private void Awake()
    {
        if (trackedGameobject != null)
            progress = trackedGameobject.GetComponent<ITaskProgress>();
    }

    void Update()
    {
        if (progress != null && slider != null)
        {
            slider.normalizedValue = progress.Progress;
        }
    }
}

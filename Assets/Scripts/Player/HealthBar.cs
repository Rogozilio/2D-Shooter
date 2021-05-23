using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public float Health { get => slider.value; set => slider.value = value; }

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
}

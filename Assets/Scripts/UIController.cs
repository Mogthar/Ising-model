using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] Slider temperatureSlider;
    [SerializeField] TMP_Text avgSpinText;
    [SerializeField] TMP_Text avgTempText;

    [Header("Element settings")]
    [SerializeField] Vector2 tempSliderEndPoints;
    Simulation simulation;

    void Awake() 
    {
        simulation = FindObjectOfType<Simulation>();
    }
    // Start is called before the first frame update
    void Start()
    {
        temperatureSlider.value = simulation.temperature;
        temperatureSlider.minValue = tempSliderEndPoints.x;
        temperatureSlider.maxValue = tempSliderEndPoints.y;
        temperatureSlider.value = simulation.temperature;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateGraphics();
    }

    void UpdateGraphics()
    {
        Vector3 averageMoment = simulation.CalculateAverageMoment();
        float averageTemperature = simulation.CalculateAverageTemperature();
        avgSpinText.text = "Average moment: " + averageMoment.y.ToString("0.00");
        avgTempText.text = "Average temperature: " + averageTemperature.ToString("0.00");
    }
}

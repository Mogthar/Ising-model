using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Simulation : MonoBehaviour
{
    [SerializeField] float _temperature;
    [SerializeField] int _latticeSize;
    [SerializeField] Spin _spinPrefab;

    [SerializeField] TMP_Text _avgSpinText;
    [SerializeField] TMP_Text _avgTempText;

    private Spin[,] _spinLattice;
    // Start is called before the first frame update
    private void Awake() 
    {
        _spinLattice = new Spin[_latticeSize, _latticeSize];
        InitializeSpins();   
    }
    // Update is called once per frame

    void FixedUpdate() {
        InteractSpins();
        ThermalizeSpins();
        UpdateGraphics();
    }

    void InitializeSpins()
    {
        for(int i = 0; i < _latticeSize; i++)
        {
            for(int j = 0; j < _latticeSize; j++)
            {
                _spinLattice[i,j] = Instantiate(_spinPrefab, new Vector3(- (float) _latticeSize / 2 + i, 0, - (float) _latticeSize / 2 + j), Random.rotation, transform);
            }
        }
    }

    void InteractSpins()
    {
        foreach(Spin s1 in _spinLattice)
        {
            foreach(Spin s2 in _spinLattice)
            {
                if(s1 != s2)
                {
                    s1.InteractWithSpin(s2);
                }
            }
        }
    }

    void UpdateGraphics()
    {
        Vector3 averageMoment = CalculateAverageMoment();
        float averageTemperature = CalculateAverageTemperature();
        _avgSpinText.text = "Average moment: " + averageMoment.y.ToString("0.00");
        _avgTempText.text = "Average temperature: " + averageTemperature.ToString("0.00");
    }

    Vector3 CalculateAverageMoment()
    {
        Vector3 totalMoment = new Vector3(0,0,0);
        foreach(Spin spin in _spinLattice)
        {
            totalMoment += spin.spinMagnitude * spin.transform.up;
        }
        return totalMoment /  (_latticeSize * _latticeSize);
    }

    float CalculateAverageTemperature()
    {
        float totalTemperature = 0.0f;
        foreach(Spin spin in _spinLattice)
        {
            Rigidbody rb = spin.GetComponent<Rigidbody>();
            Vector3 angularVelocity = rb.angularVelocity;
            totalTemperature += 0.5f * rb.mass * angularVelocity.sqrMagnitude;
        }
        return 2 * totalTemperature / (3 * _latticeSize * _latticeSize);
    }

    // Implement the langevine thermometer
    void ThermalizeSpins()
    {
        foreach(Spin spin in _spinLattice)
        {
            Rigidbody rb = spin.GetComponent<Rigidbody>();
            float sigma = Mathf.Sqrt(2 * rb.angularDrag * rb.mass * _temperature / Time.fixedDeltaTime);
            float xComponent = GaussianDistribution.getRandomValue(0, sigma);
            float yComponent = GaussianDistribution.getRandomValue(0, sigma);
            float zComponent = GaussianDistribution.getRandomValue(0, sigma);
            rb.AddTorque(new Vector3(xComponent, yComponent, zComponent));
        }
    }
}

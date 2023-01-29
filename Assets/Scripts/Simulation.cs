using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    public float temperature;
    public int nearestNeighbour;

    [SerializeField] InteractionType interaction = InteractionType.FERROMAGNETIC;
    [SerializeField] BoudaryCondition boundaryCondition = BoudaryCondition.HARDWALL;
    [SerializeField] int _latticeSize;
    [SerializeField] Spin _spinPrefab;

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
        for(int i = 0; i < _latticeSize; i++)
        {
            for(int j = 0; j < _latticeSize; j++)
            {
                Spin s1 = _spinLattice[i,j];
                for(int k = - nearestNeighbour; k < nearestNeighbour + 1; k++)
                {
                    for(int l = - nearestNeighbour; l < nearestNeighbour + 1; l++)
                    {
                        if(k != 0 || l != 0)
                        {
                            // hard wall BC
                            if(boundaryCondition == BoudaryCondition.HARDWALL)
                            {
                                int x_index = i + k;
                                int y_index = j + l;
                                if((0 <= x_index) && (x_index < _latticeSize) && (0 <= y_index) && ( y_index < _latticeSize))
                                {
                                    Spin s2 = _spinLattice[x_index, y_index];
                                    s1.InteractWithSpin(s2, s2.transform.position, interaction);
                                }
                            }
                            else if(boundaryCondition == BoudaryCondition.PERIODIC)
                            {
                                int x_index = (i + k + _latticeSize) % _latticeSize;
                                int y_index = (j + l + _latticeSize) % _latticeSize;
                                Spin s2 = _spinLattice[x_index, y_index];
                                Vector3 reflectedPosition = s2.transform.position;
                                // reflect spin x position if necessary
                                if(i+k >= _latticeSize)
                                {
                                    reflectedPosition.x += _latticeSize;
                                }
                                else if(i+k < 0)
                                {
                                    reflectedPosition.x -= _latticeSize;
                                }

                                // reflect spin y position if necessary
                                if(j+l >= _latticeSize)
                                {
                                    reflectedPosition.z += _latticeSize;
                                }
                                else if(j+l < 0)
                                {
                                    reflectedPosition.z -= _latticeSize;
                                }
                                
                                s1.InteractWithSpin(s2, reflectedPosition, interaction);
                            }
                        }
                    }
                }
            }
        }
    }

    public Vector3 CalculateAverageMoment()
    {
        Vector3 totalMoment = new Vector3(0,0,0);
        foreach(Spin spin in _spinLattice)
        {
            totalMoment += spin.spinMagnitude * spin.transform.up;
        }
        return totalMoment /  (_latticeSize * _latticeSize);
    }

    public float CalculateAverageTemperature()
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
            float sigma = Mathf.Sqrt(2 * rb.angularDrag * rb.mass * temperature / Time.fixedDeltaTime);
            float xComponent = GaussianDistribution.getRandomValue(0, sigma);
            float yComponent = GaussianDistribution.getRandomValue(0, sigma);
            float zComponent = GaussianDistribution.getRandomValue(0, sigma);
            rb.AddTorque(new Vector3(xComponent, yComponent, zComponent));
        }
    }

    public void SetTemperature(float newTemperature)
    {
        temperature = newTemperature;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GaussianDistribution
{
    public static float getRandomValue(float mean, float sigma)
    {
        float rand1 = Random.Range(0.0f, 1.0f);
        float rand2 = Random.Range(0.0f, 1.0f);

        float n = Mathf.Sqrt(-2.0f * Mathf.Log(rand1)) * Mathf.Cos((2.0f * Mathf.PI) * rand2);

        return (mean + sigma * n);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private float _lightRunTime = 0f;
    private float _lightFullTime = 30.0f;
    private float[] _lightBlinkTime = { 7.0f, 16.0f, 25.0f };

    void Update()
    {
        _lightRunTime += Time.deltaTime;

        if((_lightBlinkTime[0] <= _lightRunTime && _lightRunTime <=_lightBlinkTime[0] + 0.25f) ||
           (_lightBlinkTime[1] <= _lightRunTime && _lightRunTime <= _lightBlinkTime[1] + 0.25f) ||
           (_lightBlinkTime[2] <= _lightRunTime && _lightRunTime <= _lightBlinkTime[2] + 0.25f))
        {
            gameObject.GetComponent<Light>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<Light>().enabled = true;
        }

        if(_lightFullTime <= _lightRunTime)
        {
            _lightRunTime = 0;
        }
    }
}

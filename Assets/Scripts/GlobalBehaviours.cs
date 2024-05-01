using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBehaviours : MonoBehaviour
{
    // Start is called before the first frame update
    public float SpeedMultipliers = 1;
    public float deltaTime;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime = Time.deltaTime*100;
    }
}

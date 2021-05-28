using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{

    public float LifeSpan = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, LifeSpan);
    }

}

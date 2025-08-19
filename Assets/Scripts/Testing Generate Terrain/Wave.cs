using System;
using UnityEngine;

[Serializable]
public class Wave
{
    [field: SerializeField] public float Seed {get; private set;}
    [field: SerializeField] public float Frequency {get; private set;}
    [field: SerializeField] public float Amplitude { get; private set; }
}

using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class NaughtyComponent : MonoBehaviour
{
    [ProgressBar]
    [SerializeField]
    private float Health = 42;

    [ProgressBar("Couns found", 25, ProgressBarColor.Yellow)]
    public int CoinsFound = 3;

    private void Update()
    {
        Health += Time.deltaTime * 3;
    }
}

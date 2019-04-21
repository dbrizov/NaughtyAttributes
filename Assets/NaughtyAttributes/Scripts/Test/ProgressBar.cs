using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [NaughtyAttributes.ProgressBar("Health", 100, NaughtyAttributes.ProgressBarColor.Orange)]
    public float health = 50;
}

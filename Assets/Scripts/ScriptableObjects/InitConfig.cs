using UnityEngine;

[CreateAssetMenu(fileName = "Init config", menuName = "Configs/Init config")]
public class InitConfig : ScriptableObject
{
    public float BallSpeedMin;
    public float BallSpeedMax;

    public float BallScaleMin;
    public float BallScaleMax;
}

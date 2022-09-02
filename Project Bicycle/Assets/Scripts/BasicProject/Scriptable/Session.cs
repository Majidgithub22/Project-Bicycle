using UnityEngine;

[CreateAssetMenu(fileName = "Session", menuName = "ScriptableObject/Session")]
public class Session : ScriptableObject {
    public bool mode;
    public int levelNo;
    public int carNumber;
    public int totalLevel;
    public int unlocklevels;
}
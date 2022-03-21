using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Movement Speed Data", menuName = "Scriptable Objects/Movement Speed Data")]
public class MovementSpeedData : ScriptableObject
{
    public enum Mode
    {
        SLOW,
        FAST
    }

    [System.Serializable]
    private class SerializedModeToSpeedEntry
    {
        [SerializeField] private Mode movementMode;
        [SerializeField] private float speed;

        public Mode MovementMode { get => movementMode; }
        public float Speed { get => speed; }
    }

    [SerializeField] private List<SerializedModeToSpeedEntry> modesSpeedData = new List<SerializedModeToSpeedEntry>();

    public Dictionary<Mode, float> GetModeToSpeedDictionary()
    {
        Dictionary<Mode, float> modeToSpeedDictionary = new Dictionary<Mode, float>();
        foreach (SerializedModeToSpeedEntry entry in modesSpeedData)
        {
            modeToSpeedDictionary[entry.MovementMode] = entry.Speed;
        }

        return modeToSpeedDictionary;
    }
}

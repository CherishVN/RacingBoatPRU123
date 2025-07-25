using UnityEngine;

public class BoatLapCounter : MonoBehaviour
{

    public int TotalLaps = 2;
    public int CurrentLap { get; private set; } = 0;
    public bool passedEntryZone = false;

    public void IncreaseLap()
    {
        CurrentLap++;
        passedEntryZone = false;
        Debug.Log("Player " + GetComponent<BoatIdentity>().playerID + " lap: " + CurrentLap);
    }
}
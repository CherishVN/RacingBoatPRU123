using UnityEngine;

public class BoatLapCounter : MonoBehaviour
{
    
    public int TotalLaps = 2;
    public int CurrentLap { get; private set; } = 1;

    public void IncreaseLap()
    {
        CurrentLap++;
        Debug.Log("Player " + GetComponent<BoatIdentity>().playerID + " lap: " + CurrentLap);
    }
}

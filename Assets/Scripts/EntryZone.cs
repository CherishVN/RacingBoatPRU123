using UnityEngine;

public class EntryZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        BoatLapCounter lapCounter = other.GetComponent<BoatLapCounter>();
        if (lapCounter != null)
        {
            lapCounter.passedEntryZone = true;
            Debug.Log("[EntryZone] Player passed entry zone.");
        }
    }
}

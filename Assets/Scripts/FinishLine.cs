using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public int totalLapsToWin = 2;

   private void OnTriggerEnter(Collider other)
{
    Debug.Log("[FinishLine] Trigger entered by: " + other.name + " | Tag: " + other.tag);

    if (GameManager.Instance.raceEnded) return;

    BoatLapCounter lapCounter = other.GetComponent<BoatLapCounter>();
    BoatIdentity identity = other.GetComponent<BoatIdentity>();
    Rigidbody rb = other.GetComponent<Rigidbody>();

    if (lapCounter != null && identity != null)
{
    if (!lapCounter.passedEntryZone)
    {
        Debug.Log("[FinishLine] Player chưa qua EntryZone, không tính lap.");
        return;
    }

    

    lapCounter.IncreaseLap();
    Debug.Log("Player " + identity.playerID + " reached lap " + lapCounter.CurrentLap);

    if (lapCounter.CurrentLap == totalLapsToWin)
    {
        GameManager.Instance.DeclareWinner(identity.playerID);
    }
}
}

}

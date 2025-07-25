using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public int totalLapsToWin = 2;
    private bool gameEnded = false;

    private void OnTriggerEnter(Collider other)
    {
        if (gameEnded) return;

        BoatLapCounter lapCounter = other.GetComponent<BoatLapCounter>();
        BoatIdentity identity = other.GetComponent<BoatIdentity>();

        if (lapCounter != null && identity != null)
        {
            lapCounter.IncreaseLap(); // Tăng lap khi thuyền chạm finish line

            Debug.Log("Player " + identity.playerID + " reached lap " + lapCounter.CurrentLap);

            if (lapCounter.CurrentLap > totalLapsToWin)
            {
                // Đã vượt số vòng, bỏ qua
                return;
            }

            if (lapCounter.CurrentLap == totalLapsToWin)
            {
                gameEnded = true;

                int winnerID = identity.playerID;
                int loserID = (winnerID == 1) ? 2 : 1;

                Debug.Log("Player " + winnerID + " wins!");
                GameManager.Instance.DeclareWinner(winnerID);

                // Bạn có thể thêm hàm báo thua cho người chơi thua ở đây nếu cần
                // Ví dụ:
                // GameManager.Instance.DeclareLoser(loserID);
            }
        }
    }
}

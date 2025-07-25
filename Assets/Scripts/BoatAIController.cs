// BoatAIController.cs
using UnityEngine;

public class BoatAIController : MonoBehaviour
{
    private BoatMovementArrows boatMovement;

    [Header("AI Checkpoint Settings")]
    public Transform[] checkpoints; // Các checkpoint
    public float checkpointReachThreshold = 5f;
    private int currentCheckpointIndex = 0;

    [Header("Lap Settings")]
    public int totalLaps = 2;            // Tổng số vòng
    private int currentLap = 1;          // Bắt đầu từ vòng 1

    [Header("AI Control Settings")]
    public float steerThreshold = 10f;
    public float updatePathInterval = 0.3f;

    private float nextPathUpdate;
    private bool isAIEnabled = false;

    void Start()
    {
        boatMovement = GetComponent<BoatMovementArrows>();
    }

    public void EnableAI(bool enable)
    {
        isAIEnabled = enable;
        currentCheckpointIndex = 0;
        currentLap = 1;
        nextPathUpdate = Time.time;
    }

    void Update()
    {
        if (!isAIEnabled || checkpoints == null || checkpoints.Length == 0) return;

        if (Time.time >= nextPathUpdate)
        {
            UpdatePath();
            nextPathUpdate = Time.time + updatePathInterval;
        }
    }

    void UpdatePath()
    {
        Transform targetCheckpoint = checkpoints[currentCheckpointIndex];
        Vector3 directionToTarget = targetCheckpoint.position - transform.position;
        float distance = directionToTarget.magnitude;

        // Nếu đã đến gần checkpoint
        if (distance <= checkpointReachThreshold)
        {
            currentCheckpointIndex++;

            // Nếu đã qua hết checkpoint trong 1 vòng
            if (currentCheckpointIndex >= checkpoints.Length)
            {
                currentCheckpointIndex = 0;
                currentLap++;

                if (currentLap > totalLaps)
                {
                    Debug.Log("AI: Đã hoàn thành " + totalLaps + " vòng!");
                    if (!GameManager.Instance.raceEnded)
                        {
                            GameManager.Instance.DeclareWinner(2); 
                        }
                    isAIEnabled = false;
                    boatMovement.SimulateInput(false, false, false, false);
                    return;
                }

                Debug.Log("AI: Hoàn thành vòng " + (currentLap - 1) + ", bắt đầu vòng " + currentLap);
            }

            targetCheckpoint = checkpoints[currentCheckpointIndex];
            directionToTarget = targetCheckpoint.position - transform.position;
        }

        float angle = Vector3.SignedAngle(-transform.right, directionToTarget, Vector3.up);

        // Debug
        Debug.DrawRay(transform.position, -transform.right * 5f, Color.blue);
        Debug.DrawRay(transform.position, directionToTarget.normalized * 5f, Color.red);

        // Điều hướng AI: thêm vùng đi thẳng ổn định ±5 độ
        float absAngle = Mathf.Abs(angle);

        if (absAngle < 8f)
        {
            // Góc nhỏ: đi thẳng
            boatMovement.SimulateInput(true, false, false, false);
        }
        else if (absAngle >= 8f && absAngle < 15f)
        {
            // Góc lớn: rẽ mạnh
            if (angle > 0)
                boatMovement.SimulateInput(true, false, false, true); // Rẽ phải
            else
                boatMovement.SimulateInput(true, false, true, false); // Rẽ trái
        }
        else
        {
            // quay mạnh
    if (angle > 0)
        boatMovement.SimulateInput(true, false, false, true);
    else
        boatMovement.SimulateInput(true, false, true, false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (checkpoints == null) return;

        foreach (var cp in checkpoints)
        {
            if (cp != null)
                Gizmos.DrawWireSphere(cp.position, checkpointReachThreshold);
        }
    }
}

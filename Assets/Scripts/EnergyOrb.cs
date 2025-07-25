using UnityEngine;

public class EnergyOrb : MonoBehaviour
{
    [Header("Movement Settings")]
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;
    
    [Header("Rotation Settings")]
    public float rotationSpeed = 90f;

    public int scoreValue = 10;
    
    private Vector3 startPosition;
    private float timeOffset;

    void Start()
    {
        startPosition = transform.position;
        timeOffset = Random.Range(0f, 2f * Mathf.PI); // Để mỗi orb có phase khác nhau
    }

    void Update()
    {
        // Floating movement
        float newY = startPosition.y + floatAmplitude * Mathf.Sin((Time.time + timeOffset) * floatFrequency);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Rotation
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        BoatEnergySystem energySystem = other.GetComponent<BoatEnergySystem>();
        BoatIdentity identity = other.GetComponent<BoatIdentity>();
        if (energySystem != null)
        {
            energySystem.CollectEnergyOrb();
            // Cộng điểm nếu có danh tính thuyền
            if (identity != null && ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(identity.playerID, scoreValue);
            }
            // Có thể thêm hiệu ứng particle hoặc âm thanh ở đây trước khi destroy
            Destroy(gameObject);
        }
    }
}

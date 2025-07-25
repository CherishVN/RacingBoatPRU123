using UnityEngine;

public class BoatFloating : MonoBehaviour
{
    [Header("Buoyancy Settings")]
    public float buoyancyForce = 15f;
    public float waterLevel = 0f;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;
    
    [Header("Stability Settings")]
    public float stabilityForce = 30f;
    public float stabilityTorque = 30f;
    
    [Header("Wave Settings")]
    public bool enableWaves = true;
    public float waveHeight = 0.5f;
    public float waveSpeed = 1f;
    public float waveLength = 10f;
    
    [Header("Ocean Integration")]
    public Transform oceanTransform;
    
    [Header("Floating Points")]
    public Transform[] floatingPoints;
    
    private Rigidbody rb;
    private Vector3[] originalFloatingPoints;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        
        // Tìm Ocean tự động nếu chưa gán
        if (oceanTransform == null)
        {
            GameObject oceanObj = GameObject.Find("Ocean");
            if (oceanObj != null)
            {
                oceanTransform = oceanObj.transform;
                waterLevel = oceanTransform.position.y;
                Debug.Log("Đã tìm thấy Ocean tại Y: " + waterLevel);
            }
        }
        
        // Thiết lập các thuộc tính vật lý cho thuyền (chỉ nếu chưa được thiết lập)
        if (rb.mass == 1f) // Chỉ đặt mass nếu còn là giá trị mặc định
        {
            rb.mass = 500f; // Khối lượng thuyền nhẹ hơn
        }
        // rb.linearDamping = 0.5f;
        // rb.angularDamping = 0.5f;
        
        // Lưu vị trí gốc của các điểm nổi
        if (floatingPoints != null && floatingPoints.Length > 0)
        {
            originalFloatingPoints = new Vector3[floatingPoints.Length];
            for (int i = 0; i < floatingPoints.Length; i++)
            {
                if (floatingPoints[i] != null)
                {
                    originalFloatingPoints[i] = floatingPoints[i].localPosition;
                }
            }
        }
        else
        {
            // Tạo các điểm nổi mặc định nếu chưa có
            CreateDefaultFloatingPoints();
        }
    }
    
    void CreateDefaultFloatingPoints()
    {
        GameObject floatingPointsParent = new GameObject("FloatingPoints");
        floatingPointsParent.transform.SetParent(transform);
        floatingPointsParent.transform.localPosition = Vector3.zero;
        
        // Tạo 4 điểm nổi ở 4 góc của thuyền
        floatingPoints = new Transform[4];
        originalFloatingPoints = new Vector3[4];
        
        Vector3 size = Vector3.one * 2f; // Default size
        
        // Thử lấy bounds từ renderer
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            // Thử tìm renderer trong children
            renderer = GetComponentInChildren<Renderer>();
        }
        
        if (renderer != null && renderer.bounds.size.magnitude > 0.1f)
        {
            size = renderer.bounds.size;
        }
        else
        {
            Debug.LogWarning("Không tìm thấy Renderer, sử dụng kích thước mặc định cho floating points");
        }
        
        Vector3[] positions = new Vector3[]
        {
            new Vector3(-size.x/2, -size.y/2, size.z/2),   // Trước trái
            new Vector3(size.x/2, -size.y/2, size.z/2),    // Trước phải
            new Vector3(-size.x/2, -size.y/2, -size.z/2),  // Sau trái
            new Vector3(size.x/2, -size.y/2, -size.z/2)    // Sau phải
        };
        
        for (int i = 0; i < 4; i++)
        {
            GameObject point = new GameObject($"FloatingPoint_{i}");
            point.transform.SetParent(floatingPointsParent.transform);
            point.transform.localPosition = positions[i];
            floatingPoints[i] = point.transform;
            originalFloatingPoints[i] = positions[i];
        }
        
        Debug.Log($"Đã tạo {floatingPoints.Length} floating points cho {gameObject.name}");
    }
    
    void FixedUpdate()
    {
        ApplyBuoyancy();
        ApplyStability();
    }
    
    void ApplyBuoyancy()
    {
        if (floatingPoints == null) return;
        
        int pointsUnderWater = 0;
        
        for (int i = 0; i < floatingPoints.Length; i++)
        {
            if (floatingPoints[i] == null) continue;
            
            Vector3 worldPoint = floatingPoints[i].position;
            float currentWaterLevel = GetWaterLevelAtPosition(worldPoint);
            
            if (worldPoint.y < currentWaterLevel)
            {
                // Điểm này đang dưới nước
                pointsUnderWater++;
                
                float submersionDepth = currentWaterLevel - worldPoint.y;
                Vector3 buoyancyVector = Vector3.up * buoyancyForce * submersionDepth;
                
                // Áp dụng lực đẩy tại điểm này
                rb.AddForceAtPosition(buoyancyVector, worldPoint, ForceMode.Force);
                
                // Áp dụng lực cản nước
                Vector3 velocity = rb.GetPointVelocity(worldPoint);
                Vector3 dragForce = -velocity * waterDrag * submersionDepth;
                rb.AddForceAtPosition(dragForce, worldPoint, ForceMode.Force);
            }
        }
        
        // Áp dụng angular drag khi thuyền trong nước
        if (pointsUnderWater > 0)
        {
            rb.angularVelocity *= (1f - waterAngularDrag * Time.fixedDeltaTime);
        }
    }
    
    void ApplyStability()
    {
        // Lực ổn định để thuyền không bị lật
        Vector3 stabilityVector = Vector3.up - transform.up;
        rb.AddForce(stabilityVector * stabilityForce, ForceMode.Force);
        
        // Torque ổn định
        Vector3 stabilityTorqueVector = Vector3.Cross(transform.up, Vector3.up);
        rb.AddTorque(stabilityTorqueVector * stabilityTorque, ForceMode.Force);
    }
    
    float GetWaterLevelAtPosition(Vector3 position)
    {
        if (!enableWaves)
        {
            return waterLevel;
        }
        
        float baseLevel = waterLevel;
        
        if (oceanTransform != null)
        {
            baseLevel = oceanTransform.position.y;
        }
        
        float wave1 = Mathf.Sin((position.x / waveLength + Time.time * waveSpeed) * 2 * Mathf.PI) * waveHeight;
        float wave2 = Mathf.Sin((position.z / waveLength + Time.time * waveSpeed * 0.8f) * 2 * Mathf.PI) * waveHeight * 0.5f;
        
        return baseLevel + wave1 + wave2;
    }
    
    void OnDrawGizmos()
    {
        if (floatingPoints != null)
        {
            Gizmos.color = Color.cyan;
            foreach (Transform point in floatingPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawWireSphere(point.position, 0.1f);
                }
            }
        }
        
        Gizmos.color = Color.blue;
        Vector3 waterPlaneSize = new Vector3(5f, 0.02f, 5f);
        Gizmos.DrawWireCube(new Vector3(transform.position.x, waterLevel, transform.position.z), waterPlaneSize);
    }
} 
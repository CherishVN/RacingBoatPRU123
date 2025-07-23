using UnityEngine;

[System.Serializable]
public class BoatMovementArrows : MonoBehaviour
{
    [Header("Movement Settings")]
    public float motorForce = 1500f;
    public float steerForce = 20f;
    public float maxSteerAngle = 25f;
    
    [Header("Input Settings - Arrow Keys")]
    public KeyCode forwardKey = KeyCode.UpArrow;
    public KeyCode backwardKey = KeyCode.DownArrow;
    public KeyCode leftKey = KeyCode.LeftArrow;
    public KeyCode rightKey = KeyCode.RightArrow;
    
    [Header("Physics Settings")]
    public Transform motorPosition;
    public float waterResistance = 0.02f;
    
    private Rigidbody rb;
    private float motorInput;
    private float steerInput;
    
    // Thêm property để các script khác có thể truy cập
    public float MotorInput => motorInput;
    public float SteerInput => steerInput;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("BoatMovementArrows cần có Rigidbody component!");
            return;
        }
        rb.centerOfMass = new Vector3(0, -1.5f, 0);
        
        // Nếu không có vị trí motor, tạo một vị trí mặc định
        if (motorPosition == null)
        {
            GameObject motorPoint = new GameObject("MotorPosition");
            motorPoint.transform.SetParent(transform);
            motorPoint.transform.localPosition = new Vector3(0, 0, -1f); // Phía sau thuyền
            motorPosition = motorPoint.transform;
        }
    }
    
    void Update()
    {
        GetInput();
    }
    
    void FixedUpdate()
    {
        ApplyMotor();
        ApplySteering();
        ApplyWaterResistance();
    }
    
    void GetInput()
    {
        // CHỈ sử dụng các phím mũi tên được thiết lập
        motorInput = 0f;
        if (Input.GetKey(forwardKey))
            motorInput = 1f;
        else if (Input.GetKey(backwardKey))
            motorInput = -1f;
        
        steerInput = 0f;
        if (Input.GetKey(leftKey))
            steerInput = -1f;
        else if (Input.GetKey(rightKey))
            steerInput = 1f;
        
        // KHÔNG sử dụng Input.GetAxis() để tránh xung đột
    }
    
    void ApplyMotor()
    {
        if (Mathf.Abs(motorInput) > 0.1f)
        {
            // Sử dụng hướng của thuyền
            Vector3 force = -transform.right * motorInput * motorForce;
            rb.AddForce(force, ForceMode.Force);
        }
    }
    
    void ApplySteering()
    {
        if (Mathf.Abs(steerInput) > 0.1f && Mathf.Abs(motorInput) > 0.1f)
        {
            // Chỉ có thể quay khi thuyền đang di chuyển
            float steerAmount = steerInput * steerForce * Mathf.Abs(motorInput);
            Vector3 steerTorque = transform.up * steerAmount;
            rb.AddTorque(steerTorque, ForceMode.Force);
        }
    }
    
    void ApplyWaterResistance()
    {
        // Áp dụng lực cản tổng thể
        Vector3 velocity = rb.linearVelocity;
        Vector3 resistance = -velocity * velocity.magnitude * waterResistance;
        rb.AddForce(resistance, ForceMode.Force);
        
        // Áp dụng angular resistance
        Vector3 angularResistance = -rb.angularVelocity * waterResistance * 10f;
        rb.AddTorque(angularResistance, ForceMode.Force);
    }
    
    void OnDrawGizmos()
    {
        // Vẽ vị trí motor
        if (motorPosition != null)
        {
            Gizmos.color = Color.blue; // Màu khác để phân biệt với thuyền 1
            Gizmos.DrawWireSphere(motorPosition.position, 0.2f);
            
            // Vẽ hướng lực motor
            if (Application.isPlaying && Mathf.Abs(motorInput) > 0.1f)
            {
                Gizmos.color = Color.cyan;
                Vector3 forceDirection = transform.forward * motorInput * 2f;
                Gizmos.DrawRay(motorPosition.position, forceDirection);
            }
        }
    }
} 
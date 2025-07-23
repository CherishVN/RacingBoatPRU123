using UnityEngine;

[System.Serializable]
public class BoatController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float motorForce = 1500f;
    public float steerForce = 20f;
    public float maxSteerAngle = 25f;
    
    [Header("Input Settings")]
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode backwardKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    
    [Header("Physics Settings")]
    public Transform motorPosition;
    public float waterResistance = 0.02f;
    
    private Rigidbody rb;
    private float motorInput;
    private float steerInput;
    
    // Thêm property để PaddlerController có thể truy cập
    public float MotorInput => motorInput;
    public float SteerInput => steerInput;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("BoatController cần có Rigidbody component!");
            return;
        }
        
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
        // Lấy input cho motor
        motorInput = 0f;
        if (Input.GetKey(forwardKey))
            motorInput = 1f;
        else if (Input.GetKey(backwardKey))
            motorInput = -1f;
        
        // Lấy input cho steering
        steerInput = 0f;
        if (Input.GetKey(leftKey))
            steerInput = -1f;
        else if (Input.GetKey(rightKey))
            steerInput = 1f;
        
        // Hỗ trợ input từ Input System (nếu có)
        if (Input.GetAxis("Vertical") != 0)
            motorInput = Input.GetAxis("Vertical");
        if (Input.GetAxis("Horizontal") != 0)
            steerInput = Input.GetAxis("Horizontal");
    }
    
    void ApplyMotor()
    {
        if (Mathf.Abs(motorInput) > 0.1f)
        {
            Vector3 force = transform.forward * motorInput * motorForce;
            rb.AddForceAtPosition(force, motorPosition.position, ForceMode.Force);
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
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(motorPosition.position, 0.2f);
            
            // Vẽ hướng lực motor
            if (Application.isPlaying && Mathf.Abs(motorInput) > 0.1f)
            {
                Gizmos.color = Color.yellow;
                Vector3 forceDirection = transform.forward * motorInput * 2f;
                Gizmos.DrawRay(motorPosition.position, forceDirection);
            }
        }
    }
} 
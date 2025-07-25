using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target; // Thuyền cần theo
    public Vector3 offset = new Vector3(0, 3, -5); // Khoảng cách từ thuyền
    
    [Header("Smooth Settings")]
    public float followSpeed = 5f; // Tốc độ theo
    public float rotationSpeed = 3f; // Tốc độ xoay
    
    [Header("Look Settings")]
    public bool lookAtTarget = true; // Có nhìn vào thuyền không
    public Vector3 lookOffset = Vector3.zero; // Điểm nhìn offset
    
    void Start()
    {
        // Tự động tìm thuyền nếu chưa gán
        if (target == null)
        {
            GameObject boat = GameObject.FindWithTag("Player");
            if (boat == null)
                boat = GameObject.Find("Wood_BoatV1");
            if (boat != null)
                target = boat.transform;
        }
    }
    
    void LateUpdate()
    {
        if (target == null) return;
        
        // Tính vị trí mong muốn
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        
        // Di chuyển camera mượt mà
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        
        // Xoay camera nhìn vào thuyền (nếu bật)
        if (lookAtTarget)
        {
            Vector3 lookPoint = target.position + lookOffset;
            Vector3 direction = lookPoint - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    void OnDrawGizmos()
    {
        if (target != null)
        {
            // Vẽ đường nối từ camera đến thuyền
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, target.position);
            
            // Vẽ vị trí offset
            Gizmos.color = Color.red;
            Vector3 offsetPos = target.position + target.TransformDirection(offset);
            Gizmos.DrawWireSphere(offsetPos, 0.5f);
        }
    }
} 
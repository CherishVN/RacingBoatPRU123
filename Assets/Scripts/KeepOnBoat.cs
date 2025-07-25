using UnityEngine;

public class KeepOnBoat : MonoBehaviour
{
    [Header("Position Lock")]
    public float fixedYOffset = 0.24f; // Dựa vào Y hiện tại trong Transform
    public bool lockY = true;
    
    private Transform parentBoat;
    
    void Start()
    {
        parentBoat = transform.parent;
        if (parentBoat == null)
        {
            Debug.LogWarning("Character should be child of boat!");
        }
    }
    
    void LateUpdate()
    {
        if (lockY)
        {
            Vector3 localPos = transform.localPosition;
            localPos.y = fixedYOffset;
            transform.localPosition = localPos;
        }
    }
} 
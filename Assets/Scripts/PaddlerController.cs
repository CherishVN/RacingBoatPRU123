using UnityEngine;

public class PaddlerController : MonoBehaviour
{
    [Header("Animation Settings")]
    public Animator animator;
    
    [Header("Boat References")]
    public BoatMovement boatMovement;
    public BoatMovementArrows boatMovementArrows;
    
    [Header("Animation Parameters")]
    public string paddlingBool = "IsPaddling";
    public string speedParameter = "PaddlingSpeed";

    [Header("Audio Settings")]
    public AudioClip[] rowingSounds;
    private AudioSource audioSource;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
            
        if (boatMovement == null)
            boatMovement = GetComponentInParent<BoatMovement>();
            
        if (boatMovementArrows == null)
            boatMovementArrows = GetComponentInParent<BoatMovementArrows>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("Không tìm thấy AudioSource, đã tự động thêm một cái.");
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }
    
    void Update()
    {
        HandlePaddlingAnimation();
    }
    
    void HandlePaddlingAnimation()
    {
        if (animator == null) return;
        
        float motorInput = GetMotorInput();
        bool isMoving = Mathf.Abs(motorInput) > 0.1f;
        
        // Debug để kiểm tra
        Debug.Log($"Motor Input: {motorInput}, Is Moving: {isMoving}");
        
        // Cập nhật animator liên tục
        animator.SetBool(paddlingBool, isMoving);
        animator.SetFloat(speedParameter, Mathf.Abs(motorInput));
    }
    
    float GetMotorInput()
    {
        if (boatMovement != null)
            return boatMovement.MotorInput;
            
        if (boatMovementArrows != null)
            return boatMovementArrows.MotorInput;
        
        // Fallback: đọc input trực tiếp
        float input = 0f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) input = 1f;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) input = -1f;
        
        return input;
    }

    public void PlayRowingSound()
    {
        if (rowingSounds.Length == 0 || audioSource == null) return;

        AudioClip clipToPlay = rowingSounds[Random.Range(0, rowingSounds.Length)];

        audioSource.pitch = Random.Range(0.9f, 1.1f);

        audioSource.PlayOneShot(clipToPlay);
    }
} 
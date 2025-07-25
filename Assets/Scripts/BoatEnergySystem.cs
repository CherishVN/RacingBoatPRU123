using UnityEngine;
using UnityEngine.UI;

public class BoatEnergySystem : MonoBehaviour
{
    [Header("Energy Settings")]
    public float maxEnergy = 100f;
    public float currentEnergy = 0f;
    public float energyGainPerOrb = 20f;
    
    [Header("Speed Boost Settings")]
    public float speedBoostMultiplier = 2f;
    public float speedBoostDuration = 3f;
    public float speedBoostEnergyCost = 50f;
    
    [Header("UI References")]
    public Slider energySlider;
    
    private BoatMovement boatMovement;
    private BoatMovementArrows boatMovementArrows;
    private bool isSpeedBoosting;
    private float speedBoostTimeLeft;
    private float originalMotorForce;

    void Start()
    {
        // Thử tìm một trong hai loại movement
        boatMovement = GetComponent<BoatMovement>();
        boatMovementArrows = GetComponent<BoatMovementArrows>();

        if (boatMovement == null && boatMovementArrows == null)
        {
            Debug.LogError("BoatEnergySystem cần có BoatMovement hoặc BoatMovementArrows component!");
            return;
        }

        // Lưu lại giá trị motorForce gốc của loại movement đang dùng
        if (boatMovement != null)
            originalMotorForce = boatMovement.motorForce;
        else
            originalMotorForce = boatMovementArrows.motorForce;

        if (energySlider != null)
        {
            energySlider.minValue = 0f;
            energySlider.maxValue = maxEnergy;
            energySlider.value = currentEnergy;
        }
    }

    void Update()
    {
        // Kiểm tra input để kích hoạt speed boost tùy theo loại thuyền
        if (boatMovement != null)
        {
            // Thuyền 1 dùng phím E
            if (Input.GetKeyDown(KeyCode.E))
            {
                TryActivateSpeedBoost();
            }
        }
        else if (boatMovementArrows != null)
        {
            // Thuyền 2 dùng phím Shift
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                TryActivateSpeedBoost();
            }
        }

        // Cập nhật speed boost nếu đang active
        if (isSpeedBoosting)
        {
            UpdateSpeedBoost();
        }

        // Cập nhật UI
        if (energySlider != null)
        {
            energySlider.value = currentEnergy;
        }
    }

    public void CollectEnergyOrb()
    {
        currentEnergy = Mathf.Min(currentEnergy + energyGainPerOrb, maxEnergy);
    }

    void TryActivateSpeedBoost()
    {
        if (currentEnergy >= speedBoostEnergyCost && !isSpeedBoosting)
        {
            // Trừ năng lượng
            currentEnergy -= speedBoostEnergyCost;

            // Kích hoạt speed boost
            isSpeedBoosting = true;
            speedBoostTimeLeft = speedBoostDuration;
            
            // Áp dụng tăng tốc cho loại movement đang dùng
            if (boatMovement != null)
                boatMovement.motorForce = originalMotorForce * speedBoostMultiplier;
            else
                boatMovementArrows.motorForce = originalMotorForce * speedBoostMultiplier;

            // Visual/Audio feedback có thể thêm ở đây
        }
    }

    void UpdateSpeedBoost()
    {
        speedBoostTimeLeft -= Time.deltaTime;
        
        if (speedBoostTimeLeft <= 0)
        {
            // Kết thúc speed boost
            isSpeedBoosting = false;
            
            // Khôi phục tốc độ gốc cho loại movement đang dùng
            if (boatMovement != null)
                boatMovement.motorForce = originalMotorForce;
            else
                boatMovementArrows.motorForce = originalMotorForce;
        }
    }
}

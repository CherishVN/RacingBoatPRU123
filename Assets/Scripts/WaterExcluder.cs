using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WaterExcluder : MonoBehaviour
{
    private void Awake()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Floater floater = other.GetComponent<Floater>();
        if (floater != null)
        {
            if (floater.isPartOfShip) return;

            floater.EnterExclusionZone();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Floater floater = other.GetComponent<Floater>();
        if (floater != null)
        {
            // --- THÊM DÒNG KIỂM TRA NÀY ---
            if (floater.isPartOfShip) return; // Nếu là floater của tàu thì bỏ qua

            floater.ExitExclusionZone();
        }
    }
}
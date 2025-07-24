using UnityEngine;
using UnityEngine.Rendering.HighDefinition; // Gi? l?i n?u b?n ?ang d�ng HDRP

public class Floater : MonoBehaviour
{
    public Rigidbody rb;
    public float depthBefSub;
    public float displacementAmt;
    public int floaters;
    public float waterDrag;
    public float waterAngularDrag;
    public WaterSurface water;

    private WaterSearchParameters Search;
    private WaterSearchResult SearchResult;
    private bool isInExclusionZone = false;
    public bool isPartOfShip = false;
    void FixedUpdate()
    {
        rb.AddForceAtPosition(Physics.gravity / floaters, transform.position, ForceMode.Acceleration);

        if (!isInExclusionZone)
        {
            ApplyBuoyancy();
        }
    }

    void ApplyBuoyancy()
    {
        Search.startPositionWS = transform.position;
        water.ProjectPointOnWaterSurface(Search, out SearchResult);

        if (transform.position.y < SearchResult.projectedPositionWS.y)
        {
            float displacementMulti = Mathf.Clamp01((SearchResult.projectedPositionWS.y - transform.position.y) / depthBefSub) * displacementAmt;
            rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMulti, 0f), transform.position, ForceMode.Acceleration);
            rb.AddForce(displacementMulti * -rb.linearVelocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rb.AddTorque(displacementMulti * -rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
    public void EnterExclusionZone()
    {
        isInExclusionZone = true;
    }
    public void ExitExclusionZone()
    {
        isInExclusionZone = false;
    }
}
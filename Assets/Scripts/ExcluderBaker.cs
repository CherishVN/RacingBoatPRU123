using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ExcluderBaker : MonoBehaviour
{
    [Header("Dimensions")]
    public float totalLength = 3f;
    public float middleSectionLength = 1.5f;
    public float width = 1.2f;
    public float height = 0.5f;

    [Header("Shape")]
    [Range(0f, 1f)]
    public float sideCurve = 0.2f;

#if UNITY_EDITOR
    private Vector3[] GetVertices()
    {
        float halfTotalL = totalLength / 2f;
        float halfMidL = middleSectionLength / 2f;
        float halfW = width / 2f;
        float halfH = height / 2f;
        float curveW = halfW + sideCurve;

        return new Vector3[]
        {
            new Vector3(halfMidL, halfH, -halfW), new Vector3(halfMidL, halfH, halfW), new Vector3(halfMidL, -halfH, halfW), new Vector3(halfMidL, -halfH, -halfW),
            new Vector3(-halfMidL, halfH, -halfW), new Vector3(-halfMidL, halfH, halfW), new Vector3(-halfMidL, -halfH, halfW), new Vector3(-halfMidL, -halfH, -halfW),
            new Vector3(0, halfH, -curveW), new Vector3(0, halfH, curveW), new Vector3(0, -halfH, curveW), new Vector3(0, -halfH, -curveW),
            new Vector3(halfTotalL, 0, 0), new Vector3(-halfTotalL, 0, 0)
        };
    }

    private int[] GetTriangles()
    {
        return new int[] { 0, 1, 9, 0, 9, 8, 4, 5, 9, 4, 9, 8, 2, 3, 11, 2, 11, 10, 6, 7, 11, 6, 11, 10, 1, 2, 10, 1, 10, 9, 5, 6, 10, 5, 9, 10, 0, 11, 3, 0, 8, 11, 4, 11, 7, 4, 8, 7, 1, 12, 0, 2, 12, 1, 3, 12, 2, 0, 12, 3, 4, 13, 5, 5, 13, 6, 6, 13, 7, 7, 13, 4 };
    }

    private void OnDrawGizmosSelected()
    {
        Vector3[] localPoints = GetVertices();
        Vector3[] worldPoints = new Vector3[localPoints.Length];

        for (int i = 0; i < localPoints.Length; i++)
        {
            worldPoints[i] = transform.TransformPoint(localPoints[i]);
        }

        Gizmos.color = Color.yellow;
        var tris = GetTriangles();
        for (int i = 0; i < tris.Length; i += 3)
        {
            Gizmos.DrawLine(worldPoints[tris[i]], worldPoints[tris[i + 1]]);
            Gizmos.DrawLine(worldPoints[tris[i + 1]], worldPoints[tris[i + 2]]);
            Gizmos.DrawLine(worldPoints[tris[i + 2]], worldPoints[tris[i]]);
        }
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(ExcluderBaker))]
public class ExcluderBakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ExcluderBaker baker = (ExcluderBaker)target;

        if (GUILayout.Button("Bake and Save Mesh..."))
        {
            BakeAndSave(baker);
        }
    }

    private void BakeAndSave(ExcluderBaker baker)
    {
        MeshFilter mf = baker.GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mesh.name = "BakedBoatHullShape";

        // --- Copy/pasted a portion of the GetVertices and GetTriangles logic here ---
        float halfTotalL = baker.totalLength / 2f;
        float halfMidL = baker.middleSectionLength / 2f;
        float halfW = baker.width / 2f;
        float halfH = baker.height / 2f;
        float curveW = halfW + baker.sideCurve;
        Vector3[] vertices = { new Vector3(halfMidL, halfH, -halfW), new Vector3(halfMidL, halfH, halfW), new Vector3(halfMidL, -halfH, halfW), new Vector3(halfMidL, -halfH, -halfW), new Vector3(-halfMidL, halfH, -halfW), new Vector3(-halfMidL, halfH, halfW), new Vector3(-halfMidL, -halfH, halfW), new Vector3(-halfMidL, -halfH, -halfW), new Vector3(0, halfH, -curveW), new Vector3(0, halfH, curveW), new Vector3(0, -halfH, curveW), new Vector3(0, -halfH, -curveW), new Vector3(halfTotalL, 0, 0), new Vector3(-halfTotalL, 0, 0) };
        int[] triangles = { 0, 1, 9, 0, 9, 8, 4, 5, 9, 4, 9, 8, 2, 3, 11, 2, 11, 10, 6, 7, 11, 6, 11, 10, 1, 2, 10, 1, 10, 9, 5, 6, 10, 5, 9, 10, 0, 11, 3, 0, 8, 11, 4, 11, 7, 4, 8, 7, 1, 12, 0, 2, 12, 1, 3, 12, 2, 0, 12, 3, 4, 13, 5, 5, 13, 6, 6, 13, 7, 7, 13, 4 };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mf.sharedMesh = mesh;

        string path = EditorUtility.SaveFilePanelInProject("Save Baked Mesh", "BoatExcluderMesh", "asset", "Save the mesh.");
        if (string.IsNullOrEmpty(path)) return;

        AssetDatabase.CreateAsset(Instantiate(mf.sharedMesh), path);
        AssetDatabase.SaveAssets();
        Debug.Log("Mesh baked and saved successfully at: " + path);
    }
}
#endif
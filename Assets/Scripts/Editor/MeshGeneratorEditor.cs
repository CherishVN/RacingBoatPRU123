using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshGenerator))]
public class MeshGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MeshGenerator myScript = (MeshGenerator)target;
        if (GUILayout.Button("Generate Mesh")) { myScript.GenerateMesh(); }
        if (GUILayout.Button("Save Mesh As Asset...")) { SaveMesh(myScript); }
    }

    void SaveMesh(MeshGenerator generator)
    {
        MeshFilter mf = generator.GetComponent<MeshFilter>();
        if (mf == null || mf.sharedMesh == null) { Debug.LogError("No mesh found to save! Please generate the mesh first."); return; }
        string path = EditorUtility.SaveFilePanelInProject("Save Generated Mesh", "BoatExcluderMesh", "asset", "Save the generated mesh.");
        if (string.IsNullOrEmpty(path)) { return; }
        Mesh meshToSave = Instantiate(mf.sharedMesh);
        AssetDatabase.CreateAsset(meshToSave, path);
        AssetDatabase.SaveAssets();
        Debug.Log("Mesh saved successfully at: " + path);
    }
}
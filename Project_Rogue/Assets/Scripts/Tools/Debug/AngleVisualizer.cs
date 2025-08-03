using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class AngleVisualizer : MonoBehaviour
{
    public float radius = 3f;
    private float minAngle = -GlobalBallVariables.angleOfShooting;
    private float maxAngle = GlobalBallVariables.angleOfShooting;
    public int segmentCount = 30;
    public Color debugColor = new Color(1f, 1f, 0f, 0.3f);

    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateSectorMesh();
    }

    void CreateSectorMesh()
    {
        mesh.Clear();

        // Vertices
        Vector3[] vertices = new Vector3[segmentCount + 2];
        vertices[0] = Vector3.zero;

        float angleRange = maxAngle - minAngle;
        float angleStep = angleRange / segmentCount;

        for (int i = 0; i <= segmentCount; i++)
        {
            float angleDeg = minAngle + (angleStep * i);
            float angleRad = Mathf.Deg2Rad * angleDeg;

            float x = Mathf.Sin(angleRad) * radius;
            float y = Mathf.Cos(angleRad) * radius;

            vertices[i + 1] = new Vector3(x, y, 0);
        }

        // Triangles
        int[] triangles = new int[segmentCount * 3];
        for (int i = 0; i < segmentCount; i++)
        {
            int start = (i + 1);
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = start;
            triangles[i * 3 + 2] = start + 1;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshRenderer>().material.color = debugColor;

        //GetComponent<MeshRenderer>().GetMaterials[0].albedo = debugColor;
    }

    void OnDrawGizmosSelected()
    {
        // Affiche la direction en Gizmo (facultatif)
        Vector3 minDir = Quaternion.Euler(0, 0, minAngle) * Vector3.up;
        Vector3 maxDir = Quaternion.Euler(0, 0, maxAngle) * Vector3.up;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, minDir * radius);
        Gizmos.DrawRay(transform.position, maxDir * radius);
    }
}

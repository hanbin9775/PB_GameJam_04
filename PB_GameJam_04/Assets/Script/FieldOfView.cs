using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This code is referenced from youtube 'CodeMonkey' Channel
 * 'Field of View Effect in Unity' video
 */

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;

    private Mesh mesh;
    private Vector3 origin;
    private float fov;
    
    private int rayCount;
    private float angle;
    private float angleIncrease;
    private float viewDistance;

    private Vector3[] vertices ;
    private Vector2[] uv;
    private int[] triangles;

    private bool player_detected = false;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero;
        fov = 360f;
        rayCount = 50;
        angleIncrease = fov / rayCount;
        viewDistance = 5f;
    }
    private void LateUpdate()
    {
        angle = 0f;
        vertices = new Vector3[rayCount + 1 + 1];
        vertices[0] = origin;
        uv = new Vector2[vertices.Length];
        triangles = new int[rayCount * 3];

        player_detected = false;
        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i=0; i<=rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance,layerMask);

            if(raycastHit2D.collider == null)
            {
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                vertex = raycastHit2D.point;
                if (raycastHit2D.collider.tag == "Player")player_detected = true;
            }

            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;
                triangleIndex += 3;
            }
            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    public Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public void SetOrigin(Vector2 origin)
    {
        this.origin = origin;
    }

    public bool isPlayerDetected()
    {
        if (player_detected) return true;
        else return false;
    }

}

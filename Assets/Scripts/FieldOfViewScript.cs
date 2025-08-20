using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class FieldOfViewScript : MonoBehaviour
{
    [SerializeField] private SpriteMask spriteMask;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float fieldOfView = 90f;
    [SerializeField] private int rayCount = 5;
    [SerializeField] private float angle = 0f;
    [SerializeField] private float viewRange = 0f;
    [SerializeField] private LayerMask mask;
    [SerializeField] private float startingAngle = 0f;

    private Vector3 origin;

    private Sprite sprite;

    void Start()
    {
        var texture2D = new Texture2D(32, 32);
        sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 1);
        spriteMask.sprite = sprite;
        spriteRenderer.sprite = sprite;
        origin = Vector3.zero;

        Vector2[] vertices = new Vector2[5]
        {
            new Vector2(0, texture2D.height / 2),
            new Vector2(16, 32),
            new Vector2(24, 24),
            new Vector2(24, 12),
            new Vector2(16, 0)
        };

        ushort[] triangles = new ushort[9]
        {
            0,
            1,
            2,
            0,
            2,
            3,
            0,
            3,
            4
        };

        sprite.OverrideGeometry(vertices, triangles);
    }

    private void LateUpdate()
    {
        float angleIncrease = fieldOfView / rayCount;
        angle = startingAngle;
        origin = Vector2.zero;

        Vector2[] vertices = new Vector2[rayCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        ushort[] triangles = new ushort[rayCount * 3];

        vertices[0] = new Vector2(16, 16);
        vertices[1] = new Vector3(50, 0);
        vertices[2] = new Vector3(0, -50);

        triangles[0] = 0;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 1; i < rayCount; i++)
        {
            float angleRad = angle * (Mathf.PI / 180f);
            Vector3 vertex;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad)), viewRange, mask);
            Debug.DrawRay(transform.position, new Vector3(Mathf.Cos(angleRad) * viewRange, Mathf.Sin(angleRad)) * viewRange, Color.red);

            if (hit.collider == null)
            {
                vertex = vertices[0] + new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * viewRange;
            }
            else
            {
                vertex = vertices[0] + (hit.point - (Vector2)transform.position);
            }

            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = (ushort)(vertexIndex - 1);
                triangles[triangleIndex + 2] = (ushort)vertexIndex;
                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        sprite.OverrideGeometry(vertices, triangles);
    }

    public void SetAimDirection(Vector3 aimDirection) 
    {
        aimDirection = aimDirection.normalized;
        float n = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        startingAngle = n + fieldOfView / 2;
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }
}


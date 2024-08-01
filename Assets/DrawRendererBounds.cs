using System.Collections.Generic;
using UnityEngine;

public class DrawRendererBounds : MonoBehaviour
{
    
    public void OnDrawGizmosSelected()
    {
        Bounds BoundingBox = Utilits.GetHierarchyBounds(transform);
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(BoundingBox.center, BoundingBox.extents * 2);

        Vector3 DoOffset( Vector3 Center, Vector3 Dot, float Offset) 
            => Dot + Vector3.Normalize(Dot - Center) * Offset;

        //(1,1)  (0,1)  (-1,1)

        //(1,0)  (0,0)  (-1,0)

        //(1,-1) (0,-1) (-1,-1)

        //BoundingBox.extents - вектор указывающий на точку (1, 1) (является длинной единичного вектора(единичным отрезкои отдельновзятых осей)), умнажая его x и y компоненты на 1,0,-1 можно получить все точки в локальных координатах относительно центра BoundingBox

        for (int y = -1; y <= 1; y++)
            for (int x = -1; x <= 1; x++)
            {
                Vector2 DotDirection = new Vector2(x, y);
                if (DotDirection == Vector2.zero) continue;
                Vector3 DotOrigin = BoundingBox.center + new Vector3(BoundingBox.extents.x * DotDirection.x, 0f, BoundingBox.extents.z * DotDirection.y);
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(DoOffset(BoundingBox.center, DotOrigin, 3), 0.5f);
            }

        //Physics.Raycast(CameraAnchor.position, CameraNewPos - CameraAnchor.position, out hit, CameraRange + 2)
    }
}
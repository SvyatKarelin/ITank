using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : DestructableVehicle
{
    [SerializeField] private Cannon cannon;
    protected NavMeshAgent agent;

    public override void Start()
    {
        HealthPoints = StartHealth;
        agent = Utilits.CheckComponent<NavMeshAgent>(transform);
    }
    Transform GetTarget()
    {
        return GameObject.FindGameObjectWithTag("Player").transform;
    }

    public Dictionary<Vector2, Vector3> GetShelterPoints(Transform Shelter)
    {
        Vector3 DoOffset(Vector3 Center, Vector3 Dot, float Offset)
            => Dot + Vector3.Normalize(Dot - Center) * Offset;

        //(1,1)  (0,1)  (-1,1)

        //(1,0)  (0,0)  (-1,0)

        //(1,-1) (0,-1) (-1,-1)

        //BoundingBox.extents - вектор указывающий на точку (1, 1) (является длинной единичного вектора(единичным отрезкои отдельновзятых осей)), умнажая его x и y компоненты на 1,0,-1 можно получить все точки в локальных координатах относительно центра BoundingBox

        Bounds BoundingBox = Utilits.GetHierarchyBounds(Shelter);
        Dictionary<Vector2, Vector3> Points = new Dictionary<Vector2, Vector3>();

        for (int y = -1; y <= 1; y++)
            for (int x = -1; x <= 1; x++)
            {
                Vector2 DotDirection = new Vector2(x, y);
                if (DotDirection == Vector2.zero) continue;
                Vector3 DotOrigin = BoundingBox.center + new Vector3(BoundingBox.extents.x * DotDirection.x, 0f, BoundingBox.extents.z * DotDirection.y);
                Points.Add(DotDirection, DoOffset(BoundingBox.center, DotOrigin, 3f));
            }
        return Points;
    }

    RaycastHit[] RaycastBetweenObj(Transform Obj1, Transform Obj2, Vector3 Offset)
    {
        Vector3 Pos1Offset = Obj1.position + Offset;
        Vector3 Pos2Offset = Obj2.position + Offset;
        Debug.DrawLine(Pos1Offset,Pos2Offset);
        RaycastHit[] Result = Physics.RaycastAll(Pos1Offset, Pos2Offset - Pos1Offset, Vector3.Distance(Pos1Offset, Pos2Offset))
        .Where(Hit => (Hit.transform.parent != Obj1) && (Hit.transform.parent != Obj2) && (Hit.transform != Obj1) && (Hit.transform != Obj2)).ToArray();
        return Result;
    }

    RaycastHit[] RaycastBetweenObj(Transform Obj1, Vector3 Obj2, Vector3 Offset)
    {
        Vector3 Pos1Offset = Obj1.position + Offset;
        Vector3 Pos2Offset = Obj2 + Offset;
        RaycastHit[] Result = Physics.RaycastAll(Pos1Offset, Pos2Offset - Pos1Offset, Vector3.Distance(Pos1Offset, Pos2Offset))
        .Where(Hit => (Hit.transform.parent != Obj1.transform) && (Hit.transform != Obj1.transform)).ToArray();
        return Result;
    }
    RaycastHit[] RaycastBetweenObj(Transform Obj1, Transform Obj2) => Physics.RaycastAll(Obj1.position, Obj2.position - Obj1.position, Vector3.Distance(Obj1.position, Obj2.position))
        .Where(Hit => (Hit.transform.parent != Obj1.transform) && (Hit.transform != Obj1.transform) && (Hit.transform != Obj2)).ToArray();


    Transform FindShelter(float Radius)//, out Vector3 SafePos, out Vector3 ShootPos)
    {
        List<Transform> AvailableShelters = new List<Transform>();
        Collider[] Objects = Physics.OverlapSphere(transform.position, Radius).Where(Obj => Obj.tag == "Shelter").ToArray();
        foreach (Collider obj in Objects) {
            RaycastHit[] hits;
            //проверяем наличие преград до цели
            hits = RaycastBetweenObj(GetTarget(), obj.transform, new Vector3(0f, 3f, 0f));
            //foreach (RaycastHit h in hits) print(h.transform.name);
            if (hits.Length <= 0) AvailableShelters.Add(obj.transform);
        }
        //Сортируем по расстоянию до нас
        return AvailableShelters.OrderBy(Shelter => Vector3.Distance(Shelter.position , transform.position)).ToArray()[0];
    }

    bool GetShelterPos(float Radius, out Vector3 SafePos, out Vector3 ShootPos)
    {
        SafePos = Vector3.zero;
        ShootPos = Vector3.zero;

        Transform Shelter = FindShelter(Radius);
        if (!Shelter) return false;
        Dictionary<Vector2, Vector3> ShPoints = GetShelterPoints(Shelter);

        Dictionary<Vector2, bool> Raycasts = ShPoints.Select(Point => new { Key = Point.Key, Value = RaycastBetweenObj(GetTarget(), Point.Value, new Vector3(0f, 2f, 0f)).Length <= 0 })
            .ToDictionary(Pair => Pair.Key, Pair => Pair.Value);


        foreach (var Center in Raycasts.Where(Pair => Pair.Key.x == 0 || Pair.Key.y == 0))
        {
            if (!Center.Value) {
                print(Center.Key);
                //проверяем центры на безопасность от игрока
                SafePos = ShPoints[Center.Key];
                for (int Point = -1; Point <= 1; Point++)
                {
                    if (Point == 0) continue;
                    //проверяем края на возможность отаки по игроку
                    if (Center.Key.x == 0 && Raycasts[new Vector2(Point, Center.Key.y)]) { ShootPos = ShPoints[new Vector2(Point, Center.Key.y)]; return true; }
                    if (Center.Key.y == 0 && Raycasts[new Vector2(Center.Key.x, Point)]) { ShootPos = ShPoints[new Vector2(Center.Key.x, Point)]; return true; }
                }
            }
        }
        return false;
    }

    private void Update()
    {
        //if (RaycastBetweenObj(transform, GetTarget(), new Vector3(0f, 3f, 0f)).Length > 0) {
        if(GetShelterPos(200, out Vector3 SafePos, out Vector3 ShootPos)) {
            if (cannon.IsReloading) agent.SetDestination(SafePos);
            else { 
                agent.SetDestination(ShootPos);
            }
        }
        else agent.SetDestination(GetTarget().position + (transform.position-GetTarget().position).normalized * 10);

        cannon.LookAt(GetTarget());
        if (RaycastBetweenObj(transform, GetTarget(), new Vector3(0f, 2f, 0f)).Length <= 0 && cannon.IsAimed) cannon.Shoot();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    [SerializeField] private int GridSize;
    [SerializeField] private float GlobalGridSize;
    [SerializeField] private Vector2 MapSize;
    [SerializeField] private List<GameObject> CellPrefabs;
    [SerializeField] private Transform StartPoss;
    private int[,] Map;
    [SerializeField] private Dictionary<Vector2, GameObject> CellSizes;

    Vector2 GetCellSize(Transform Cell)
    {
        Vector3 GlblSize = Cell.transform.lossyScale;
        return new Vector2(GlblSize.x / GridSize, GlblSize.z / GridSize);
    }

    void CreateIntersect(Vector2 Coords, Vector3 StartPos)
    {
        if (Map[(int)Coords.x, (int)Coords.y] != 0) return;
        //максимально возможный bounding box обьекта где Coords + min - нижняя левая точка, Coords + max - верхняя правая 
        Vector2 Min = new Vector2( GetIntersectLenght(Coords, new Vector2(-1,0)), GetIntersectLenght(Coords, new Vector2(0, -1)));
        Vector2 Max = new Vector2( GetIntersectLenght(Coords, new Vector2(1,0)), GetIntersectLenght(Coords, new Vector2(0, 1)));
        //максимально возможный размер блока(с учетом блока из которого происходят intersect ы)
        Vector2 FreeSpace = Min + Max + new Vector2(1,1);
        Dictionary<Vector2, GameObject> AvailableSizes = new();

        foreach (var Size in CellSizes)
            if ((Size.Key.x <= FreeSpace.x && Size.Key.y <= FreeSpace.y) || (Size.Key.x <= FreeSpace.y && Size.Key.y <= FreeSpace.x)) AvailableSizes.Add(Size.Key, Size.Value);
        //print(AvailableSizes.ElementAt(0));
        if (AvailableSizes.Count <= 0) return;

        var Rand = AvailableSizes.ElementAt(UnityEngine.Random.Range(0, AvailableSizes.Count));
        //если обьект не влезает в стандартном положении - перевернуть
        bool Rotate = !(Rand.Key.x <= FreeSpace.x && Rand.Key.y <= FreeSpace.y);

        //размер с учетом вращения
        Vector2 RandSize = new Vector2((Rotate? Rand.Key.y : Rand.Key.x) - 1, (Rotate ? Rand.Key.x : Rand.Key.y) - 1);

        print(RandSize);
        //в координатной системе относительно Coords(Coords - (0, 0) Min - (-X, -Y); Max - (X, Y)) приоритет размещения блока:-Y, X
        //тоесть для X: из всего свободного пространства в Max берем максииально возможное количество клеток, в Min оставшеся((свободное пространство) - (Max))
        Vector2 CurMin = - new Vector2(Mathf.Clamp(Mathf.Clamp(Min.x, 0, RandSize.x) - Mathf.Clamp(Max.x, 0, RandSize.x), 0, Mathf.Infinity), Mathf.Clamp(Min.y, 0, RandSize.y));
        Vector2 CurMax = new Vector2(Mathf.Clamp(Max.x, 0, RandSize.x), Mathf.Clamp(Mathf.Clamp(Max.y, 0, RandSize.y) - Mathf.Clamp(Min.y, 0, RandSize.y), 0, Mathf.Infinity));
        //print(CurMin);
        //print(CurMax);

        int SUS = UnityEngine.Random.Range(1, 9);
        for (int x = (int)(CurMin + Coords).x; x <= (int)(CurMax + Coords).x; x++)
            for (int y = (int)(CurMin + Coords).y; y <= (int)(CurMax + Coords).y; y++)
                Map[x, y] = SUS;

        //находим центр обьекта как сумму локальных координат центра (размер пополам) и минимальной точки
        Vector3 Pos = StartPos + new Vector3((CurMin + Coords).x + (RandSize.x) / 2, 0 , (CurMin + Coords).y + (RandSize.y) / 2)* GlobalGridSize;
        Quaternion Rot = new();
        Rot.eulerAngles = Rotate ? new Vector3(0, 90, 0) : Vector2.zero;
        GameObject Cell = Instantiate(Rand.Value, Pos, Rot);

    }

    public bool IsBetween(double testValue, double bound1, double bound2)
    {
        return (testValue >= Math.Min(bound1, bound2) && testValue <= Math.Max(bound1, bound2));
    }

    Vector2 Intersect(Vector2 Coords, Vector2 Dir)
    {
        Dir = Dir.normalized;
        Vector2 CurCheck = Coords;
        while(IsBetween(CurCheck.x, 0, MapSize.x - 1) && IsBetween(CurCheck.y, 0, MapSize.y - 1))
        {
            if (Map[(int)CurCheck.x, (int)CurCheck.y] != 0) break;
            //print(Map[(int)CurCheck.x, (int)CurCheck.y]);
            CurCheck += Dir;
        }
        //если произошло изменение координат то возвращаем точку до точки вхождения внутрь обьекта иначе стартовую позицию
        return CurCheck == Coords? Coords : CurCheck - Dir;
    }

    float GetIntersectLenght(Vector2 Coords, Vector2 Dir) => (Intersect(Coords, Dir) - Coords).magnitude;

    void Start()
    {
        //--> y
        //|
        //\/ x
        Map = new int[(int)MapSize.x, (int)MapSize.y];
        CellSizes = new();

        foreach (GameObject Cell in CellPrefabs)
        {
            CellSizes.Add(GetCellSize(Cell.transform), Cell);
            print(GetCellSize(Cell.transform));
            //print(GetIntersectLenght(new Vector2(0,0), new Vector2(1,0)));
            //Map[2, 2] = 4;
        }
        //CreateIntersect(new Vector2(1, 1), StartPoss.position);
        for (int x = 0; x < (int)MapSize.x; x++)
            for (int y = 0; y < (int)MapSize.y; y++) 
                CreateIntersect(new Vector2(x, y), StartPoss.position);

        for (int y = 0; y < (int)MapSize.y; y++)
        {
            string PrStr = "";
            for (int x = 0; x < (int)MapSize.x; x++)
                PrStr += Map[x,y];
            print(PrStr);
        }

        //GameObject Cell = Instantiate(CellPrefabs[0], transform.position, Quaternion.identity);
        //print(GetCellSize(Cell.transform));
        //for (int i = 0; i < ; i++) 
        //print(Intersect(new Vector2(0,0), new Vector2(0, 1)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

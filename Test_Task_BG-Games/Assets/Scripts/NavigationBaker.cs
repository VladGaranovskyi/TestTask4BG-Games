using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationBaker : MonoBehaviour
{
    [SerializeField] private List<NavMeshSurface> surfaces = new List<NavMeshSurface>();
    [HideInInspector] public Action OnSpawnObject;
    private const float _defaultCount = 1;

    private void Start()
    {
        OnSpawnObject += BakeMesh;
    }

    private void BakeMesh()
    {
        foreach (var surface in surfaces)
        {
            surface.BuildNavMesh();
        }
    }

    public void AddSurface(NavMeshSurface surface)
    {
        surfaces.Add(surface);
    }

    public void ClearSurfaces()
    {
        for(int i = surfaces.Count - 1; i > _defaultCount; i--)
        {
            GameObject obj = surfaces[i].gameObject;
            surfaces.RemoveAt(i);
            Destroy(obj);
        }
    }
}

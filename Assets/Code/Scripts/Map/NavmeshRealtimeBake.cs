using NavMeshPlus.Components;
using System;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshRealtimeBake : MonoBehaviour
{

    [SerializeField] NavMeshData map;
    private bool CanUpdate;
    private void OnEnable()
    {
        InfinityMap.OnMapReposition += BakeMap;
    }

  

    public void BakeMap()
    {
        CanUpdate = true;
    }

    private void Update()
    {
        if (!CanUpdate)
            return;
        map.position += Vector3.up * (2 * Time.deltaTime);
        
    }

    // private void OnDestroy()
    // {
    //     map.BuildNavMeshAsync();
    // }
}
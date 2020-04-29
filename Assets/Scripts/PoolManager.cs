using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonMono<PoolManager>
{
    public Transform poolMergeContainer;
    public Transform poolNonmergerContainer;
    
    public MeshMerger _meshMerge;
    public override void Awake()
    {
        base.Awake();
    }
    public Transform GetPoolContainer(bool isMergable)
    {
        if (isMergable) return poolMergeContainer;
        return poolNonmergerContainer;
    }
    public void MergeMesh()
    {
        _meshMerge.MergeMesh(true);
    }
}

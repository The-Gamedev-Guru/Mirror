﻿using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesMeshLoader : MonoBehaviour, AddressablesLoaderBase
{
    [SerializeField] private MeshFilter meshFilter = null;
    [SerializeField] private AssetReference addressablesMesh = null;
    private AsyncOperationHandle<Mesh> _asyncOperationHandle;

    private void Awake()
    {
        Debug.Log("AddressablesMeshLoader: Awake");
        _asyncOperationHandle = addressablesMesh.LoadAssetAsync<Mesh>();
        _asyncOperationHandle.Completed += handle =>
        {
            meshFilter.sharedMesh = handle.Result;
        };
    }

    private void OnDestroy()
    {
        Debug.Log("AddressablesMeshLoader: OnDestroy?");
        if (_asyncOperationHandle.IsValid())
        {
            Debug.Log("AddressablesMeshLoader: OnDestroy!");
            Addressables.Release(_asyncOperationHandle);
        }
    }

    public void SetEditorPrefabPreview(bool on)
    {
#if UNITY_EDITOR
        Mesh targetMesh = null;
        if (on)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(addressablesMesh.AssetGUID);
            var subassets = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);
            foreach (var subasset in subassets)
            {
                if (subasset.name == addressablesMesh.SubObjectName && subasset is Mesh targetSubasset)
                {
                    targetMesh = targetSubasset;
                    break;
                }
            }
        }

        meshFilter.sharedMesh = targetMesh;
#endif
    }
}

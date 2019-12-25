using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesSkinnedMeshLoader : MonoBehaviour, AddressablesLoaderBase
{
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer = null;
    [SerializeField] private AssetReference addressablesMesh = null;
    private AsyncOperationHandle<Mesh> _asyncOperationHandle;

    private void Awake()
    {
        Debug.Log("AddressablesSkinnedMeshLoader: Awake");
        _asyncOperationHandle = addressablesMesh.LoadAssetAsync<Mesh>();
        _asyncOperationHandle.Completed += handle =>
        {
            skinnedMeshRenderer.sharedMesh = handle.Result;
        };
    }

    private void OnDestroy()
    {
        Debug.Log("AddressablesSkinnedMeshLoader: OnDestroy?");
        if (_asyncOperationHandle.IsValid())
        {
            Debug.Log("AddressablesSkinnedMeshLoader: OnDestroy!");
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

        skinnedMeshRenderer.sharedMesh = targetMesh;
#endif
    }
}

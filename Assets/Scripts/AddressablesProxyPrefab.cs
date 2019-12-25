using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TheGamedevGuru
{
[RequireComponent(typeof(NetworkIdentity))]
public class AddressablesProxyPrefab : MonoBehaviour
{
    [SerializeField] private AssetReference prefabReference = null;
    private AsyncOperationHandle<GameObject> _instanceHandle;

    private void Awake()
    {
        _instanceHandle = prefabReference.InstantiateAsync(transform);
    }

    private void OnDestroy()
    {
        if (_instanceHandle.IsValid())
        {
            Addressables.ReleaseInstance(_instanceHandle);
        }
    }
}
}


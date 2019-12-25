using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

[InitializeOnLoad]
static class AddressablesPrefabPreview
{
    static AddressablesPrefabPreview()
    {
        Editor.finishedDefaultHeaderGUI += OnPostHeaderGUI;
    }

    private static void OnPostHeaderGUI(Editor editor)
    {
        if (Application.isEditor == false || Application.isPlaying || editor.targets.Length != 1) return;

        var gameObject = editor.target as GameObject;
        if (gameObject == null) return;

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Addressables Preview ON"))
        {
            SetPrefabPreview(gameObject, true);
        }
        if (GUILayout.Button("Addressables Preview OFF"))
        {
            SetPrefabPreview(gameObject, false);
        }
        GUILayout.EndHorizontal();
    }

    private static void SetPrefabPreview(GameObject target, bool on)
    {
        foreach (var addressablesLoaderBase in target.GetComponentsInChildren<AddressablesLoaderBase>())
        {
            addressablesLoaderBase.SetEditorPrefabPreview(on);
        }
    }
}

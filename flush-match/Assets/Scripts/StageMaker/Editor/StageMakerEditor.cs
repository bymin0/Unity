using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StageMaker))]
public class StageMakerEditor : Editor
{
    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        StageMaker stageMaker = (StageMaker)target;
        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0) // 좌클릭
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit2D hit;
            hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit)
            {
                GameObject clickedObject = hit.collider.gameObject;
                Debug.Log($"클릭된 오브젝트: {clickedObject.name}");
                // 클릭된 오브젝트에 대해 원하는 작업 수행
                stageMaker.OnClick(clickedObject.transform.position);
            }
            
            e.Use();
        }
    }
    
    public override void OnInspectorGUI()
    {
        StageMaker stageMaker = (StageMaker)target;

        if (GUILayout.Button("Generate"))
        {
            stageMaker.Generate();
        }
        GUILayout.Space(10);
        if (GUILayout.Button("SetTile"))
        {
            stageMaker.SetTile();
        }
        if (GUILayout.Button("Load"))
        {
            stageMaker.Load();
        }
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("Save"))
        {
            stageMaker.Save();
        }
        
        if (GUILayout.Button("Save As"))
        {
            string path = EditorUtility.SaveFilePanel("Save Stage As", "", $"Stage_{stageMaker.stageNumber}.json", "json");
            if (!string.IsNullOrEmpty(path)) {
                stageMaker.SaveAs(path);
            }
        }

        
        GUILayout.Space(10);
        DrawDefaultInspector();
    }
}

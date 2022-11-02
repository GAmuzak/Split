using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GridMapCreator : EditorWindow
{
    private Vector2 offset;
    private int gridSize = 50;
    private Vector2 drag;
    [MenuItem("Window/LevelCreator")]
    private static void OpenWindow()
    {
        GridMapCreator window = GetWindow<GridMapCreator>();
        window.titleContent = new GUIContent("LevelCreator");
        
    }

    private void OnGUI()
    {
        DrawGrid();
        ProcessGrid(Event.current);
        if (GUI.changed)
        {
            Repaint();
        }
    }

    private void ProcessGrid(Event e)
    {
        drag = Vector2.zero;
        switch (e.type)
        {
            case EventType.MouseDrag:
                if (e.button == 0)
                {
                    OnMouseDrag(e.delta);
                }
                break;
        }
    }

    private void OnMouseDrag(Vector2 delta)
    {
        drag = delta;
        GUI.changed = true;
    }

    private void DrawGrid()
    {
        int widthDivider = Mathf.CeilToInt(position.width / 20);
        int heightDivider = Mathf.CeilToInt(position.height / 20);
        Handles.BeginGUI();
        Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);
        offset += drag;
        Vector3 newOffset = new Vector3(offset.x % gridSize, offset.y % gridSize, 0);

        for (int i = 0; i < widthDivider; i++)
        {
            Vector3 fromVector = new Vector3(gridSize * i, -gridSize, 0) + newOffset;
            Vector3 toVector = new Vector3(gridSize*i, position.height, 0) + newOffset;
            Handles.DrawLine(fromVector, toVector);
        }
        
        for (int i = 0; i < heightDivider; i++)
        {
            Vector3 fromVector = new Vector3(-gridSize, gridSize*i,0) + newOffset;
            Vector3 toVector = new Vector3( position.width, gridSize*i, 0) + newOffset;
            Handles.DrawLine(fromVector, toVector);
        }
        Handles.color = Color.white;
        Handles.EndGUI();
    }
}

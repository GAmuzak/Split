using System;
using System.Collections;
using System.Collections.Generic;
using Codice.Client.Common;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GridMapCreator : EditorWindow
{
    private Vector2 offset;
    private int cellSize = 20;
    private Vector2 gridSize = new Vector2(20,20);
    private Vector2 drag;
    private List<List<Node>> nodes;
    private GUIStyle empty;
    private Vector2 nodePos;
    private StyleManager styleManager;
    private bool isErasing = false;
    [MenuItem("Window/LevelCreator")]
    private static void OpenWindow()
    {
        GridMapCreator window = GetWindow<GridMapCreator>();
        window.titleContent = new GUIContent("LevelCreator");
        Debug.Log("Hello World");
    }

    private void OnEnable()
    {
        SetupStyles();
        empty = new GUIStyle();
        Texture2D icon = Resources.Load("LevelEditoricons/DefaultTile") as Texture2D;
        empty.normal.background = icon;
        SetUpNodes();
    }
    private void OnGUI()
    {
        DrawGrid();
        DrawNodes();
        ProcessNodes(Event.current);
        ProcessGrid(Event.current);
        if (GUI.changed)
        {
            Repaint();
        }
    }

    private void ProcessNodes(Event currentEvent)
    {
        int row = (int)((currentEvent.mousePosition.x - offset.x) / 30);
        int column = (int)((currentEvent.mousePosition.y - offset.y) / 30);
        if (currentEvent.type == EventType.MouseDown)
        {
            if (nodes[row][column].style.normal.background.name == "Empty")
            {
                isErasing = false;
            }
            else
            {
                isErasing = true;
            }

            if (isErasing)
            {
                nodes[row][column].SetStyle(empty);
                GUI.changed = true;
            }
            else
            {
                nodes[row][column].SetStyle(styleManager.buttonStyles[1].nodeStyle);
                GUI.changed = true;
            }
        }

        // if (current.type == EventType.MouseDrag)
        // {
        //     if (isErasing)
        //     {
        //         nodes[row][column].SetStyle(empty);
        //         GUI.changed = true;
        //     }
        //     else
        //     {
        //         nodes[row][column].SetStyle(styleManager.buttonStyles[1].nodeStyle);
        //         GUI.changed = true;
        //     }
        //     current.Use();
        // }

    }

    private void SetupStyles()
    {
        try
        {
            styleManager = GameObject.FindGameObjectWithTag("StyleManager").GetComponent<StyleManager>();
            for (int i = 0; i < styleManager.buttonStyles.Length; i++)
            {
                styleManager.buttonStyles[i].nodeStyle = new GUIStyle();
                styleManager.buttonStyles[i].nodeStyle.normal.background = styleManager.buttonStyles[i].Icon;
            }
        }
        catch (Exception e) { }
    }

    private void SetUpNodes()
    {
        nodes = new List<List<Node>>();
        for (int i = 0; i < gridSize.x; i++)
        {
            nodes.Add(new List<Node>());
            for (int j = 0; j < gridSize.y; j++)
            {
                nodePos.Set(i*30, j*30);
                nodes[i].Add(new Node(nodePos, 30, 30, empty));
            }
        }
    }

    
    private void DrawGrid()
    {
        int widthDivider = Mathf.CeilToInt(position.width / cellSize);
        int heightDivider = Mathf.CeilToInt(position.height / cellSize);
        Handles.BeginGUI();
        Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);
        offset += drag;
        Vector3 newOffset = new Vector3(offset.x % gridSize.x, offset.y % gridSize.y, 0);

        for (int i = 0; i < widthDivider; i++)
        {
            Vector3 fromVector = new Vector3(cellSize * i, -cellSize, 0) + newOffset;
            Vector3 toVector = new Vector3(cellSize*i, position.height, 0) + newOffset;
            Handles.DrawLine(fromVector, toVector);
        }
        
        for (int i = 0; i < heightDivider; i++)
        {
            Vector3 fromVector = new Vector3(-cellSize, cellSize*i,0) + newOffset;
            Vector3 toVector = new Vector3( position.width, cellSize*i, 0) + newOffset;
            Handles.DrawLine(fromVector, toVector);
        }
        Handles.color = Color.white;
        Handles.EndGUI();
    }

    private void DrawNodes()
    {
        for (int i = 0; i < gridSize.x; i++)
        {
            nodes.Add(new List<Node>());
            for (int j = 0; j < gridSize.y; j++)
            {
                nodes[i][j].Draw();
            }
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
        for (int i = 0; i < gridSize.x; i++)
        {
            nodes.Add(new List<Node>());
            for (int j = 0; j < gridSize.y; j++)
            {
                nodes[i][j].Drag(delta);
            }
        }
        GUI.changed = true;
    }

}

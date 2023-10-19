using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DragRigidbody)), CanEditMultipleObjects]
public class MovableRigidbodyEditor : Editor
{/*
    private MovableRigidbody dragRigidbodyScript;

    private SerializedProperty contactPointParent;
    private SerializedProperty outlineTransform;
    private void OnEnable()
    {
        contactPointParent = serializedObject.FindProperty("contactPointParent");
        outlineTransform = serializedObject.FindProperty("outlineTransform");
    }


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //Draggable
        EditorGUILayout.Space();
        
        dragRigidbodyScript = (DragRigidbody) target;
        
        if (dragRigidbodyScript.canBeDragged)
        {
            if (GUILayout.Button("Remove Draggable Object"))
            {
                ToggleDraggableObject();
            }
            
            dragRigidbodyScript.canBeDragged = GUILayout.Toggle(dragRigidbodyScript.canBeDragged, "Can be Dragged");
            EditorGUILayout.PropertyField(contactPointParent);
        }
        else
        {
            if (GUILayout.Button("Setup Draggable Object"))
            {
                ToggleDraggableObject();
            }
        }

                
        //Transform
        EditorGUILayout.Space();
        
        EditorGUILayout.BeginHorizontal();
        if (dragRigidbodyScript.outlineTransform == null)
        {
            if (GUILayout.Button("Setup Transform Object"))
            {
                SetupOutlineTransform();
            }
        }
        else
        {
            if (GUILayout.Button("Reset Transform Object"))
            {
                ResetOutlineTransform();
            }
            EditorGUILayout.PropertyField(outlineTransform);
        }
        EditorGUILayout.EndHorizontal();
        
        if (GUILayout.Button("Setup Outline- and Highlight-Object"))
        {
            SetupOutlineObject();
            //SetupHighlightObject();
        }
        
    }


    private void ToggleDraggableObject()
    {
        //
        dragRigidbodyScript.canBeDragged = !dragRigidbodyScript.canBeDragged;

        if (dragRigidbodyScript.canBeDragged == false)
        {
            DestroyImmediate(dragRigidbodyScript.contactPointParent.gameObject);
        }
        else
        {
            var contactPointPar = new GameObject("ContactPointsParent");
            contactPointPar.transform.SetParent(dragRigidbodyScript.transform);
            dragRigidbodyScript.contactPointParent = contactPointPar.transform;
        }
    }

    private void SetupOutlineTransform()
    {
        if (dragRigidbodyScript.outlineTransform == null)
        {
            foreach (Transform child in dragRigidbodyScript.transform)
            {
                if (child.name == "Transform")
                {
                    dragRigidbodyScript.outlineTransform = child;
                    break;
                }
            }

            if (dragRigidbodyScript.outlineTransform == null)
            {
                var transformObject = new GameObject("Transform");
                transformObject.transform.SetParent(dragRigidbodyScript.transform);
                dragRigidbodyScript.outlineTransform = transformObject.transform;
            }
        }
    }

    private void ResetOutlineTransform()
    {
        dragRigidbodyScript.outlineTransform.position = new Vector3(0, 0, 0);
    }

    void SetupOutlineObject()
    {
        if (!dragRigidbodyScript.GetComponent<MeshFilter>())
        {
            return;
        }

        if (dragRigidbodyScript.outlineObject != null)
        {
            
            DestroyImmediate(dragRigidbodyScript.outlineObject);
            DestroyImmediate(dragRigidbodyScript.outlineMaskObject);
            DestroyImmediate(dragRigidbodyScript.wireFrameObject);
            // dragRigidbodyScript.outlineObject = null;
            // dragRigidbodyScript.outlineMaskObject = null;
            // dragRigidbodyScript.wireFrameObject = null;
        }
        //outlineObject
        var outlineObject = new GameObject("OutlineMesh", typeof(MeshFilter), typeof(MeshRenderer));
        outlineObject.transform.parent = dragRigidbodyScript.outlineTransform.transform;
        outlineObject.transform.position = dragRigidbodyScript.outlineTransform.position;
        outlineObject.transform.rotation = dragRigidbodyScript.outlineTransform.rotation;
        outlineObject.layer = 7;
        outlineObject.GetComponent<MeshFilter>().mesh = dragRigidbodyScript.GetComponent<MeshFilter>().sharedMesh;
        outlineObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("M_outline2");

        Material[] mats = dragRigidbodyScript.GetComponent<MeshRenderer>().sharedMaterials;
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = Resources.Load<Material>("M_outline2");
            //highlightMaskObject.GetComponent<MeshRenderer>().sharedMaterials[i] = Resources.Load<Material>("M_mask");
        }
        
        outlineObject.GetComponent<MeshRenderer>().materials = mats;
        
        
        
        dragRigidbodyScript.outlineObject = outlineObject;
        
        var outlineMaskObject = new GameObject("OutlineMeshMask", typeof(MeshFilter), typeof(MeshRenderer));
        outlineMaskObject.transform.parent = dragRigidbodyScript.outlineObject.transform;
        outlineMaskObject.transform.position = dragRigidbodyScript.outlineTransform.position;
        outlineMaskObject.transform.rotation = dragRigidbodyScript.outlineTransform.rotation;
        outlineMaskObject.transform.localScale = new Vector3(0.99f, 0.99f, 0.99f);
        outlineMaskObject.layer = 6;
        outlineMaskObject.GetComponent<MeshFilter>().mesh = dragRigidbodyScript.GetComponent<MeshFilter>().sharedMesh;
        outlineMaskObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("M_mask");

        //mats = dragRigidbodyScript.GetComponent<MeshRenderer>().sharedMaterials;
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = Resources.Load<Material>("M_mask");
            //highlightMaskObject.GetComponent<MeshRenderer>().sharedMaterials[i] = Resources.Load<Material>("M_mask");
        }
        
        outlineMaskObject.GetComponent<MeshRenderer>().materials = mats;
        
        
        dragRigidbodyScript.outlineMaskObject = outlineMaskObject;
        
        var wireFrameObject = new GameObject("wireFrameObject", typeof(MeshFilter), typeof(MeshRenderer));
        wireFrameObject.transform.parent = dragRigidbodyScript.outlineObject.transform;
        wireFrameObject.transform.position = dragRigidbodyScript.outlineTransform.position;
        wireFrameObject.transform.rotation = dragRigidbodyScript.outlineTransform.rotation;
        wireFrameObject.GetComponent<MeshFilter>().mesh = dragRigidbodyScript.GetComponent<MeshFilter>().sharedMesh;
        wireFrameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("M_wireframe_outline");

        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = Resources.Load<Material>("M_wireframe_outline");
            //highlightMaskObject.GetComponent<MeshRenderer>().sharedMaterials[i] = Resources.Load<Material>("M_mask");
        }
        
        wireFrameObject.GetComponent<MeshRenderer>().materials = mats;
        
        dragRigidbodyScript.wireFrameObject = wireFrameObject;
    }
    
    /*
    void SetupHighlightObject()
    {
        if (!dragRigidbodyScript.GetComponent<MeshFilter>())
        {
            return;
        }

        if (!dragRigidbodyScript.GetComponent<MeshRenderer>())
        {
            return;
        }
        if (dragRigidbodyScript.highlightObject != null)
        {
            DestroyImmediate(dragRigidbodyScript.highlightObject);
            DestroyImmediate(dragRigidbodyScript.highlightMaskObject);
        }
        
        var highlightObject = new GameObject("HighlightMesh", typeof(MeshFilter), typeof(MeshRenderer));
        highlightObject.transform.parent = dragRigidbodyScript.transform;
        highlightObject.transform.position = dragRigidbodyScript.transform.position;
        highlightObject.transform.rotation = dragRigidbodyScript.transform.rotation;
        highlightObject.transform.localScale = new Vector3(1.04f, 1.04f, 1.04f);
        highlightObject.layer = 7;
        //highlightObject.AddComponent<MeshFilter>() = dragRigidbodyScript.GetComponent<MeshFilter>();
        highlightObject.GetComponent<MeshFilter>().mesh = dragRigidbodyScript.GetComponent<MeshFilter>().sharedMesh;
        highlightObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("M_outline2");

        Material[] mats = dragRigidbodyScript.GetComponent<MeshRenderer>().sharedMaterials;
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = Resources.Load<Material>("M_outline2");
            //highlightMaskObject.GetComponent<MeshRenderer>().sharedMaterials[i] = Resources.Load<Material>("M_mask");
        }
        
        highlightObject.GetComponent<MeshRenderer>().materials = mats;
        

        dragRigidbodyScript.highlightObject = highlightObject;
        
        var highlightMaskObject = new GameObject("OutlineMeshMask", typeof(MeshFilter), typeof(MeshRenderer));
        highlightMaskObject.transform.parent = highlightObject.transform;
        highlightMaskObject.transform.position = dragRigidbodyScript.transform.position;
        highlightMaskObject.transform.rotation = dragRigidbodyScript.transform.rotation;
        highlightMaskObject.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
        highlightMaskObject.layer = 6;
        highlightMaskObject.GetComponent<MeshFilter>().mesh = dragRigidbodyScript.GetComponent<MeshFilter>().sharedMesh;
        //highlightMaskObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("M_mask");


        //highlightMaskObject.GetComponent<MeshRenderer>().sharedMaterials = GetComponent<MeshRenderer>().sharedMaterials;
        
        Material[] mats2 = dragRigidbodyScript.GetComponent<MeshRenderer>().sharedMaterials;
        for (int i = 0; i < mats2.Length; i++)
        {
            mats2[i] = Resources.Load<Material>("M_mask");
            //highlightMaskObject.GetComponent<MeshRenderer>().sharedMaterials[i] = Resources.Load<Material>("M_mask");
        }
        
        highlightMaskObject.GetComponent<MeshRenderer>().materials = mats2;
        
        dragRigidbodyScript.highlightMaskObject = highlightMaskObject;
        
        dragRigidbodyScript.DeactivateHighlight();
    }
    */
    
    
    // public static void AddResource(dragRigidbodyScript ICollection<Material> materials, string resource)
    // {
    //     Material material = Resources.Load(resource, typeof(Material)) as Material;
    //     if (material)
    //         materials.Add(material);
    //     else
    //         Debug.LogWarning("Material Resource '" + resource + "' could not be loaded.");
    // }
    
}


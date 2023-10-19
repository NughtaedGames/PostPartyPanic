using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DragRigidbody)), CanEditMultipleObjects]
public class DragRigidbodyEditor : Editor
{
    private DragRigidbody dragRigidbodyScript;

    private SerializedProperty contactPointParent;
    private SerializedProperty snappingTransform;
    private void OnEnable()
    {
        contactPointParent = serializedObject.FindProperty("contactPointParent");
        snappingTransform = serializedObject.FindProperty("snappingTransform");
    }


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //Draggable
        EditorGUILayout.Space();
        
        dragRigidbodyScript = (DragRigidbody) target;

        //dragRigidbodyScript.canBeDragged = GUILayout.Toggle(dragRigidbodyScript.canBeDragged, "Can be Dragged");
        EditorGUILayout.PropertyField(contactPointParent);
        // }
        // else
        // {
        //     if (GUILayout.Button("Setup Draggable Object"))
        //     {
        //         ToggleDraggableObject();
        //     }
        // }

                
        //Transform
        EditorGUILayout.Space();
        
        EditorGUILayout.BeginHorizontal();
        if (dragRigidbodyScript.snappingTransform == null)
        {
            // if (GUILayout.Button("Setup Transform Object"))
            // {
            //     SetupOutlineTransform();
            // }
        }
        else
        {
            if (GUILayout.Button("Reset Transform Object"))
            {
                ResetSnappingTransform();
            }
            EditorGUILayout.PropertyField(snappingTransform);
        }
        EditorGUILayout.EndHorizontal();
        
        if (GUILayout.Button("Setup Snapping-Object"))
        {
            SetupSnappingTransform();
            SetupSnappingObject();
            //SetupHighlightObject();
        }
        
        if (GUILayout.Button("Setup contactPoints"))
        {
            SetupContactPoints();
        }
        
    }

    private void SetupSnappingTransform()
    {
        if (dragRigidbodyScript.snappingTransform == null)
        {
            foreach (Transform child in dragRigidbodyScript.transform)
            {
                if (child.name == "Transform")
                {
                    //dragRigidbodyScript.outlineTransform = child;
                    dragRigidbodyScript.snappingTransform = child;
                    break;
                }
            }

            if (dragRigidbodyScript.snappingTransform == null)
            {
                var transformObject = new GameObject("Transform");
                transformObject.transform.SetParent(dragRigidbodyScript.transform);
                transformObject.transform.localPosition = new Vector3(0, 0, 0);
                transformObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                //dragRigidbodyScript.outlineTransform = transformObject.transform;
                dragRigidbodyScript.snappingTransform = transformObject.transform;
            }
        }
    }

    private void ResetSnappingTransform()
    {
        dragRigidbodyScript.snappingTransform.position = new Vector3(0, 0, 0);
    }

    void SetupSnappingObject()
    {
        if (!dragRigidbodyScript.GetComponent<MeshFilter>())
        {
            return;
        }

        if (dragRigidbodyScript.snappingTransform != null)
        {
            
            DestroyImmediate(dragRigidbodyScript.snappingTransform);
            DestroyImmediate(dragRigidbodyScript.outlineMaskObject);
            DestroyImmediate(dragRigidbodyScript.wireFrameObject);
            // dragRigidbodyScript.outlineObject = null;
            // dragRigidbodyScript.outlineMaskObject = null;
            // dragRigidbodyScript.wireFrameObject = null;
        }
        //outlineObject
        var snappingObject = new GameObject("OutlineMesh", typeof(MeshFilter), typeof(MeshRenderer));
        snappingObject.transform.parent = dragRigidbodyScript.snappingTransform.transform;
        snappingObject.transform.position = dragRigidbodyScript.snappingTransform.position;
        snappingObject.transform.rotation = dragRigidbodyScript.snappingTransform.rotation;
        snappingObject.layer = 7;
        snappingObject.GetComponent<MeshFilter>().mesh = dragRigidbodyScript.GetComponent<MeshFilter>().sharedMesh;
        snappingObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("M_outline2");

        Material[] mats = dragRigidbodyScript.GetComponent<MeshRenderer>().sharedMaterials;
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = Resources.Load<Material>("M_outline2");
            //highlightMaskObject.GetComponent<MeshRenderer>().sharedMaterials[i] = Resources.Load<Material>("M_mask");
        }
        
        snappingObject.GetComponent<MeshRenderer>().materials = mats;
        
        
        
        dragRigidbodyScript.snappingTransform = snappingObject.transform;
        
        var outlineMaskObject = new GameObject("OutlineMeshMask", typeof(MeshFilter), typeof(MeshRenderer));
        outlineMaskObject.transform.parent = dragRigidbodyScript.snappingTransform;
        outlineMaskObject.transform.position = dragRigidbodyScript.snappingTransform.position;
        outlineMaskObject.transform.rotation = dragRigidbodyScript.snappingTransform.rotation;
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
        wireFrameObject.transform.parent = dragRigidbodyScript.snappingTransform;
        wireFrameObject.transform.position = dragRigidbodyScript.snappingTransform.position;
        wireFrameObject.transform.rotation = dragRigidbodyScript.snappingTransform.rotation;
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

    void SetupContactPoints()
    {
        int amountOfContactPoints = 4;
        var contactPointParent = new GameObject("contactPointParent");
        contactPointParent.transform.parent = dragRigidbodyScript.transform;
        contactPointParent.transform.localPosition = new Vector3(0, 0, 0);
        contactPointParent.transform.localEulerAngles = new Vector3(0, 0, 0);

        for (int i = 0; i < amountOfContactPoints; i++)
        {
            var contactPoint = new GameObject("contactPoint");
            contactPoint.transform.parent = contactPointParent.transform;
            contactPoint.transform.localPosition = new Vector3(0,0,0);
            contactPoint.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        dragRigidbodyScript.contactPointParent = contactPointParent.transform;

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

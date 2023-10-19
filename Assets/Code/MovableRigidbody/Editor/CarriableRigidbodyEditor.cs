using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CarriableRigidbody)), CanEditMultipleObjects]
public class CarriableRigidbodyEditor : Editor
{
    
    private CarriableRigidbody carriableRigidbodyScript;
    
    private SerializedProperty snappingTransform;
    
    private void OnEnable()
    {
        snappingTransform = serializedObject.FindProperty("snappingTransform");
    }
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //Draggable
        EditorGUILayout.Space();
        
        carriableRigidbodyScript = (CarriableRigidbody) target;

        //Transform
        EditorGUILayout.Space();
        
        EditorGUILayout.BeginHorizontal();
        if (carriableRigidbodyScript.snappingTransform != null)
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
        }

    }

    private void SetupSnappingTransform()
    {
        if (carriableRigidbodyScript.snappingTransform == null)
        {
            foreach (Transform child in carriableRigidbodyScript.transform)
            {
                if (child.name == "Transform")
                {
                    //dragRigidbodyScript.outlineTransform = child;
                    carriableRigidbodyScript.snappingTransform = child;
                    break;
                }
            }

            if (carriableRigidbodyScript.snappingTransform == null)
            {
                var transformObject = new GameObject("Transform");
                transformObject.transform.SetParent(carriableRigidbodyScript.transform);
                transformObject.transform.localPosition = new Vector3(0, 0, 0);
                transformObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                //dragRigidbodyScript.outlineTransform = transformObject.transform;
                carriableRigidbodyScript.snappingTransform = transformObject.transform;
            }
        }
    }
    
    private void ResetSnappingTransform()
    {
        carriableRigidbodyScript.snappingTransform.position = new Vector3(0, 0, 0);
    }
    
    void SetupSnappingObject()
    {
        if (!carriableRigidbodyScript.GetComponent<MeshFilter>())
        {
            return;
        }

        if (carriableRigidbodyScript.snappingTransform != null)
        {
            
            DestroyImmediate(carriableRigidbodyScript.snappingTransform);
            DestroyImmediate(carriableRigidbodyScript.outlineMaskObject);
            DestroyImmediate(carriableRigidbodyScript.wireFrameObject);
            // dragRigidbodyScript.outlineObject = null;
            // dragRigidbodyScript.outlineMaskObject = null;
            // dragRigidbodyScript.wireFrameObject = null;
        }
        //outlineObject
        var snappingObject = new GameObject("OutlineMesh", typeof(MeshFilter), typeof(MeshRenderer));
        snappingObject.transform.parent = carriableRigidbodyScript.snappingTransform.transform;
        snappingObject.transform.position = carriableRigidbodyScript.snappingTransform.position;
        snappingObject.transform.rotation = carriableRigidbodyScript.snappingTransform.rotation;
        snappingObject.layer = 7;
        snappingObject.GetComponent<MeshFilter>().mesh = carriableRigidbodyScript.GetComponent<MeshFilter>().sharedMesh;
        snappingObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("M_outline2");

        Material[] mats = carriableRigidbodyScript.GetComponent<MeshRenderer>().sharedMaterials;
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = Resources.Load<Material>("M_outline2");
            //highlightMaskObject.GetComponent<MeshRenderer>().sharedMaterials[i] = Resources.Load<Material>("M_mask");
        }
        
        snappingObject.GetComponent<MeshRenderer>().materials = mats;
        
        
        
        carriableRigidbodyScript.snappingTransform = snappingObject.transform;
        
        var outlineMaskObject = new GameObject("OutlineMeshMask", typeof(MeshFilter), typeof(MeshRenderer));
        outlineMaskObject.transform.parent = carriableRigidbodyScript.snappingTransform;
        outlineMaskObject.transform.position = carriableRigidbodyScript.snappingTransform.position;
        outlineMaskObject.transform.rotation = carriableRigidbodyScript.snappingTransform.rotation;
        outlineMaskObject.transform.localScale = new Vector3(0.99f, 0.99f, 0.99f);
        outlineMaskObject.layer = 6;
        outlineMaskObject.GetComponent<MeshFilter>().mesh = carriableRigidbodyScript.GetComponent<MeshFilter>().sharedMesh;
        outlineMaskObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("M_mask");

        //mats = dragRigidbodyScript.GetComponent<MeshRenderer>().sharedMaterials;
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = Resources.Load<Material>("M_mask");
            //highlightMaskObject.GetComponent<MeshRenderer>().sharedMaterials[i] = Resources.Load<Material>("M_mask");
        }
        
        outlineMaskObject.GetComponent<MeshRenderer>().materials = mats;
        
        
        carriableRigidbodyScript.outlineMaskObject = outlineMaskObject;
        
        var wireFrameObject = new GameObject("wireFrameObject", typeof(MeshFilter), typeof(MeshRenderer));
        wireFrameObject.transform.parent = carriableRigidbodyScript.snappingTransform;
        wireFrameObject.transform.position = carriableRigidbodyScript.snappingTransform.position;
        wireFrameObject.transform.rotation = carriableRigidbodyScript.snappingTransform.rotation;
        wireFrameObject.GetComponent<MeshFilter>().mesh = carriableRigidbodyScript.GetComponent<MeshFilter>().sharedMesh;
        wireFrameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("M_wireframe_outline");

        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = Resources.Load<Material>("M_wireframe_outline");
            //highlightMaskObject.GetComponent<MeshRenderer>().sharedMaterials[i] = Resources.Load<Material>("M_mask");
        }
        
        wireFrameObject.GetComponent<MeshRenderer>().materials = mats;
        
        carriableRigidbodyScript.wireFrameObject = wireFrameObject;
    }

}

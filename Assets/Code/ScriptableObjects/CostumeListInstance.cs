using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class CostumeListInstance : ScriptableObject
{
    [FormerlySerializedAs("costumeList")]
    public CostumeList value;
}

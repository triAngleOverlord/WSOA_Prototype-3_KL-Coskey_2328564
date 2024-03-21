using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class ObjectsDatabase : ScriptableObject
{
    public List<ObjectData> objectData;

    
}
[Serializable]
public class ObjectData
{
    [field: SerializeField]//display it in the inspector
    public string Name { get;private set; }//always access it but can only set it through the inspector
    [field: SerializeField]
    public int ID { get; private set; }
    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;
    [field: SerializeField]
    public GameObject Prefab { get; private set; }
}

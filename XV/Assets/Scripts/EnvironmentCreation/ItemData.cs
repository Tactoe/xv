using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemData
{
    public string PrefabName;
    public string ItemName;
    public List<string> ColorOverride;
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTags{Heavy, Light, Worker, Station, Storage, Vehicle};

[Serializable]
public class ItemData
{
    public string PrefabName;
    public string ItemName;
    public Color[] ColorOverride;
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale;
	public List<ItemTags> Tags;
}

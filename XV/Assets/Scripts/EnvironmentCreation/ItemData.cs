using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTags{Heavy, Light, Worker, Station, Storage, Vehicle, Furniture};

[Serializable]
public class ItemData
{
    public string PrefabName;
    public string ItemName;
    public List<string> ColorOverride;
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale;
	public List<ItemTags> Tags;
}

[Serializable]
public class TaskData
{
	public Vector3 Position;
	public bool HasParent;
	public int RelatedParentID;
	public int RelatedWorkerID;
	public string PrefabName;
	public string TaskDescription;
}

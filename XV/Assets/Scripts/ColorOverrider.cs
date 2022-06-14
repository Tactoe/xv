using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorOverrider : MonoBehaviour
{
    public List<RendererObject> ExtractMats(Transform target, List<RendererObject> i_MaterialList)
    {
        foreach (Transform child in target)
        {
            Renderer mesh = child.GetComponent<Renderer>();
            if (mesh != null)
            {
                for (int i = 0; i < mesh.sharedMaterials.Length; i++)
                {
                    if (!mesh.sharedMaterials[i].shader.name.Contains("Multiple"))
                        continue;
                    bool isNewMat = true;
                    for(int j = 0; j < i_MaterialList.Count; j++)
                    {
                        if (i_MaterialList[j].mat.color == mesh.sharedMaterials[i].color
                            && i_MaterialList[j].mat.name == mesh.sharedMaterials[i].name)
                        {
                            i_MaterialList[j].meshRenderers.Add(mesh);
                            i_MaterialList[j].meshRenderMatIndexes.Add(i);
                            isNewMat = false;
                            break;
                        }
                    }
                    if (isNewMat)
                    {
                        RendererObject tmp = new RendererObject(mesh.sharedMaterials[i], mesh, i);
                        i_MaterialList.Add(tmp);
                    }
                }
            }
            ExtractMats(child, i_MaterialList);
        }
        return i_MaterialList;
    }
    
    public void ApplyColorOverride(List<string> i_ColorOverride)
    {
        List<RendererObject> targetRenderers = ExtractMats(transform, new List<RendererObject>());
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        for (int i = 0; i < targetRenderers.Count; i++)
        {
            if (i_ColorOverride[i] == null || i_ColorOverride[i] == "")
                continue;
            for(int j = 0; j < targetRenderers[i].meshRenderers.Count; j++)
            {
                Color colorOverride = new Color();
                ColorUtility.TryParseHtmlString(i_ColorOverride[i], out colorOverride);
                Renderer meshRenderer = targetRenderers[i].meshRenderers[j];
                int materialIndex = targetRenderers[i].meshRenderMatIndexes[j];
                meshRenderer.GetPropertyBlock(propertyBlock);
                propertyBlock.SetColor("_Color", colorOverride);
                meshRenderer.SetPropertyBlock(propertyBlock, materialIndex);
            }

        }
    }
}

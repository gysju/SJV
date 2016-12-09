using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SplatMapShaderGUI : ShaderGUI 
{

	void DrawLayer(MaterialEditor editor, int i, MaterialProperty[] props, string[] keyWords, 
	  bool hasGloss, bool hasSpec, bool hasEmis, bool hasDistBlend)
	{
		EditorGUIUtility.labelWidth = 0f;
		var RGB_Nx = FindProperty ("_RGB_Nx" + i, props);
		var RHE_Ny = FindProperty ("_REH_Ny" + i, props);
		var tint = FindProperty("_Tint" + i, props);
		var smoothness = FindProperty("_Roughness" + i, props);
		var emissionMult = FindProperty("_EmissiveMult" + i, props);
		var emissionColor = FindProperty("_EmissiveColor" + i, props);
		var texScale = FindProperty("_TexScale" + i, props);
		var distUVScale = FindProperty("_DistUVScale" + i, props, false);

		editor.TexturePropertySingleLine(new GUIContent("Albedo + Nx"), RGB_Nx);
		editor.TexturePropertySingleLine(new GUIContent("Roughness + Emissive + Height + Ny"), RHE_Ny);
		editor.ShaderProperty(tint, "Tint");

		editor.ShaderProperty(smoothness, "Roughness");

		editor.ShaderProperty(emissionMult, "Emissive Multiplier");
		editor.ShaderProperty(emissionColor, "Emissive Color");

		editor.ShaderProperty(texScale, "Texture Scale");
		if (hasDistBlend)
		{
			editor.ShaderProperty(distUVScale, "Distance UV Scale");
		}

		if (i != 1)
		{
			editor.ShaderProperty(FindProperty("_Contrast"+i, props), "Interpolation Contrast");
		}
	}

   enum FlowChannel
   {
		None = 0,
		One,
		Two,
		Three,
		Four
   }

   string[] flowChannelNames = new string[]
   {
      "None", "One", "Two", "Three", "Four"
   };

   public override void OnGUI (MaterialEditor materialEditor, MaterialProperty[] props)
   {
      // get the current keywords from the material
      Material targetMat = materialEditor.target as Material;
      string[] keyWords = targetMat.shaderKeywords;

      int layerCount = 1;
      if (targetMat.shader.name == "VertexPainter/SplatBlend_1Layer")
      {
         layerCount = 1;
      }
      else if (targetMat.shader.name == "VertexPainter/SplatBlend_2Layer")
      {
         layerCount = 2;
      }
      else if (targetMat.shader.name == "VertexPainter/SplatBlend_3Layer")
      {
         layerCount = 3;
      }
      else if (targetMat.shader.name == "VertexPainter/SplatBlend_4Layer")
      {
         layerCount = 4;
      }
      
      FlowChannel fchannel = FlowChannel.None;
      if (keyWords.Contains("_FLOW1"))
         fchannel = FlowChannel.One;
      if (keyWords.Contains("_FLOW2"))
         fchannel = FlowChannel.Two;
      if (keyWords.Contains("_FLOW3"))
         fchannel = FlowChannel.Three;
      if (keyWords.Contains("_FLOW4"))
         fchannel = FlowChannel.Four;

      bool flowDrift = keyWords.Contains("_FLOWDRIFT");
      bool flowRefraction = keyWords.Contains("_FLOWREFRACTION");
      bool hasGloss = (HasTexture(layerCount, targetMat, "_GlossinessTex"));
      bool hasSpec = (HasTexture(layerCount, targetMat, "_SpecGlossMap"));
      bool hasEmis = (HasTexture(layerCount, targetMat, "_Emissive"));
      bool hasDistBlend = keyWords.Contains("_DISTBLEND");

      EditorGUI.BeginChangeCheck();

      int oldLayerCount = layerCount;
      layerCount = EditorGUILayout.IntField("Layer Count", layerCount);
      if (oldLayerCount != layerCount)
      {
         if (layerCount < 1)
            layerCount = 1;
         if (layerCount > 4)
            layerCount = 4;

         targetMat.shader = Shader.Find("VertexPainter/SplatBlend_" + layerCount + "Layer");
         
         return;
      }


      hasDistBlend = EditorGUILayout.Toggle("UV Scale in distance", hasDistBlend);
      var distBlendMin = FindProperty("_DistBlendMin", props);
      var distBlendMax = FindProperty("_DistBlendMax", props); 

      if (hasDistBlend)
      {
         materialEditor.ShaderProperty(distBlendMin, "Distance Blend Min");
         materialEditor.ShaderProperty(distBlendMax, "Distance Blend Max");

         // make sure max is at least min
         if (distBlendMin.floatValue > distBlendMax.floatValue)
         {
            distBlendMax.floatValue = distBlendMin.floatValue;
         }
         // make sure max is at least 1
         if (distBlendMax.floatValue <= 1)
         {
            distBlendMax.floatValue = 1;
         }
      }


      for (int i = 0; i < layerCount; ++i)
      {
         DrawLayer(materialEditor, i+1, props, keyWords, hasGloss, hasSpec, hasEmis, hasDistBlend);

         EditorGUILayout.Space();
      }

      EditorGUILayout.Space();

      fchannel = (FlowChannel)EditorGUILayout.Popup((int)fchannel, flowChannelNames);
      if (fchannel != FlowChannel.None)
      {

         var flowSpeed = FindProperty("_FlowSpeed", props);
         var flowIntensity = FindProperty("_FlowIntensity", props);
         var flowAlpha = FindProperty("_FlowAlpha", props);
         var flowRefract = FindProperty("_FlowRefraction", props);

         materialEditor.ShaderProperty(flowSpeed, "Flow Speed");
         materialEditor.ShaderProperty(flowIntensity, "Flow Intensity");
         materialEditor.ShaderProperty(flowAlpha, "Flow Alpha");
         if (layerCount > 1)
         {
            flowRefraction = EditorGUILayout.Toggle("Flow Refraction", flowRefraction);
            if (flowRefraction)
            {
               materialEditor.ShaderProperty(flowRefract, "Refraction Amount");
            }
         }
         flowDrift = EditorGUILayout.Toggle("Flow Drift", flowDrift);
      }

      if (EditorGUI.EndChangeCheck())
      {
         var newKeywords = new List<string>();

         newKeywords.Add("_LAYERS" + layerCount.ToString());
         if (hasDistBlend)
         {
            newKeywords.Add("_DISTBLEND");
         }
         if (HasTexture(layerCount, targetMat, "_Normal"))
         {
            newKeywords.Add("_NORMALMAP");
         }
         if (hasGloss)
         {
            newKeywords.Add("_METALLICGLOSSMAP");
         }
         if (hasEmis)
         {
            newKeywords.Add("_EMISSION"); 
         }
         if (fchannel != FlowChannel.None)
         {
            newKeywords.Add("_FLOW" + (int)fchannel);
         }

         if (flowDrift)
         {
            newKeywords.Add("_FLOWDRIFT");
         }
         if (flowRefraction && layerCount > 1)
         {
            newKeywords.Add("_FLOWREFRACTION");
         }
         targetMat.shaderKeywords = newKeywords.ToArray ();
         EditorUtility.SetDirty (targetMat);
      }
   } 

   bool HasTexture(int numLayers, Material mat, string key)
   {
      for (int i = 0; i < numLayers; ++i)
      {
         int index = i+1;
         string prop = key + index;
         if (mat.HasProperty(prop) && mat.GetTexture(prop) != null)
            return true;
      }
      return false;
   }

}

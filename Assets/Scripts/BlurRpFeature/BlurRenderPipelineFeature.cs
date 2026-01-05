using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurRenderPipelineFeature : ScriptableRendererFeature
{
    [SerializeField] private Shader blurShader;

    private Material blurMaterial;

    private BlurPass blurPass;

    public override void Create()
    {
        this.blurMaterial = CoreUtils.CreateEngineMaterial(this.blurShader);
        this.blurPass = new BlurPass(blurMaterial);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(this.blurPass);
    }
}

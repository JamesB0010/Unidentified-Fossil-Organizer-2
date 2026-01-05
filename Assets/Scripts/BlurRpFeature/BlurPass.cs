using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurPass : ScriptableRenderPass
{
    private Material blurPassMaterial;

    public BlurPass(Material material)
    {
        this.blurPassMaterial = material;
        
        base.renderPassEvent = RenderPassEvent.AfterRenderingTransparents; // or another appropriate stage
    }
    
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        this.blurPassMaterial.SetFloat("_blurSize" ,VolumeManager.instance.stack.GetComponent<BlurVolume>().BlurMag);
        if (VolumeManager.instance.stack.GetComponent<BlurVolume>().Enabled == false)
            return;
        
        CommandBuffer cmd = CommandBufferPool.Get("BlurPass");

        RenderTargetIdentifier source = renderingData.cameraData.renderer.cameraColorTarget;
        
        //this.blurPassMaterial.SetTexture("_MainTex", renderingData.cameraData.renderer.cameraColorTargetHandle);

        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
        int temporaryRT = Shader.PropertyToID("_TemporaryBlurRT");
        cmd.GetTemporaryRT(temporaryRT, descriptor);
    
        // Blit to temp RT with blur
        cmd.Blit(source, temporaryRT, blurPassMaterial);
    
        // Blit back to screen
        cmd.Blit(temporaryRT, source);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}

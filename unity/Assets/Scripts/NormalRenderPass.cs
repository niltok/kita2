using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class NormalRenderPass : ScriptableRenderPass
{
    private readonly Material _material = new(Shader.Find("Hidden/CopyNormal"));
    private RenderTexture _rt;

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        base.OnCameraSetup(cmd, ref renderingData);
        var desc = renderingData.cameraData.cameraTargetDescriptor;
        desc.depthBufferBits = 0;
        _rt = new RenderTexture(desc);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        base.OnCameraCleanup(cmd);
        _rt.Release();
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var renderer = renderingData.cameraData.renderer;
        var source = renderer.cameraColorTargetHandle;
        var cmd = CommandBufferPool.Get("Normal Pass");
        cmd.Blit(source, _rt, _material);
        cmd.Blit(_rt, source);
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}

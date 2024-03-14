using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorRenderFeature : ScriptableRendererFeature
{
    private RenderPass _renderPass;
    public RenderPassEvent @event = RenderPassEvent.AfterRenderingGbuffer;
    
    public override void Create()
    {
        _renderPass?.OnDestroy();
        _renderPass = new RenderPass(@event);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_renderPass);
    }

    private class RenderPass : BaseRenderPass
    {
        private readonly Material _mat = new(Shader.Find("Hidden/CopyColor"));
        private static readonly int ColorBuffer = Shader.PropertyToID("_GBuffer0");
        
        public RenderPass(RenderPassEvent @event)
        {
            renderPassEvent = @event;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var renderer = renderingData.cameraData.renderer;
            var camera = renderingData.cameraData.camera;
            var image = camera.GetComponent<CameraImage>();
            if (image == null || image.cameraTexture == null) return;
            var source = renderer.cameraColorTargetHandle;
            var cmd = CommandBufferPool.Get("ColorPass");
            
            cmd.Blit(image.cameraTexture, source);
            cmd.Blit(image.cameraTexture, ColorBuffer);
            
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
}
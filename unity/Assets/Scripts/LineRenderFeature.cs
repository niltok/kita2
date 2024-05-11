using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LineRenderFeature : ScriptableRendererFeature
{
    public RenderPassEvent @event = RenderPassEvent.AfterRenderingGbuffer;
    private RenderPass _renderPass;

    public override void Create()
    {
        _renderPass?.OnDestroy();
        _renderPass = new RenderPass(@event);
    }

    private void OnDestroy()
    {
        _renderPass?.OnDestroy();
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_renderPass);
    }

    private class RenderPass : BaseRenderPass
    {
        private readonly Material _material = new(Shader.Find("Custom/CopyLine"));
        private RenderTexture _rt;

        public RenderPass(RenderPassEvent @event)
        {
            renderPassEvent = @event;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            _rt = new RenderTexture(TargetSize.Width, TargetSize.Height, 0, 
                RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _rt?.Release();
            _rt = null;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var renderer = renderingData.cameraData.renderer;
            var source = renderer.cameraColorTargetHandle;
            var cmd = CommandBufferPool.Get("NormalPass");
            cmd.Blit(source, _rt, _material);
            cmd.Blit(_rt, source);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
}
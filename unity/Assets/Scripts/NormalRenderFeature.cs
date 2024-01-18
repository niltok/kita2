using UnityEngine.Rendering.Universal;

public class NormalRenderFeature : ScriptableRendererFeature
{
    public override void Create()
    {
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(new NormalRenderPass());
    }
}

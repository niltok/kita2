using System.Drawing;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public abstract class BaseRenderPass : ScriptableRenderPass
{
    protected Size TargetSize;
    public virtual void OnCreate() {}
    public virtual void OnDestroy() {}
    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        base.OnCameraSetup(cmd, ref renderingData);
        var desc = renderingData.cameraData.cameraTargetDescriptor;
        if (desc.width == TargetSize.Width && desc.height == TargetSize.Height) return;
        TargetSize = new Size(desc.width, desc.height);
        OnDestroy();
        OnCreate();
    }
}
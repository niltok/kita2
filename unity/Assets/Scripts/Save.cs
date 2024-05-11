using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Save : MonoBehaviour
{
     /// <summary>
    /// 生成相机照片并保存
    /// </summary>
    /// <param name="photographyCamera">相机</param>
    /// <param name="width">图像宽度</param>
    /// <param name="height">图像高度</param>
    /// <param name="path">保存路径</param>
    /// <param name="imageName">保存图片名字</param>
    public void CreateCameraCaptureAndSaveLocal(Camera photographyCamera,int width,int height, string path, string imageName){
        // 销毁之前的 RenderTexture 和 Texture2D
        if (photographyCamera.targetTexture != null){
            RenderTexture.ReleaseTemporary(photographyCamera.targetTexture);
            photographyCamera.targetTexture = null;
            RenderTexture.active = null;
        }

        // 创建 RenderTexture
        RenderTexture rt = new RenderTexture(width, height, 16, RenderTextureFormat.ARGB32);
        photographyCamera.targetTexture = rt;
        GL.Clear(true, true, Color.clear); // 清除颜色和深度缓冲区
        photographyCamera.Render();
        RenderTexture.active = rt;

        // 创建 Texture2D 并读取图像数据
        Texture2D image = new Texture2D(width, height, TextureFormat.ARGB32, false);
        image.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        image.Apply();

        // 重要：将 targetTexture 设置为 null，以便相机继续渲染到主屏幕
        photographyCamera.targetTexture = null;
        RenderTexture.active = null;

        // 检查保存路径是否为空或无效
        if (string.IsNullOrEmpty(path)){
            Debug.LogError("Invalid save path.");
            return;
        }

        // 如果文件夹不存在，则创建文件夹
        if (!Directory.Exists(path)){
            Directory.CreateDirectory(path);
        }

        // 保存图像到本地文件夹

        byte[] bytes = image.EncodeToJPG();
        if (bytes != null){
            string savePath = Path.Combine(path, imageName + ".jpg");

            File.WriteAllBytes(savePath, bytes);
            Debug.Log("Image saved successfully: " + savePath);
        }
        else{
            Debug.LogError("Failed to encode image to JPG.");
        }
    }

}

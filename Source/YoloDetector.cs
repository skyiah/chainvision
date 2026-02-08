using System;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ChainVision;

using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

public class YoloDetector : IDisposable
{
    private readonly InferenceSession _session;

    public YoloDetector(string modelPath)
    {
        var options = new SessionOptions();
        options.AppendExecutionProvider_CPU();
        _session = new InferenceSession(modelPath, options);
    }

    public float[] Predict(Image<Rgb24> image)
    {
        var input = YoloPreprocessor.Preprocess(image);

        var inputTensor = new DenseTensor<float>(input, new[] { 1, 3, 640, 640 });
        var inputs = NamedOnnxValue.CreateFromTensor("images", inputTensor);

        using var results = _session.Run(new[] { inputs });
        return results.First(r => r.Name == "output0").AsTensor<float>().ToArray(); // YOLOv11 output [1, 84, 8400] or similar
    }

    // TODO: Add post-processing (NMS, confidence filtering) to return a list of detections
    // Recommended: Refer to YoloDotNet project (GitHub: NickSwardh/YoloDotNet) for quick full post-processing

    public void Dispose() => _session.Dispose();
}
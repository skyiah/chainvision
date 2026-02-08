namespace ChainVision;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;

public static class YoloPreprocessor
{
    private const int InputSize = 640;

    public static float[] Preprocess(Image<Rgb24> image)
    {
        // 1. Letterbox resize (preserve aspect ratio with black padding)
        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new Size(InputSize, InputSize),
            Mode = ResizeMode.BoxPad,       // Black padding
            PadColor = SixLabors.ImageSharp.Color.Black
        }));

        // 2. Convert to float array [1, 3, 640, 640] (CHW format)
        var input = new float[3 * InputSize * InputSize];
        
        image.ProcessPixelRows(accessor =>
        {
            int idx = 0;
            for (int y = 0; y < InputSize; y++)
            {
                var row = accessor.GetRowSpan(y);
                for (int x = 0; x < InputSize; x++)
                {
                    var pixel = row[x];
                    input[idx + 0 * InputSize * InputSize] = pixel.R / 255.0f;     // R
                    input[idx + 1 * InputSize * InputSize] = pixel.G / 255.0f;     // G
                    input[idx + 2 * InputSize * InputSize] = pixel.B / 255.0f;     // B
                    idx++;
                }
            }
        });

        return input;
    }
}
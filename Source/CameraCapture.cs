using System;
using System.Diagnostics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ChainVision;

public static class CameraCapture
{
    private static readonly string CapturePath = "/tmp/latest.jpg";

    public static Image<Rgb24> CapturePhoto()
    {
        // libcamera-still parameters: no preview, immediate capture, no GUI
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "libcamera-still",
                Arguments = $"-o {CapturePath} --nopreview --immediate -n --width 3280 --height 2464", // Adjust resolution based on camera
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        process.WaitForExit();

        if (process.ExitCode != 0)
            throw new Exception("Photo capture failed");

        // Load JPEG with ImageSharp
        return Image.Load<Rgb24>(CapturePath);
    }
}
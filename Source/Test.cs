namespace ChainVision;

using System.IO.Ports;
using System.Text.Json;

public class Test
{
    public static void Main(string[] args)
    {
        var serial = new SerialPort("/dev/ttyUSB0", 57600) // Your LoRa serial device and baud rate
        {
            ReadTimeout = 5000,
            WriteTimeout = 500
        };
        serial.Open();


// Send
        var result = new DetectionResult(true, 5, 39.9, 116.3);
        var json = JsonSerializer.Serialize(result);
        serial.WriteLine(json + "\n"); // Add newline for easy parsing on receiver
    }

// Example result class
    public record DetectionResult(bool HasAnomaly, int Count, double Lat, double Lon);
}
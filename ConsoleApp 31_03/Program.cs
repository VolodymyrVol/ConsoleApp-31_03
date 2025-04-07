using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Server
{
    static async Task Main()
    {
        await RunServer(); 
    }

    static async Task RunServer()
    {
        using var server = new UdpClient(5555);
        Console.WriteLine("Сервер запущен...");
        
        while (true)
        {
            var result = await server.ReceiveAsync();
            var request = Encoding.UTF8.GetString(result.Buffer);
            if (request == "end")
                server.Close();
            var response = GetParts(request);
            Console.WriteLine(response.ToString());
            byte[] data = Encoding.UTF8.GetBytes(response);
            server.SendAsync(data, result.RemoteEndPoint);
        }
    }


  

    private static string GetParts(string request)
    {
        var parts = new (string, double)[]
        {
            ("Процессор", 8700),
            ("Видеокарта", 30000),
            ("Блок питания",  6000),
            ("Оперативная память", 8000)
        };

        foreach (var part in parts)
        {
            if (request == $"{part.Item1}")
            {
                return $"{part.Item1} = {part.Item2}";
            }
        }
        return "Ошибка: Комплектующее не найдено";
    }
}

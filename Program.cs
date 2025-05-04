using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Server
{
    static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 5000);
        server.Start();
        Console.WriteLine("Сервер запущен...");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            ThreadPool.QueueUserWorkItem(HandleClient, client);
        }
    }

    private static void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        IPEndPoint clientEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
        Console.WriteLine($"Клиент подключился: {clientEndPoint.Address}:{clientEndPoint.Port}");
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[256];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string request = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
        Console.WriteLine($"Запрос от клиента: {request}");
        string response = GetExchangeRate(request);
        byte[] responseData = Encoding.UTF8.GetBytes(response);
        stream.Write(responseData, 0, responseData.Length);
        Console.WriteLine($"Отправлен курс: {response}");
        client.Close();
        Console.WriteLine($"Клиент отключился: {clientEndPoint.Address}:{clientEndPoint.Port}");
    }

    private static string GetExchangeRate(string request)
    {
        var rates = new (string, string, double)[]
        {
            ("USD", "EUR", 0.92),
            ("EUR", "USD", 1.09),
            ("GBP", "USD", 1.27),
            ("USD", "GBP", 0.79),
            ("EUR", "GBP", 0.86),
            ("GBP", "EUR", 1.16)
        };

        foreach (var rate in rates)
        {
            if (request == $"{rate.Item1} {rate.Item2}")
            {
                return $"{rate.Item1} -> {rate.Item2} = {rate.Item3}";
            }
        }
        return "Ошибка: Курс не найден";
    }
}

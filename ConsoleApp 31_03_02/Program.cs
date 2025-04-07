using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Сlient
{
    static async Task Main()
    {
        while (true)
        {
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("Введите название комплектующего:");
                string input = Console.ReadLine();
                try
                {
                    using var udpClient = new UdpClient();
                    udpClient.Connect("127.0.0.1", 5555);
                    byte[] data = Encoding.UTF8.GetBytes(input);
                    udpClient.SendAsync(data, data.Length);
                    var ans = await udpClient.ReceiveAsync();
                    var result = Encoding.UTF8.GetString(ans.Buffer);
                    Console.WriteLine(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
            Console.WriteLine("Вы не можете отправлять запросы некоторое время");    
            Thread.Sleep(10000);
        }
    }
}

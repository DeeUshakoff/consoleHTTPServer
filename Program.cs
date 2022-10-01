namespace consoleHTTPServer;
public static class Program
{
    [STAThreadAttribute]
    public static void Main()
    {
        var server = new HTTPServer();

        while (true)
            _ =  ReadCommands();
        async Task ReadCommands()
        {
            "\nCommands:\nStart | Stop | Restart | Listen | Status | Clear".Print();
            switch (Console.ReadLine()?.ToLower())
            {
                case "start":
                    server.Start();
                    break;
                case "stop":
                    server.Stop();
                    break;
                case "restart":
                    server.Stop();
                    server.Start();
                    break;
                case "listen":
                    await server.Listen();
                    break;
                case "status":
                    server.GetStatus.Print();
                    break;
                case "clear":
                    Console.Clear();
                    break;
                default:
                    Console.Clear();
                    "\nPlease, use on of the following commands:".Print(ConsoleColor.Yellow);
                    break;
            }
        }

    }
    
}
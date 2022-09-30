using System.Net;

namespace consoleHTTPServer
{
    public class HTTPServer
    {
        private HttpListener? listener;
        string responseText = "";
        public void Start()
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = "Open HTML file",
                Filter = "HTML file|*.html"
            };


            var html_lines = new List<string>();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                html_lines = File.ReadAllLines(dialog.FileName).ToList();
            }

            var css = Directory.GetDirectories(Path.GetDirectoryName(dialog.FileName))
                .Select(x => Directory.GetFiles(x))
                .SelectMany(x => x)
                .Where(x => Path.GetExtension(x) == ".css")
                .Select(x => File.ReadAllText(x));

            foreach (var cssCode in css)
                for (var i = 0; i < html_lines.Count; i++)
                    if (html_lines[i].Contains("rel=\"stylesheet\""))
                    {
                        html_lines[i] = "<style type=\"text/css\">" + cssCode + "</style>";
                        break;
                    }

            responseText = String.Join("\n", html_lines.Where(x => !x.Contains("rel=\"stylesheet\"")));

            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8888/");
            listener.Prefixes.Add("http://localhost:8888/google/");
            listener.Start();
            "Started...".Print(ConsoleColor.Green);
        }
        
        public async Task Listen()
        {
            if (listener == null)
            {
                "Listener is null, please use \"Start\" command before".Print(ConsoleColor.Red);
                return;
            }
            "Waiting for connection...".Print(ConsoleColor.Yellow);

            while (true)
            {
                HttpListenerContext context = await listener.GetContextAsync();
                _ = context.Request;
                HttpListenerResponse response = context.Response;

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseText);

                response.ContentLength64 = buffer.Length;
                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
                context.Response.Close();

                "Request received".Print(ConsoleColor.Green);
            }
        }

        public void Stop()
        {
            listener?.Stop();
            "Listener stopped".Print(ConsoleColor.Yellow);
        }

    }

}

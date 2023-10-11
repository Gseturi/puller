using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.Text;
using System.Text.RegularExpressions;

internal class Program
{
    private static async Task Main(string[] args)
    {
        string url = "";

        Console.WriteLine("1 for Url 2 for pdf-TextFile");
        string tec = Console.ReadLine();
        tec = "1";

        if (tec == "1")
        {

            Console.WriteLine("paste URL and Api key");
            string Url = Console.ReadLine();
            string ApiKey = Console.ReadLine();

            string text = Cleaner.scraper(Url);

            HttpClient client = new HttpClient();
            string command = "summerize this text";


            await Cleaner.postreturn(client, command, "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", "gpt-3.5-turbo", "https://api.openai.com/v1/chat/completions");

        }
        else
        {
            string pdfFilePath = "path_to_your_pdf.pdf";
            string extractedText = ExtractTextFromPdf(pdfFilePath);
            Console.WriteLine(extractedText);

            static string ExtractTextFromPdf(string filePath)
            {
                StringBuilder text = new StringBuilder();

                using (PdfReader pdfReader = new PdfReader(filePath))
                {
                    for (int page = 1; page <= pdfReader.GetNumberOfPages(); page++)
                    {
                        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                        string pageText = PdfTextExtractor.GetTextFromPage(pdfReader.GetPage(page), strategy);
                        text.Append(pageText);
                    }
                }

                return text.ToString();
            }


        }
    }
}

public static class Cleaner {

    public class GptPost
     {
                 public GptPost(string pass,string model)
         {
             this.model = model;
             messege = pass;
             role = "you are a good summerizer";
 ;       }
         public  string model;
         public string role;
         public  string messege;

     }



    public static async Task postreturn(HttpClient client,string pass, string key, string model, string apiUrl)
     {
             GptPost x=new GptPost(pass,model);


         HttpClient Client;
        Client = client;
         client.DefaultRequestHeaders.Add("Authorization", "Bearer"+" "+key);
         string jsonRequestData = Newtonsoft.Json.JsonConvert.SerializeObject(x);
         var content = new StringContent(jsonRequestData, System.Text.Encoding.UTF8, "application/json");
         var response =await client.PostAsync(apiUrl, content);
         var responseContent = response.Content.ReadAsStringAsync();
         Console.WriteLine(responseContent); // You should see the response


     }
    
   /* public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    public class GptPost
    {
        public string model { get; set; }
        public List<Message> messages { get; set; }

        public GptPost(string pass, string model)
        {
            this.model = model;
            messages = new List<Message>
        {
            new Message
            {
                role = "you are a good summerizer",
                content = pass
            }
        };
        }
    }

    public static async Task postreturn(string pass, string key, string model, string apiUrl)
    {
        GptPost x = new GptPost(pass, model);

        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + key);

        string jsonRequestData = Newtonsoft.Json.JsonConvert.SerializeObject(x);
        var content = new StringContent(jsonRequestData, System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync(apiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent); // You should see the response
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }
    }*/
    public static string scraper(string Url)
    {
        string text, text2 = "";
        int dec = 0;
        int counter = 0;

        using (HttpClient client = new HttpClient())
        {
            var result = client.GetAsync(Url).Result.Content;
            var jason = result.ReadAsStringAsync().Result;

            text = jason + " ";


        }
        text = Regex.Replace(text, "script", "~");

        for (int i = 0; i < text.Length; i++)
        {
            if (counter == 2)
            {
                counter = 0;
                dec = 1;
            }

            if (text[i] == '~')
            {
                dec = 3;
            }

            if (text[i] == '<')
            {
                dec = 0;

            }
            else
            if (text[i] == '>')
            {

                dec = 1;
                counter++;
            }

            if (dec == 1 && counter == 0)
            {
                text2 = text2 + text[i];
            }

        }
        StringBuilder sb = new StringBuilder();
        counter = 0;
        dec = 0;

        string[] words = text2.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        string reuslt = string.Join(" ", words);
        text = " ";
        for (int i = 0; i < words.Length; i++)
        {
            text = text + " " + words[i];
        }
        return text;
    }

}

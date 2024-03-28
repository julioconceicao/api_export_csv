using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

class Program
{
    static async Task Main(string[] args)
    {
        string url = "https://economia.awesomeapi.com.br/json/last/USD-BRL";

        using (HttpClient client = new HttpClient())
        {
            // Definindo cabeçalhos
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            // Fazendo a requisição GET
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                JsonDocument jsonDocument = JsonDocument.Parse(jsonString);

                // Obtendo dados
                JsonElement usdData = jsonDocument.RootElement.GetProperty("USDBRL");

                // Criando lista para armazenar os dados
                List<string> listing = new List<string>
                {
                    usdData.GetProperty("code").GetString(),
                    usdData.GetProperty("codein").GetString(),
                    usdData.GetProperty("name").GetString(),
                    usdData.GetProperty("high").GetString(),
                    usdData.GetProperty("low").GetString(),
                    usdData.GetProperty("varBid").GetString(),
                    usdData.GetProperty("pctChange").GetString(),
                    usdData.GetProperty("bid").GetString(),
                    usdData.GetProperty("ask").GetString(),
                    usdData.GetProperty("timestamp").GetString(),
                    usdData.GetProperty("create_date").GetString()
                };

                // Escrevendo os dados em um arquivo CSV
                using (StreamWriter file = new StreamWriter("crypto.csv"))
                {
                    await file.WriteLineAsync("code,codein,name,high,low,varBid,pctChange,bid,ask,timestamp,create_date");
                    await file.WriteLineAsync(string.Join(",", listing));
                }

                Console.WriteLine("CSV criado com sucesso!");
            }
            else
            {
                Console.WriteLine("Falha ao obter os dados: " + response.ReasonPhrase);
            }
        }
    }
}


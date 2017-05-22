using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FakePoll.Helpers;
using Newtonsoft.Json.Linq;

namespace FakePoll
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();
            Task.Run(ActionAsync).Wait();
            Console.WriteLine("Czas pracy " + stopwatch.Elapsed);
            Console.WriteLine("Nacisnij dowolny przycisk aby wyjsc z programu.");
            Console.ReadKey();
        }

        private static async Task ActionAsync()
        {
            Console.Write("Podaj ID ankiety:");
            var id = Console.ReadLine();
            var httpClient = HttpClientHelper.CreateHttpClient(WebProxy.GetDefaultProxy(), 10);

            var poll = await StrawpollRequests.GetPollAsync(httpClient, id);
            if (!poll.Votes.Any())
                throw new Exception("Podales niepoprawne ID ankiety!");

            for (int i = 0; i < poll.Votes.Count(); i++)
                Console.WriteLine($"ID:{i} Vote:{poll.Votes.ElementAt(i).Name}");
           
            Console.Write($"Wpisz ID vota:");

            int voteId = 0;
            if (!int.TryParse(Console.ReadLine(), out voteId) && voteId >= poll.Votes.Count())
                throw new Exception("Podałeś niepoprawne ID vota!");


            Console.Write("Podaj sciezke do pliku z proxy:");
            var proxyPath = Console.ReadLine();

            if (proxyPath == null)
                throw new NullReferenceException("Podana sciezka jest bledna!");

            if (!File.Exists(proxyPath))
                throw new Exception("Podany plik nie istnieje!");

            Console.Write("Max liczba glosow:");
            int max = 0;
            if (!int.TryParse(Console.ReadLine(), out max))        
                throw new Exception("Podałeś niepoprawna liczbe głosow!");
            

            var lines = File.ReadAllLines(proxyPath);
            int total = lines.Length;
            int success = 0;
            int fail = 0;
            int threads = 0;

            Console.Write("Wpisz ilosc watkow:");
            if (!int.TryParse(Console.ReadLine(), out threads))
                throw new Exception("Podales nieporawna liczbe watkow!");


            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = threads,
            };

            Parallel.ForEach(lines, options, address =>
            {
                Task.Run(async () =>
                {
                    if (success <= max)
                    {
                        Console.WriteLine($"SUCKES:{success} FAIL:{fail} WOLNE:{Math.Abs((success + fail) - total)} WSZYSTKIE:{total}");
                        if (await ProxyHelpers.CheckProxyAsync(address))
                        {
                            try
                            {
                                    var client = HttpClientHelper.CreateHttpClient(new WebProxy(address), 10);
                                    var info = await StrawpollRequests.GetPollAsync(client, id);
                                    if (await StrawpollRequests.Vote(client, id, poll.Votes.ElementAt(voteId).ID, info.Token))
                                       success++;
                            }
                            catch (Exception)
                            {
                                fail++;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Proxy nie odpowiada!");
                            fail++;
                        }
                    }
                }).Wait();
            });
        }
    }
}


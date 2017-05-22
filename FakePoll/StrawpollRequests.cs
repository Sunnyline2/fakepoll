using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FakePoll.Helpers;

namespace FakePoll
{
    public static class StrawpollRequests   
    {
        private static async Task<bool> GetStatus(string value)
        {
            try
            {
                var result = await JsonHelpers.DeserializeJObjectAsync(value);
                return result["success"].ToObject<string>().Equals("success");
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> Vote(HttpClient httpClient, string id, string voteId, string token)
        {
            var body = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("security-token", token),
                new KeyValuePair<string, string>("options", voteId),

            });
            var responseMessage = await httpClient.PostAsync($"http://www.strawpoll.me/{id}", body);
            return await GetStatus(await responseMessage.Content.ReadAsStringAsync());
        }

        public static async Task<Poll> GetPollAsync(HttpClient httpClient, string id)
        {
            var responseMessage = await httpClient.GetAsync($"http://www.strawpoll.me/embed_1/{id}");
            var decode = WebUtility.HtmlDecode(await responseMessage.Content.ReadAsStringAsync());

            string title = RegexHelpers.GetValue(decode, "<h2>(.+)<\\/h2>").Groups[1].Value;
            string token = RegexHelpers.GetValue(decode, "name=\"security-token\" type=\"hidden\" value=\"(.+)\"").Groups[1].Value;
            IEnumerable<Vote> votes = GetVotes();

            IEnumerable<Vote> GetVotes()
            {
                var matches = RegexHelpers.GetValues(decode, "<input type=\"radio\"\\s+ name=\"options\"\\s+ value=\"(.+)\"\\s+ id=\"field-options-.+\"  \\/>\\s+<label title=\"\"\\s+ id=\"field-options-.+\" for=\"field-options-.+\" ><span>(.+)<\\/span><\\/label>\r\n");
                foreach (Match match in matches)
                    yield return new Vote(match.Groups[1].Value, match.Groups[2].Value);
            }

            return new Poll(title, votes, token);
        }

        public static async Task GetPollResultAsync(HttpClient httpClient, string id)
        {
            throw new NotImplementedException();
        }
    }
}
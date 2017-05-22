using System.Collections.Generic;

namespace FakePoll
{
    public class Poll
    {
        public string Title { get; }
        public IEnumerable<Vote> Votes { get; }
        public string Token { get; }

        public Poll(string title, IEnumerable<Vote> votes, string token)
        {
            Title = title;
            Votes = votes;
            Token = token;
        }
    }
}
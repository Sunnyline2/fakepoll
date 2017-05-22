using System.Text.RegularExpressions;

namespace FakePoll.Helpers
{
    public static class RegexHelpers
    {
        public static Match GetValue(string input, string pattern)
        {
            var regex = new Regex(pattern);
            return regex.Match(input);
        }

        public static MatchCollection GetValues(string input, string pattern)
        {
            var regex = new Regex(pattern);
            return regex.Matches(input);
        }
    }
}
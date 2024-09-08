using System;
using System.Text.RegularExpressions;

namespace API.Repos.Helpers
{
	public static class RegexHelpers
	{

        public static string ExtractEmailAddress(string from)
        {
            // Regular expression to extract email address
            string pattern = @"\<(.*?)\>";
            Match match = Regex.Match(from, pattern);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            // If no match found, return the original string
            return from;
        }
    }
}


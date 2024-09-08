namespace API.Repos.Helpers
{
    public static class StaticHelpers
    {
        public static string NormalizeLeadNo(string leadNo)
        {
            leadNo = leadNo.TrimStart('0');
            if (string.IsNullOrEmpty(leadNo) || !int.TryParse(leadNo, out _))
            {
                leadNo = "";
            }

            return leadNo;
        }
    }
}

public class Parser
{
    public static List<string> ParseASTM(string rawData)
    {
        List<string> results = new();
        var lines = rawData.Split('\n');
        foreach(var line in lines)
        {
            if(line.StartsWith("R|")) // Segmento de resultado
            {
                var fields = line.Split('|');
                string testName = fields[2];
                string value = fields[3];
                results.Add($"{testName}|{value}");
            }
        }
        return results;
    }
    
}

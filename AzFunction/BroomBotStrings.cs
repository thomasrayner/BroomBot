using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Reflection;

namespace BroomBot
{
    public class BroomBotStrings
    {
        public string PullRequestIsStale { get; set; }
        public string BroomBotName { get; set; }
        public string WarningPrefix { get; set; }
        public string StaleAge { get; set; }
        public string WarningCount { get; set; }

        public BroomBotStrings(string configFile)
        {
            var json = File.ReadAllText(configFile);

            var objects = JArray.Parse(json); // parse as array  
            foreach (JObject root in objects)
            {
                foreach (KeyValuePair<string, JToken> app in root)
                {
                    PropertyInfo info = GetType().GetProperty((string)app.Value["Name"]);
                    info.SetValue(this, (string)app.Value["Value"]);
                }
            }
        }
    }
}
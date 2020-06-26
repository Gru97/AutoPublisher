using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoPublisher.Models
{
    public class RunnerConfig
    {
        public RunnerConfig()
        {
            ScriptPath = new Dictionary<string, string>();
        }
        public Dictionary<string, string> ScriptPath { get; set; }
    }
}

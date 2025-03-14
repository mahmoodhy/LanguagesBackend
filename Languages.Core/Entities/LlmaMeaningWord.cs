using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Choice
    {
        public string finish_reason { get; set; }
        public int index { get; set; }
        public object logprobs { get; set; }
        public Message message { get; set; }
    }
    public class Content
    {
        public string Persian { get; set; }
        public string Meaning { get; set; }
        public string Example { get; set; }
    }
    public class Message
    {
        public string content { get; set; }
        public object refusal { get; set; }
        public string role { get; set; }
        public object function_call { get; set; }
        public object tool_calls { get; set; }
    }

    public class LlmaMeaningWord
    {
        public int created { get; set; }
        public string model { get; set; }
        public Usage usage { get; set; }
        public List<Choice> choices { get; set; }
    }

    public class Usage
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }

}

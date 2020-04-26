using System;

namespace Models
{
    public class Story
    {
        public string StoryID { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTimeOffset Time { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
    }
}

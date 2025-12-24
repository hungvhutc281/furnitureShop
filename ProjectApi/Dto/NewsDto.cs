using System;

namespace ProjectApi.Dto
{
    public class NewsDto
    {
        public int NewsID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public DateTime PostedDate { get; set; }
        public string Author { get; set; }
    }

    public class CreateNewsDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string Author { get; set; }
    }

    public class UpdateNewsDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string Author { get; set; }
    }
} 
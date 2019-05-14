using System.Web.Mvc;

namespace WBS.Web.Models
{
    [Bind(Exclude = "UniqueKey")]
    public class StatusMessageViewModel
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
    }
}
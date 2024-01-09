using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace Route.DAL.Models
{
	public class Email:BaseEntity
	{
        public int Id { get; set; }
        public string Subject { get; set; }
		public string Recipients { get; set; }
        public string To { get; set; }

        public string Body { get; set; }

        IList<IFormFile>? Attachments { get; set; } = null;
    }
}

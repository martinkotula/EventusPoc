using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventusPoc.Core.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        [Display(Name="Abonamentowy?")]
        public bool IsSubscribed { get; set; }
    }
}

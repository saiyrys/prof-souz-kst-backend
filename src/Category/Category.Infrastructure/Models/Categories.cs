using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Category.Infrastructure.Models
{
    public class Categories
    {
        [Column("id")]
        public string categoriesId { get; set; } = null;
        public string name { get; set; } = null;

    }
}

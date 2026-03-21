using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pract_15.Models;

public partial class ProductTag
{
    public double ProductId { get; set; }

    public double? TagId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Tag? Tag { get; set; }

}

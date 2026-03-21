using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pract_15.Models;

public partial class Tag
{
    public double Id { get; set; }

    public string? Name { get; set; }

}

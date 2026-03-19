using System;
using System.Collections.Generic;

namespace Pract15.Models;

public partial class Form
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int Age { get; set; }

    public int BirthdayYear { get; set; }

    public int Height { get; set; }

    public double Weight { get; set; }
}

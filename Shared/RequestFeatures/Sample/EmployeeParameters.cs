﻿using Shared.RequestFeatures.Base;

namespace Shared.RequestFeatures.Sample;

public class EmployeeParameters : RequestParameters
{
    public EmployeeParameters() => OrderBy = "fullName";

    public uint MinAge { get; set; }
    public uint MaxAge { get; set; } = int.MaxValue;
    
    public bool ValidAgeRange => MaxAge > MinAge;

    public string? SearchTerm { get; set; }
}

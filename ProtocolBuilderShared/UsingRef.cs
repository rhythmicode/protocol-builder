using System;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
public class UsingRef : Attribute
{
    public string Name { get; set; }
    public string[] RelativePathSegments { get; set; }
    public UsingRef(string name)
    {
        Name = name;
    }
    public UsingRef(string name, params string[] relativePathSegments)
    {
        Name = name;
        RelativePathSegments = relativePathSegments;
    }
}

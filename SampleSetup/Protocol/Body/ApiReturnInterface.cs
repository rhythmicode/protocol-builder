using System.Collections.Generic;

namespace SampleProject.Protocol.Body
{
    [UsingRef(nameof(AbstractReturn))]
    public interface ApiReturnInterface
    {
        AbstractReturn Payload();
        string? Meta();
        string? Fill(string? p1, List<int> p2, List<double> p3, int? p4);
    }
}

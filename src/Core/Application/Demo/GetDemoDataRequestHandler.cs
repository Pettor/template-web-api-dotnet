namespace Backend.Application.Demo;

public class GetDemoDataRequestHandler : IRequestHandler<GetDemoDataRequest, DemoData>
{
    public async Task<DemoData> Handle(GetDemoDataRequest request, CancellationToken cancellationToken)
    {
        return new DemoData {Data = "Hello World"};
    }
}

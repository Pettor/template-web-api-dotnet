using Backend.Application.Common.Interfaces;

namespace Backend.Application.Common.Exporters;

public interface IExcelWriter : ITransientService
{
    Stream WriteToStream<T>(IList<T> data);
}

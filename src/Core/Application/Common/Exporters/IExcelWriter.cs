using WebApiTemplate.Application.Common.Interfaces;

namespace WebApiTemplate.Application.Common.Exporters;

public interface IExcelWriter : ITransientService
{
    Stream WriteToStream<T>(IList<T> data);
}
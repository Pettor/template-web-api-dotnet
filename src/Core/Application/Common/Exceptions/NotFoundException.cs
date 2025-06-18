using System.Net;

namespace Backend.Application.Common.Exceptions;

public class NotFoundException(string message)
    : CustomException(message, null, HttpStatusCode.NotFound);

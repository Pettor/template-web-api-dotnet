using System.Net;

namespace Backend.Application.Common.Exceptions;

public class UnauthorizedException(string message)
    : CustomException(message, null, HttpStatusCode.Unauthorized);

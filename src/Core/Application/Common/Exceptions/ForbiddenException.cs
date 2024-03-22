using System.Net;

namespace Backend.Application.Common.Exceptions;

public class ForbiddenException(string message) : CustomException(message, null, HttpStatusCode.Forbidden);

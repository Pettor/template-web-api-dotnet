using System.Net;

namespace Backend.Application.Common.Exceptions;

public class ConflictException(string message) : CustomException(message, null, HttpStatusCode.Conflict);

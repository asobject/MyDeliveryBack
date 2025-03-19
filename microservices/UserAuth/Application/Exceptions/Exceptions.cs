

namespace Application.Exceptions;

public class InvalidTokenException(string message) : Exception(message) { }
public class NotFoundException(string message) : Exception(message) { }
public class PasswordIncorrectException(string message) : Exception(message) { }
public class AlreadyExistsException(string message) : Exception(message) { }
public class UserException(string message) : Exception(message) { }
public class AccessDeniedException(string message) : Exception(message) { }
public class ConflictException(string message) : Exception(message) { }
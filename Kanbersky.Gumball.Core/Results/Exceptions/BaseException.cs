using System;

namespace Kanbersky.Gumball.Core.Results.Exceptions
{
    public class BaseException : Exception
    {
        public static NotFoundException NotFoundException()
        {
            return new NotFoundException();
        }

        public static NotFoundException NotFoundException(string message)
        {
            return new NotFoundException(message);
        }

        public static NotFoundException NotFoundException(string message, Exception ex)
        {
            return new NotFoundException(message, ex);
        }

        public static BadRequestException BadRequestException()
        {
            return new BadRequestException();
        }

        public static BadRequestException BadRequestException(string message)
        {
            return new BadRequestException(message);
        }

        public static BadRequestException BadRequestException(string message, Exception ex)
        {
            return new BadRequestException(message, ex);
        }
    }
}

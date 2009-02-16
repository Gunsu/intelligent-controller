#ifndef EXCEPTIONS_H
#define EXCEPTIONS_H

#include <string>

/*******************************
*           Exception
********************************/

class Exception
{
	protected:
		std::string errorMes;

	public:
		Exception (std::string errorMessage) : errorMes (errorMessage) {}
		std::string GetErrorMessage () { return errorMes; }
};

/*******************************
*     CompilationException
********************************/

class CompilationException : public Exception
{
	public:
		CompilationException (std::string errorMessage) : Exception (errorMessage) {}
};

/*******************************
*     ValidationException
********************************/

class ValidationException : public Exception
{
	public:
		ValidationException (std::string errorMessage) : Exception (errorMessage) {}
};

/*******************************
*     BlockFactoryException
********************************/

class BlockFactoryException : public Exception
{
	public:
		BlockFactoryException (std::string errorMessage) : Exception (errorMessage) {}
};

/*******************************
*       ProjectException
********************************/

class ProjectException : public Exception
{
	public:
		ProjectException (std::string errorMessage) : Exception (errorMessage) {}
};

#endif
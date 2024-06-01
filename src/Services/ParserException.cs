namespace Bau.Libraries.LibMarkupLanguage.Services;

/// <summary>
///		Excepciones del intérprete
/// </summary>
public class ParserException : Exception
{
	public ParserException() {}

	public ParserException(string message) : base(message) { }

	public ParserException(string message, Exception innerException) : base(message, innerException) { }

	public ParserException(string message, string fileName, Exception innerException) : base(message, innerException)
	{	
		FileName = fileName;
	}

	/// <summary>
	///		Nombre del archivo en el que se encuentra la excepción
	/// </summary>
	public string FileName { get; }
}
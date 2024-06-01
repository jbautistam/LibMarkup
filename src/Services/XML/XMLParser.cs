using System.Xml;

namespace Bau.Libraries.LibMarkupLanguage.Services.XML;

/// <summary>
///		Interpreta un archivo XML
/// </summary>
public class XMLParser : IParser
{
	public XMLParser(bool includeComments = false)
	{
		IncludeComments = includeComments;
	}

	/// <summary>
	///		Carga un archivo
	/// </summary>
	public MLFile Load(string fileName)
	{
		if (!File.Exists(fileName))
			return new MLFile();
		else
			try
			{
				return ParseText(Files.FileTools.LoadTextFile(fileName));
			}
			catch (Exception exception)
			{
				throw new ParserException($"Error when parse XML file {fileName}", fileName, exception);
			}
	}

	/// <summary>
	///		Interpreta un texto XML
	/// </summary>
	public MLFile ParseText(string text)
	{
		if (string.IsNullOrWhiteSpace(text))
			return new MLFile();
		else
			return Load(XmlReader.Create(new StringReader(text), GetXmlReaderSettings()));
	}

	/// <summary>
	///		Obtiene las setting para la lectura de XML
	/// </summary>
	private XmlReaderSettings GetXmlReaderSettings()
	{
		XmlReaderSettings settings = new();

			// Carga el documento
			settings.CheckCharacters = false;
			settings.CloseInput = true;
			settings.DtdProcessing = DtdProcessing.Parse;
			settings.MaxCharactersFromEntities = 1024;
			// Devuelve la configuración
			return settings;
	}

	/// <summary>
	///		Carga un archivo
	/// </summary>
	public MLFile Load(XmlReader xmlReader)
	{
		MLFile fileML = new();

			// Carga los datos del archivo
			while (xmlReader.Read())
				switch (xmlReader.NodeType)
				{
					case XmlNodeType.Element:
							fileML.Nodes.Add(LoadNode(xmlReader));
						break;
				}
			// Devuelve el archivo
			return fileML;
	}

	/// <summary>
	///		Carga los datos de un nodo
	/// </summary>
	private MLNode LoadNode(XmlReader xmlReader)
	{
		MLNode nodeML = new(xmlReader.Name);

			// Asigna los atributos
			nodeML.Attributes.AddRange(LoadAttributes(xmlReader));
			// Lee los nodos
			if (!xmlReader.IsEmptyElement)
			{
				bool isEnd = false;

					// Lee los datos del nodo
					while (!isEnd && xmlReader.Read())
						switch (xmlReader.NodeType)
						{
							case XmlNodeType.Element:
									nodeML.Nodes.Add(LoadNode(xmlReader));
								break;
							case XmlNodeType.Text:
							case XmlNodeType.CDATA:
									nodeML.Value = Decode(xmlReader.Value);
								break;
							case XmlNodeType.EndElement:
									isEnd = true;
								break;
						}
			}
			else
				nodeML.Value = xmlReader.Value;
			// Devuelve el nodo
			return nodeML;
	}

	/// <summary>
	///		Carga los atributos
	/// </summary>
	private MLAttributesCollection LoadAttributes(XmlReader xmlReader)
	{
		MLAttributesCollection attributes = new();

			// Obtiene los atributos
			if (xmlReader.AttributeCount > 0)
			{ 
				// Carga los atributos
				for (int index = 0; index < xmlReader.AttributeCount; index++)
				{ 
					// Pasa al atributo
					xmlReader.MoveToAttribute(index);
					// Asigna los valores del atributo
					if (xmlReader.NodeType == XmlNodeType.Attribute)
						attributes.Add(xmlReader.Name, xmlReader.Value);
				}
				// Pasa al primer atributo de nuevo
				xmlReader.MoveToElement();
			}
			// Devuelve la colección de atributos
			return attributes;
	}

	/// <summary>
	///		Decodifica una cadena HTML
	/// </summary>
	private string Decode(string value)
	{ 
		// Quita los caracteres raros
		if (!string.IsNullOrEmpty(value))
		{
			value = value.Replace("&amp;", "&");
			value = value.Replace("&lt;", "<");
			value = value.Replace("&gt;", ">");
			value = value.Replace("&quot;", "\"");
			value = value.Replace("&aacute;", "á");
			value = value.Replace("&eacute;", "é");
			value = value.Replace("&iacute;", "í");
			value = value.Replace("&oacute;", "ó");
			value = value.Replace("&uacute;", "ú");
		}
		// Devuelve la cadena
		return value;
	}

	/// <summary>
	///		Indica si se deben incluir los comentarios en los nodos
	/// </summary>
	public bool IncludeComments { get; set; }
}

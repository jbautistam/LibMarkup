using System;

namespace Bau.Libraries.LibMarkupLanguage
{
	/// <summary>
	///		Colección de <see cref="MLNode"/>
	/// </summary>
	public class MLNodesCollection : MLItemsBaseCollection<MLNode>
	{
		/// <summary>
		///		Añade una serie de nodos a un nodo
		/// </summary>
		public void Add(string name, MLNodesCollection nodesML)
		{
			MLNode nodeML = Add(name);

				// Añade los nodos
				nodeML.Nodes.AddRange(nodesML);
		}

		/// <summary>
		///		Añade un nodo CDATA normalizado
		/// </summary>
		public void AddCData(string name, string text)
		{
			MLNode nodeML = Add(name);
				
				nodeML.Value = ConvertToCData(text);
		}

		/// <summary>
		///		Normaliza un texto para un valor CData
		/// </summary>
		private string ConvertToCData(string text)
		{
			string result = string.Empty;

				// Normaliza el texto
				if (!string.IsNullOrWhiteSpace(text))
				{
					// Asigna el texto
					result = text;
					// Normaliza el texto
					result = result.Replace("&", "&amp;");
					result = result.Replace("<", "&lt;");
					result = result.Replace(">", "&gt;");
					// Asigna el CData
					result = @$"<![CDATA[
								{result}
								]]>
							";
				}
				// Devuelve los datos convertidos
				return result;
		}
	}
}

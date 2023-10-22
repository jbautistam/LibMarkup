namespace Bau.Libraries.LibMarkupLanguage.Builders
{
	/// <summary>
	///		Generador de nodos
	/// </summary>
	public class MLNodeBuilder
	{
		public MLNodeBuilder(string name, MLNodeBuilder parent = null)
		{
			Parent = parent;
		}

		/// <summary>
		///		Nodos
		/// </summary>
		public MLNodesCollection Nodes { get; } = new MLNodesCollection();

		/// <summary>
		///		Genera un nodo
		/// </summary>
		private MLNode LastNode => Nodes[0];

		/// <summary>
		///		Generador padre
		/// </summary>
		private MLNodeBuilder Parent { get; }
	}
}

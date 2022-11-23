using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AuditCAC.Domain.Dto
{

	[XmlRoot(ElementName = "archivo")]
	public class Archivo
	{

		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "pagina")]
	public class Pagina
	{

		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }

		[XmlText]
		public int Text { get; set; }
	}

	[XmlRoot(ElementName = "texto")]
	public class Texto
	{

		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "palabra_clave")]
	public class PalabraClave
	{

		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "fecha")]
	public class Fecha
	{

		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "parrafo")]
	public class Parrafo
	{

		[XmlElement(ElementName = "texto")]
		public Texto Texto { get; set; }

		[XmlElement(ElementName = "palabra_clave")]
		public PalabraClave PalabraClave { get; set; }

		[XmlElement(ElementName = "fecha")]
		public Fecha Fecha { get; set; }

		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "parrafos")]
	public class Parrafos
	{

		[XmlElement(ElementName = "parrafo")]
		public Parrafo Parrafo { get; set; }

		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "contexto")]
	public class Contexto
	{

		[XmlElement(ElementName = "pagina")]
		public Pagina Pagina { get; set; }

		[XmlElement(ElementName = "parrafos")]
		public Parrafos Parrafos { get; set; }

		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "contextos")]
	public class Contextos
	{

		[XmlElement(ElementName = "contexto")]
		public List<Contexto> Contexto { get; set; }

		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "documento")]
	public class Documento
	{

		[XmlElement(ElementName = "archivo")]
		public Archivo Archivo { get; set; }

		[XmlElement(ElementName = "contextos")]
		public Contextos Contextos { get; set; }

		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "documentos")]
	public class Documentos
	{

		[XmlElement(ElementName = "documento")]
		public List<Documento> Documento { get; set; }

		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "root")]
	public class ContextoDto
	{

		[XmlElement(ElementName = "documentos")]
		public Documentos Documentos { get; set; }
	}

}

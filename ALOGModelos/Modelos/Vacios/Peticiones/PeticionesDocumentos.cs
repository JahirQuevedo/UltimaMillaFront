using ALOG.Modelos.Modelos.Catalogos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALOG.Modelos.Modelos.Vacios.Peticiones
{
    public class PeticionesDocumentos
    {



        public int IdDocumento { get; set; }
        //[NotMapped]
        //public byte[] Documento { get; set; }

        public string DocumentoUUID { get; set; } = Guid.NewGuid().ToString();
        public string Ubicacion { get; set; }



        public string MimeType { get; set; }

        public string NombreDocumento { get; set; }

        [ForeignKey("CatDocumento")]
        public int IdTipoDocumento { get; set; }
        public CatDocumentos CatDocumento { get; set; }

        // Esta propiedad no se mapeará a la base de datos
        [NotMapped]
        public string TipoDocumentoNombre { get; set; }

        //[ForeignKey("peticionesServicios")]
        public int IdServicio { get; set; }

        //public PeticionesServicio peticionesServicios { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool Activo { get; set; }
    }
}

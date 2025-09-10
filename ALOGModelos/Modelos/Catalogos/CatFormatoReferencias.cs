using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatFormatoReferencias
    {
        //VACIOS
        //1

        //Por linea de negocio.
        //ORDEN
        //TIPO DATO
        //1,1, 1,AÑO
        //2,1, 2,ACRONIMO
        //3,1, 4,DIA
        //4,1, 3 ADUANA

        [Key]
        public int IdCatFormatoRef { get; set; }
        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; }
        [Required]
        [ForeignKey("CatLineaNegocio")]
        public int IdCatLineaNegocio { get; set; }
        public virtual CatLineaNegocio CatLineaNegocio { get; set; }

       
        [Required]
        [ForeignKey("catUsuario")]
        public int IdUsuarioRegistro { get; set; }
        public virtual CatUsuarios catUsuario { get; set; }

        public virtual ICollection<CatFormatoRefDet> CatFormatoRefDet { get; set; }



    }
}

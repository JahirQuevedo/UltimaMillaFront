using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace ALOG.Modelos.Modelos.Catalogos
{
    public class CatFormatoRefDet
    {

        //ORDEN
        //TIPO DATO
        //1,1, 1,LN
        //2,1, 2,ADUANA
        //3,1, 3,AÑO
        //4,1, 4 CONSECUTIVO - SEQ3PL

        //ORDEN
        //TIPO DATO
        //5,2, 1,LN
        //6,2, 2,ACRONIMO
        //7,2, 3,PROYECTO
        //8,2, 4 CONSECUTIVO - SEQ3PL



        //AÑO-VC-16-LZC-1
            //Buscar por ordeby OrdenCampo asc
            //AÑO - 2024
            //LINEA ACRONIMO VC
            //DIA - NOW.date.day
            //ADUANA EN FUNCION

        [Key]
        public int IdCatFormatoRefDet { get; set; }

        [Required]
        [ForeignKey("catFormatoReferencias")]
        public int IdCatFormatoRef { get; set; }
        public virtual CatFormatoReferencias catFormatoReferencias { get; set; }

        [Required]
        public int OrdenCampo { get; set; }
        [Required]
        [MaxLength(20)]
        public string TipoDato { get; set; } 
        [MaxLength(200)]
        public string Especificacion { get; set; }
        [MaxLength(100)]
        public string Valor { get; set; }
        [Required]
        public bool Activo { get; set; }=true;
        [Required]
        public DateTime FechaRegistro { get; set; }=DateTime.Now;


    }
}

using ALOG.Enums;

namespace ALOG.Modelos;

public class PartidaAlmacen : BaseEntity
{
        #region Propiedades

        public int IdTarja { get; set; }

        /// <summary>
        /// Numero de partida que tiene
        /// </summary>
        public int Partida { get; set; }

        public DateTime? FechaIngreso { get; set; }

        public DateTime? FechaSalida { get; set; }

        public int CantidadAveriados { get; set; }

        public decimal PesoAveriado { get; set; }

        public string DescripcionAveria { get; set; }

        public string BlHouse { get; set; }

        public string Marcas { get; set; }

        public string Numeros { get; set; }

        public string Modelo { get; set; }

        public int IdReferencia { get; set; }

        public EstadoInventario Estado { get; set; }

        public int CantidadInicial { get; set; }

        public decimal PesoInicial { get; set; }

        public string Mercancia { get; set; }

        public bool EsCongelado { get; set; }

        public bool EsRefrigerado { get; set; }

        public bool EsPerecedero { get; set; }

        public bool EsSusceptible { get; set; }

        public bool EnExistencia { get; set; }

        public int IdTipoEmbalaje { get; set; }

        public string DescripcionTipoEmbalaje { get; set; }

        public int IdUnidadMedida { get; set; }

        public string DescripcionUnidadMedida { get; set; }

        public decimal PesoNeto { get; set; }

        public string FolioRevalidado { get; set; }

        /// <summary>
        /// Cantidad Actual del inventario
        /// </summary>
        public int CantidadActual { get; set; }

        /// <summary>
        /// Peso actual del inventario
        /// </summary>
        public decimal PesoActual { get; set; }

        /// <summary>
        /// Operador que realizo ingreso o traslado de mercancia
        /// </summary>
        public string Operador { get; set; }

        /// <summary>
        /// Indica si la partida es editable en el modulo de tarja
        /// </summary>
        public bool EsPartidaEditable { get; set; }

        /// <summary>
        /// Agregado para ocupar un link de tarja destino en tarjas
        /// </summary>
        public int IdTarjaDestino { get; set; }

        /// <summary>
        /// Identifica el GRUPO al pertenece la partida
        /// </summary>
        public string Grupo { get; set; }

        #endregion
        
    }

namespace ALOG.Modelos.Modelos.Graficas {
    public class EChartOption {
        public Tooltip Tooltip { get; set; } = new Tooltip { Trigger = "item" };
        public Legend Legend { get; set; } = new Legend { Top = "5%", Left = "center" };
        public List<Series> Series { get; set; } = new List<Series>();
    }
}

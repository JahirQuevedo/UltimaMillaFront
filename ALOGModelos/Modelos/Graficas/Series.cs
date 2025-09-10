using System.Reflection.Emit;

namespace ALOG.Modelos.Modelos.Graficas {
    public class Series {
        public string Name { get; set; }
        public string Type { get; set; } = "pie";
        public List<string> Radius { get; set; } = new List<string> { "40%", "70%" };
        public bool AvoidLabelOverlap { get; set; } = true;
        public Label Label { get; set; } = new Label { Show = true, Position = "center" };
        public Emphasis Emphasis { get; set; } = new Emphasis { Label = new Label { Show = true, FontSize = 40, FontWeight = "normal" } };
        public LabelLine LabelLine { get; set; } = new LabelLine { Show = false };
        public List<DataPoint> Data { get; set; }
    }
}

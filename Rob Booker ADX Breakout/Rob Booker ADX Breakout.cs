using cAlgo.API;
using cAlgo.API.Indicators;
using System.Linq;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class RobBookerAdxBreakout : Indicator
    {
        private DirectionalMovementSystem _adx;

        [Parameter("ADX Periods", DefaultValue = 14)]
        public int AdxPeriods { get; set; }

        [Parameter("ADX Threshold", DefaultValue = 18)]
        public double AdxThreshold { get; set; }

        [Parameter("Box Periods", DefaultValue = 20)]
        public int BoxPeriods { get; set; }

        [Output("Box High", LineColor = "Green", PlotType = PlotType.DiscontinuousLine)]
        public IndicatorDataSeries BoxHigh { get; set; }

        [Output("Box Low", LineColor = "Red", PlotType = PlotType.DiscontinuousLine)]
        public IndicatorDataSeries BoxLow { get; set; }

        protected override void Initialize()
        {
            _adx = Indicators.DirectionalMovementSystem(AdxPeriods);
        }

        public override void Calculate(int index)
        {
            if (_adx.ADX[index] > AdxThreshold)
            {
                BoxHigh[index] = double.NaN;
                BoxLow[index] = double.NaN;

                return;
            }

            var boxBars = Bars.Skip(Bars.Count - BoxPeriods).ToArray();

            var boxHigh = boxBars.Max(bar => bar.High);
            var boxLow = boxBars.Min(bar => bar.Low);

            for (int barIndex = index; barIndex > index - BoxPeriods; barIndex--)
            {
                BoxHigh[barIndex] = boxHigh;
                BoxLow[barIndex] = boxLow;
            }
        }
    }
}
using System;
using DevExpress.Mvvm.UI;
using LeagueRecorder.Abstractions.Data;

namespace LeagueRecorder.Windows.Converters
{
    public class RegionToStringConverter : ObjectToObjectConverter
    {
        public RegionToStringConverter()
        {
            foreach (Region enumValue in Enum.GetValues(typeof (Region)))
            {
                this.Map.Add(new MapItem(enumValue, enumValue.GetReadableString()));
            }
        }
    }
}
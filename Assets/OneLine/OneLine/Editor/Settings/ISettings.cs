
namespace OneLine.Settings {
    public interface ISettings {
        TernaryBoolean Enabled { get; }
        TernaryBoolean DrawVerticalSeparator { get; }
        TernaryBoolean DrawHorizontalSeparator { get; }
    }
}
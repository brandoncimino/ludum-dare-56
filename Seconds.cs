using System;
using Godot;

namespace ludumdare56;

/// <summary>
/// A quantity of the fixed numerical value of the caesium frequency, ΔνCs, the unperturbed ground-state hyperfine transition frequency of the caesium 133 atom, to be 9192631770 when expressed in the unit Hz, which is equal to s−¹.
/// </summary>
/// <remarks>
/// This class serves as a bridge between <see cref="double"/>-based time measurements like <see cref="Node._Process"/>
/// and <see cref="long"/>-based time measurements like <see cref="TimeSpan"/> <i>(which is stored as <see cref="TimeSpan.Ticks"/>)</i>.
/// </remarks>
/// <param name="TotalSeconds">The amount of seconds.</param>
public readonly record struct Seconds(double TotalSeconds)
{
    #region Casts

    public static implicit operator TimeSpan(Seconds seconds) => TimeSpan.FromSeconds(seconds.TotalSeconds);
    public static implicit operator Seconds(TimeSpan timeSpan) => new(timeSpan.TotalSeconds);

    #endregion

    #region Addition / Subtraction

    public static Seconds operator +(Seconds a, Seconds b) => new(a.TotalSeconds + b.TotalSeconds);
    public static Seconds operator -(Seconds a, Seconds b) => new(a.TotalSeconds - b.TotalSeconds);

    #endregion

    #region Multiplication / Division

    public static Seconds operator /(Seconds a, Seconds b) => new(a.TotalSeconds / b.TotalSeconds);
    public static Seconds operator /(Seconds seconds, double divisor) => new(seconds.TotalSeconds / divisor);
    public static Seconds operator *(Seconds seconds, double factor) => new(seconds.TotalSeconds * factor);
    public static Seconds operator *(double factor, Seconds seconds) => seconds * factor;
    public static Seconds operator %(Seconds a, Seconds b) => new(a.TotalSeconds % b.TotalSeconds);

    #endregion

    #region Unary

    public static Seconds operator +(Seconds seconds) => seconds;
    public static Seconds operator -(Seconds seconds) => new(-seconds.TotalSeconds);

    #endregion

    #region Comparison

    public static bool operator >(Seconds a, Seconds b) => a.TotalSeconds > b.TotalSeconds;
    public static bool operator <(Seconds a, Seconds b) => a.TotalSeconds < b.TotalSeconds;
    public static bool operator >=(Seconds a, Seconds b) => a.TotalSeconds >= b.TotalSeconds;
    public static bool operator <=(Seconds a, Seconds b) => a.TotalSeconds <= b.TotalSeconds;

    #endregion

    /// <remarks>
    /// The International System of Units prescribes a space between the number and unit of measurement <i>(<a href="https://en.wikipedia.org/wiki/Space_(punctuation)#Unit_symbols_and_numbers">Wikipedia</a>)</i>.
    /// </remarks>
    public override string ToString() => $"{TotalSeconds:N2} s";
}

public static class SecondsExtensions
{
    public static Seconds Seconds(this double totalSeconds) => new(totalSeconds);
    public static Seconds Seconds(this float totalSeconds) => new(totalSeconds);
}
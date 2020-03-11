using Implementation;
using System;

public readonly struct GenericValueLanguage
{
    public readonly static GenericValueLanguage VOID = new GenericValueLanguage(new object());

    public readonly object Value;

    public GenericValueLanguage(object value) => Value = value;

    public static GenericValueLanguage operator +(GenericValueLanguage left, GenericValueLanguage right) =>
        new GenericValueLanguage(left.AsDecimal() + right.AsDecimal());

    public static GenericValueLanguage operator -(GenericValueLanguage left, GenericValueLanguage right) =>
        new GenericValueLanguage(left.AsDecimal() - right.AsDecimal());

    public static GenericValueLanguage operator *(GenericValueLanguage left, GenericValueLanguage right) =>
        new GenericValueLanguage(left.AsDecimal() * right.AsDecimal());

    public static GenericValueLanguage operator /(GenericValueLanguage left, GenericValueLanguage right) =>
        new GenericValueLanguage(left.AsDecimal() / right.AsDecimal());

    public static explicit operator decimal(GenericValueLanguage genericValue) =>
        decimal.Parse(genericValue.Value?.ToString() ?? "0.0");

    public static explicit operator int(GenericValueLanguage genericValue) =>
        int.Parse(genericValue.Value.ToString());

    public bool AsBoolean() => bool.Parse(Value.ToString());

    public decimal AsDecimal() => decimal.Parse(Value.ToString());

    public int AsInt() => int.Parse(Value.ToString());

    public DateTime AsDateTime() => DateTime.Parse(Value.ToString());

    public string AsString() => Value.ToString();

    public bool IsDecimal() => Value is decimal;

    public bool IsInt() => Value is int;

    public bool IsDate() => Value is DateTime;

    public override int GetHashCode() =>
        Value == null ? 0 : Value.GetHashCode();

    public override bool Equals(object o)
    {
        if (Value == o)
        {
            return true;
        }

        if (Value is null || o is null)
        {
            return false;
        }

        var that = (GenericValueLanguage)o;

        return Value.Equals(that.Value);
    }

    public override string ToString() =>
        IsDecimal() ? Value.ToString().Replace(',', '.') : Value?.ToString();
}


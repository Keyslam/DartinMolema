using App.Models;

namespace App.View.MatchInput;

public class DartInput
{
    private string input;
    public string Input
    {
        get => input;
        set
        {
            this.input = value;

            this.IsValid = this.ParseInput(this.input, out int newValue, out ThrowKind throwKind);
            this.Value = newValue;
            this.ThrowKind = throwKind;
        }
    }


    public bool HasInput => input != string.Empty;
    public bool IsValid { get; private set; }
    public int Value { get; private set; }
    public ThrowKind ThrowKind { get; private set; }

    public DartInput()
    {
        this.IsValid = false;
        this.Value = 0;
        this.ThrowKind = ThrowKind.None;

        this.input = string.Empty;
    }

    private bool ParseInput(string input, out int value, out ThrowKind throwKind)
    {
        var prefix = input.Substring(0, Math.Min(1, input.Length));

        if (!this.MapPrefixToThrowKind(prefix, out throwKind))
        {
            value = 0;
            return false;
        }

        if (!this.ParseThrownValue(input, throwKind, out int thrownValue))
        {
            value = 0;
            return false;
        }

        value = thrownValue;
        return true;
    }

    private bool ParseThrownValue(string input, ThrowKind throwKind, out int value)
    {
        switch (throwKind)
        {
            case ThrowKind.Double:
            case ThrowKind.Triple:
                return this.ParseDoubleOrTriple(input, out value);
            case ThrowKind.Single:
                return this.ParseSingle(input, out value);
            case ThrowKind.InnerBull:
            case ThrowKind.OuterBull:
                value = 25;
                return true;
            default:
                value = 0;
                return true;
        }
    }

    private bool ParseDoubleOrTriple(string input, out int value)
    {
        var stringValue = input.Substring(Math.Min(1, input.Length), Math.Min(2, input.Length - 1));
        if (!int.TryParse(stringValue, out int parsedValue))
        {
            value = 0;
            return false;
        }

        if (parsedValue < 1 || parsedValue > 20)
        {
            value = 0;
            return false;
        }

        value = parsedValue;
        return true;
    }

    private bool ParseSingle(string input, out int value)
    {
        var prefix = input.Substring(0, Math.Min(1, input.Length));
        string stringValue = string.Empty;

        if (int.TryParse(prefix, out _))
            stringValue = input;
        else
            stringValue = input.Substring(Math.Min(1, input.Length), Math.Min(2, input.Length - 1));

        if (!int.TryParse(stringValue, out int parsedValue))
        {
            value = 0;
            return false;
        }

        if (parsedValue < 1 || parsedValue > 20)
        {
            value = 0;
            return false;
        }

        value = parsedValue;
        return true;
    }

    private bool MapPrefixToThrowKind(string prefix, out ThrowKind throwKind)
    {
        switch (prefix.ToUpper())
        {
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
            case "7":
            case "8":
            case "9":
            case "S":
                throwKind = ThrowKind.Single;
                return true;
            case "D":
                throwKind = ThrowKind.Double;
                return true;
            case "T":
                throwKind = ThrowKind.Triple;
                return true;
            case "O":
                throwKind = ThrowKind.OuterBull;
                return true;
            case "I":
                throwKind = ThrowKind.InnerBull;
                return true;
            case "0":
            case "F":
                throwKind = ThrowKind.Foul;
                return true;
            default:
                throwKind = ThrowKind.None;

                return false;
        }
    }
}
using System.Text.RegularExpressions;

namespace WhoAmI.Domain.Shared.Base;

public abstract class TextValueObject : ValueObject {
    protected static string NormalizeText(string str) =>
        Regex.Replace(str.Trim(), @"\s+", " ");
}

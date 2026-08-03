namespace WhoAmI.Application.Abstractions.Serialization;

internal interface IObjectSerializer {
    string Serialize(object value);
}

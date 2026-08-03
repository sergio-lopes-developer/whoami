namespace WhoAmI.Application.Abstractions.Decorators;

internal interface IHandlerDecorator<out T> {
    T Inner { get; }
}

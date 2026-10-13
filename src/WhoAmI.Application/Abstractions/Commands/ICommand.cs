namespace WhoAmI.Application.Abstractions.Commands;

public interface ICommand;

public interface ICommand<TResult> : ICommand;

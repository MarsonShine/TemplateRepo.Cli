namespace Fz.Tool.Cli.Abstract
{
	public interface ICommandHandler
	{
		ValueTask ExecuteAsync(string repoUrl, string projectName);
	}
}

namespace Fz.Tool.Cli.Abstract
{
	public interface INamespaceReplacer
	{
		ValueTask ReplaceNamespaceAsync(string originProjectName, string projectName);
	}
}

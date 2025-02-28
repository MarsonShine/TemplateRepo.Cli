namespace Fz.Tool.Cli.Abstract
{
	public interface ITemplateCloner
	{
		ValueTask CloneTemplateAsync(string repoUrl, string projectName);
	}
}

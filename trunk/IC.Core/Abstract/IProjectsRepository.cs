using IC.Core.Entities.UI;

namespace IC.Core.Abstract
{
	public interface IProjectsRepository
	{
		Project Load(string name);
		Project Create(string name);
		void Update(Project project);
	}
}

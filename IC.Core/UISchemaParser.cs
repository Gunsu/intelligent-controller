using System;
using System.Linq;
using System.Xml.Linq;
using IC.Core.Abstract;
using IC.Core.Entities;

namespace IC.Core
{
	public class UISchemaParser
	{
		private readonly IQueryable<BlockType> _blockTypes;

		public UISchemaParser(IBlockTypesRepository blockTypesRepository)
		{
			_blockTypes = blockTypesRepository.LoadBlockTypesFromFile().AsQueryable();
		}

		public Entities.Project Parse(Entities.UI.Project uiProject)
		{
			var compilationProject = new Entities.Project();
			foreach(var uiSchema in uiProject.Schemas)
			{
				var compilationSchema = new Entities.Schema() {Name = uiSchema.Name, Project = compilationProject};
				compilationProject.Schemas.Add(compilationSchema);

				var designerItems = uiSchema.CurrentUISchema.Element("DesignerItems");
				foreach (XElement designerItem in designerItems.Nodes().OfType<XElement>())
				{
					var content = XElement.Parse(designerItem.Element("Content").Value);
					int blockTypeID = GetBlockTypeID(content);
					BlockType blockType = (from b in _blockTypes
									       where b.ID == blockTypeID
									       select b).First();
					var block = new Block(blockType);
					compilationSchema.Blocks.Add(block);
				}

				foreach (XElement element in uiSchema.CurrentUISchema.Elements())
				{
				}

			}
			throw new NotImplementedException();
		}

		private int GetBlockTypeID(XElement content)
		{
			foreach (XElement element in content.Nodes().OfType<XElement>())
			{
				if (element.Name.ToString() == "Tag" || element.Name.LocalName == "Image.Tag")
				{
					return int.Parse(element.Value);
				}
			}
			throw new InvalidOperationException("Не найден необходимый элемент Tag с идентификатором блока.");
		}
	}
}
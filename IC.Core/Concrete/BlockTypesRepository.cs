using System.Collections.Generic;
using System.Xml;
using IC.Core.Abstract;
using IC.Core.Entities;
using ValidationAspects;

namespace IC.Core.Concrete
{
	public class BlockTypesRepository : IBlockTypesRepository
	{
		private string _blockTypesFilePath;

		public List<BlockType> LoadBlockTypesFromFile()
		{
			var document = new XmlDocument();
			document.Load(_blockTypesFilePath);
			XmlNodeList nodeList = document.GetElementsByTagName("BlockType");
			List<BlockType> blockTypes = new List<BlockType>();
			foreach (XmlNode node in nodeList)
			{
				string blockTypeName = node.ChildNodes[0].InnerText;
				int id = int.Parse(node.ChildNodes[1].InnerText);
				string description = node.ChildNodes[2].InnerText;
				BlockType blockType = new BlockType(id, blockTypeName, description);
				for (int i = 3; i < node.ChildNodes.Count; ++i)
				{
					string name = node.ChildNodes[i].ChildNodes[0].InnerText;
					int size = int.Parse(node.ChildNodes[i].ChildNodes[1].InnerText);
					BlockConnectionPoint blockConnectionPoint = new BlockConnectionPoint(name, size);
					if (node.ChildNodes[i].Name == "Input")
					{
						blockType.InputPoints.Add(blockConnectionPoint);
					}
					else if (node.ChildNodes[i].Name == "Output")
					{
						blockType.OutputPoints.Add(blockConnectionPoint);
					}
				}
				blockTypes.Add(blockType);
			}

			return blockTypes;
		}

		public BlockTypesRepository([NotNullOrEmpty] string blockTypesFilePath)
		{
			_blockTypesFilePath = blockTypesFilePath;
		}
	}
}

using System.Collections.Generic;

using IC.CoreInterfaces.Processes;
using IC.CoreInterfaces.Objects;
using ValidationAspects;
using System.Xml;
using IC.Core.Objects;

namespace IC.Core.Processes
{
	public sealed class BlockTypesProcesses : IBlockTypesProcesses
	{
		private string _blockTypesFilePath;

		#region IBlockTypesProcesses Members

		public IList<IBlockType> LoadBlockTypesFromFile()
		{
			var document = new XmlDocument();
			document.Load(_blockTypesFilePath);
			XmlNodeList nodeList = document.GetElementsByTagName("BlockType");
			IList<IBlockType> blockTypes = new List<IBlockType>();
			foreach (XmlNode node in nodeList)
			{
				string blockTypeName = node.ChildNodes[0].InnerText;
				int id = int.Parse(node.ChildNodes[1].InnerText);
				IBlockType blockType = new BlockType(id, blockTypeName);
				for (int i = 2; i < node.ChildNodes.Count; ++i)
				{
					string name = node.ChildNodes[i].ChildNodes[0].InnerText;
					int size = int.Parse(node.ChildNodes[i].ChildNodes[1].InnerText);
					IBlockConnectionPoint blockConnectionPoint = new BlockConnectionPoint(name, size);
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

		#endregion

		public BlockTypesProcesses([NotNullOrEmpty] string blockTypesFilePath)
		{
			_blockTypesFilePath = blockTypesFilePath;
		}
	}
}

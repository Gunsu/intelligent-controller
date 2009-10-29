namespace IC.Core.Entities
{
	public class OutputCommandBlock : Block
	{
		public string Mask { get; set; }

		public OutputCommandBlock() : base()
		{
			this.ObjectType = IC.Core.Enums.ObjectType.OutputCommandBlock;
		}
	}
}

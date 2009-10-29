namespace IC.Core.Entities
{
	public class InputCommandBlock : Block
	{
		public string Mask { get; set; }

		public InputCommandBlock() : base()
		{
			this.ObjectType = IC.Core.Enums.ObjectType.InputCommandBlock;
		}
	}
}

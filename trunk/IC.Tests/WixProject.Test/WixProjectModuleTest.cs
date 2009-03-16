using IC.UI.WixProject;
using IC.UI.WixProject.Mock;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IC.Tests.WixProjectTest
{
    /// <summary>
    ///This is a test class for WixProjectModuleTest and is intended
    ///to contain all WixProjectModuleTest Unit Tests
    ///</summary>
	[TestClass]
	public class WixProjectModuleTest
	{
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		///A test for Initialize
		///</summary>
		[TestMethod]
		public void InitializeTest()
		{
			var regionManager = new MockRegionManager();
			var target = new WixProjectModule(regionManager);
			var menuRegion = new MockRegion();
			regionManager.Regions.Add("MenuRegion", menuRegion);
			target.Initialize();
			Assert.AreEqual(1, menuRegion.AddedViews.Count);
		}
	}
}

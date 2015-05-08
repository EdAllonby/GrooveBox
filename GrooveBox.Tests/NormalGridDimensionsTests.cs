using NUnit.Framework;

namespace GrooveBox.Tests
{
    [TestFixture]
    public class NormalGridDimensionsTests
    {
        private static void AssertGridWithDimensions(IGridDimensions gridDimensions, int expectedRows,
            int expectedColumns)
        {
            Assert.AreEqual(expectedRows, gridDimensions.Rows);
            Assert.AreEqual(expectedColumns, gridDimensions.Columns);
        }

        [Test]
        public void ShouldCreate1Row1ColumnsFor1Bars()
        {
            AssertGridWithDimensions(new NormalGridDimensions(1, 4), 1, 1);
        }

        [Test]
        public void ShouldCreate1Row2ColumnsFor2Bars()
        {
            AssertGridWithDimensions(new NormalGridDimensions(2, 4), 1, 2);
        }

        [Test]
        public void ShouldCreate1Row3ColumnsFor3Bars()
        {
            AssertGridWithDimensions(new NormalGridDimensions(3, 4), 1, 3);
        }

        [Test]
        public void ShouldCreate1Row4ColumnsFor4Bars()
        {
            AssertGridWithDimensions(new NormalGridDimensions(4, 4), 1, 4);
        }

        [Test]
        public void ShouldCreate2Rows4ColumnsFor5Bars()
        {
            AssertGridWithDimensions(new NormalGridDimensions(5, 4), 2, 4);
        }

        [Test]
        public void ShouldCreate2Rows4ColumnsFor6Bars()
        {
            AssertGridWithDimensions(new NormalGridDimensions(6, 4), 2, 4);
        }

        [Test]
        public void ShouldCreate2Rows4ColumnsFor7Bars()
        {
            AssertGridWithDimensions(new NormalGridDimensions(7, 4), 2, 4);
        }

        [Test]
        public void ShouldCreate2Rows4ColumnsFor8Bars()
        {
            AssertGridWithDimensions(new NormalGridDimensions(8, 4), 2, 4);
        }

        [Test]
        public void ShouldCreate3Rows4ColumnsFor10Bars()
        {
            AssertGridWithDimensions(new NormalGridDimensions(10, 4), 3, 4);
        }

        [Test]
        public void ShouldCreate3Rows4ColumnsFor11Bars()
        {
            AssertGridWithDimensions(new NormalGridDimensions(11, 4), 3, 4);
        }

        [Test]
        public void ShouldCreate3Rows4ColumnsFor12Bars()
        {
            AssertGridWithDimensions(new NormalGridDimensions(12, 4), 3, 4);
        }

        [Test]
        public void ShouldCreate3Rows4ColumnsFor9Bars()
        {
            AssertGridWithDimensions(new NormalGridDimensions(9, 4), 3, 4);
        }

        [Test]
        public void ShouldCreate4Rows4ColumnsFor13Bars()
        {
            AssertGridWithDimensions(new NormalGridDimensions(13, 4), 4, 4);
        }

        [Test]
        public void ShouldCreate4Rows4ColumnsFor14Bars()
        {
            AssertGridWithDimensions(new NormalGridDimensions(14, 4), 4, 4);
        }

        [Test]
        public void ShouldCreate4Rows4ColumnsFor15Bars()
        {
            AssertGridWithDimensions(new NormalGridDimensions(15, 4), 4, 4);
        }

        [Test]
        public void ShouldCreate4Rows4ColumnsFor16Bars()
        {
            AssertGridWithDimensions(new NormalGridDimensions(16, 4), 4, 4);
        }
    }
}
using NUnit.Framework;

namespace CheckSumTask.Test;

public class Tests
{
    public class CheckSumTests
    {
        [Test]
        public void ComputeSingleThreadTest()
        {
            string path = "C:/Users/Acer/source/repos/spbu-3sem-tasks/MD5";
            var checkSum1 = CheckSum.CalculateSingleThread("C:/Users/Acer/source/repos/spbu-3sem-tasks/MD5");
            var checkSum2 = CheckSum.CalculateSingleThread("C:/Users/Acer/source/reposspbu-3sem-tasks/MD5");
            Assert.AreEqual(checkSum1, checkSum2);
        }

        [Test]
        public void ComputeMultiThreadTest()
        {
            string path = "C:/Users/Acer/source/repos/spbu-3sem-tasks/MD5";
            var checkSum1 = CheckSum.CalculateMultiThread(path);
            var checkSum2 = CheckSum.CalculateMultiThread(path);
            Assert.AreEqual(checkSum1, checkSum2);
        }

        [Test]
        public void SingleAndMultiThreadTest()
        {
            string path = "C:/Users/Acer/source/repos/spbu-3sem-tasks/MD5";
            var checkSum1 = CheckSum.CalculateSingleThread(path);
            var checkSum2 = CheckSum.CalculateMultiThread(path);
            Assert.AreEqual(checkSum1, checkSum2);
        }
    }
}

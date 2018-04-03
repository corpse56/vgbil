using Circulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace scannedtest
{
    
    
    /// <summary>
    ///Это класс теста для Form1Test, в котором должны
    ///находиться все модульные тесты Form1Test
    ///</summary>
    [TestClass()]
    public class Form1Test
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Получает или устанавливает контекст теста, в котором предоставляются
        ///сведения о текущем тестовом запуске и обеспечивается его функциональность.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Дополнительные атрибуты теста
        // 
        //При написании тестов можно использовать следующие дополнительные атрибуты:
        //
        //ClassInitialize используется для выполнения кода до запуска первого теста в классе
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //ClassCleanup используется для выполнения кода после завершения работы всех тестов в классе
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //TestInitialize используется для выполнения кода перед запуском каждого теста
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //TestCleanup используется для выполнения кода после завершения каждого теста
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///Тест для Form1_Scanned
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CirculationOF.exe")]
        public void Form1_ScannedTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: инициализация подходящего значения
            object sender = null; // TODO: инициализация подходящего значения
            EventArgs ev = null; // TODO: инициализация подходящего значения
            target.Form1_Scanned(sender, ev);
            Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }
    }
}

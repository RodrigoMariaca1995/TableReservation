using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;

namespace TableReservationUnitTests
{
    [TestClass]
    public class ReservationsControllerTests
    {
        public int TablesReserved(ref List<int> tables, int partySize)
        {
            int capacity = 0;
            while (partySize > 0)
            {
                if (partySize > 4)
                {
                    if (tables.Exists(x => x == 6))
                    {
                        int index = tables.FindIndex(x => x == 6);
                        tables.RemoveAt(index);
                        capacity += 6;
                        partySize -= 6;
                    }
                    else if (tables.Exists(x => x == 4))
                    {
                        int index = tables.FindIndex(x => x == 4);
                        tables.RemoveAt(index);
                        capacity += 4;
                        partySize -= 4;
                    }
                    else
                    {
                        int index = tables.FindIndex(x => x == 2);
                        tables.RemoveAt(index);
                        capacity += 2;
                        partySize -= 2;
                    }
                }
                else if (partySize > 2)
                {
                    if (tables.Exists(x => x == 4))
                    {
                        int index = tables.FindIndex(x => x == 4);
                        tables.RemoveAt(index);
                        capacity += 4;
                        partySize -= 4;
                    }
                    else if (tables.Exists(x => x == 2))
                    {
                        int index = tables.FindIndex(x => x == 2);
                        tables.RemoveAt(index);
                        capacity += 2;
                        partySize -= 2;
                    }
                    else
                    {
                        int index = tables.FindIndex(x => x == 6);
                        tables.RemoveAt(index);
                        capacity += 6;
                        partySize -= 6;
                    }
                }
                else
                {
                    if (tables.Exists(x => x == 2))
                    {
                        int index = tables.FindIndex(x => x == 2);
                        tables.RemoveAt(index);
                        capacity += 2;
                        partySize -= 2;
                    }
                    else if (tables.Exists(x => x == 4))
                    {
                        int index = tables.FindIndex(x => x == 4);
                        tables.RemoveAt(index);
                        capacity += 4;
                        partySize -= 4;
                    }
                    else
                    {
                        int index = tables.FindIndex(x => x == 6);
                        tables.RemoveAt(index);
                        capacity += 6;
                        partySize -= 6;
                    }
                }
            }
            return capacity;
        }

        [TestMethod]
        public void TablesReservedTests_PartySize7_Remove6and2()
        {
            // Arrange
            List<int> tables = new List<int>() { 2, 2, 2, 4, 4, 4, 6, 6, 6 };

            // Act
            TablesReserved(ref tables, 7);
            
            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 2, 2, 4, 4, 4, 6, 6 });
        }

        [TestMethod]
        public void TablesReservedTests_PartySize9_Remove6and4()
        {
            // Arrange
            List<int> tables = new List<int>() { 2, 2, 2, 4, 4, 4, 6, 6, 6 };

            // Act
            TablesReserved(ref tables, 9);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 2, 2, 2, 4, 4, 6, 6 });
        }

        [TestMethod]
        public void TablesReservedTests_PartySize11_Remove6and6()
        {
            // Arrange
            List<int> tables = new List<int>() { 2, 2, 2, 4, 4, 4, 6, 6, 6 };

            // Act
            TablesReserved(ref tables, 11);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 2, 2, 2, 4, 4, 4, 6 });
        }

        [TestMethod]
        public void TablesReservedTests_PartySize17_Remove6and6and6()
        {
            // Arrange
            List<int> tables = new List<int>() { 2, 2, 2, 4, 4, 4, 6, 6, 6 };

            // Act
            TablesReserved(ref tables, 17);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 2, 2, 2, 4, 4, 4 });
        }
        [TestMethod]
        public void TablesReservedTests_PartySize15_Remove6and6and4()
        {
            // Arrange
            List<int> tables = new List<int>() { 2, 2, 2, 4, 4, 4, 6, 6, 6 };

            // Act
            TablesReserved(ref tables, 15);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 2, 2, 2, 4, 4, 6 });
        }
        [TestMethod]
        public void TablesReservedTests_PartySize13_Remove6and6and2()
        {
            // Arrange
            List<int> tables = new List<int>() { 2, 2, 2, 4, 4, 4, 6, 6, 6 };

            // Act
            TablesReserved(ref tables, 13);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 2, 2, 4, 4, 4, 6 });
        }
        [TestMethod]
        public void TablesReservedTests_PartySize13_Remove6and4and4()
        {
            // Arrange
            List<int> tables = new List<int>() { 2, 2, 2, 4, 4, 4, 6 };

            // Act
            TablesReserved(ref tables, 13);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 2, 2, 2, 4 });
        }
        [TestMethod]
        public void TablesReservedTests_PartySize11_Remove6and4and2()
        {
            // Arrange
            List<int> tables = new List<int>() { 2, 2, 2, 4, 4, 4, 6 };

            // Act
            TablesReserved(ref tables, 11);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 2, 2, 4, 4 });
        }
        [TestMethod]
        public void TablesReservedTests_PartySize9_Remove6and2and2()
        {
            // Arrange
            List<int> tables = new List<int>() { 2, 2, 2, 6, 6, 6 };

            // Act
            TablesReserved(ref tables, 9);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 2, 6, 6 });
        }
        [TestMethod]
        public void TablesReservedTests_PartySize7_Remove4and4()
        {
            // Arrange
            List<int> tables = new List<int>() { 2, 2, 2, 4, 4, 4 };

            // Act
            TablesReserved(ref tables, 7);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 2, 2, 2, 4});
        }
        [TestMethod]
        public void TablesReservedTests_PartySize5_Remove4and2()
        {
            // Arrange
            List<int> tables = new List<int>() { 2, 2, 2, 4, 4, 4 };

            // Act
            TablesReserved(ref tables, 5);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 2, 2, 4, 4 });
        }
        [TestMethod]
        public void TablesReservedTests_PartySize11_Remove4and4and4()
        {
            // Arrange
            List<int> tables = new List<int>() { 2, 2, 2, 4, 4, 4};

            // Act
            TablesReserved(ref tables, 11);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 2, 2, 2 });
        }
        [TestMethod]
        public void TablesReservedTests_PartySize9_Remove4and4and2()
        {
            // Arrange
            List<int> tables = new List<int>() { 2, 2, 2, 4, 4, 4 };

            // Act
            TablesReserved(ref tables, 9);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 2, 2, 4 });
        }
        [TestMethod]
        public void TablesReservedTests_PartySize7_Remove4and2and2()
        {
            // Arrange
            List<int> tables = new List<int>() { 2, 2, 2, 4 };

            // Act
            TablesReserved(ref tables, 7);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 2 });
        }
        [TestMethod]
        public void TablesReservedTests_PartySize9_Remove4and2and2and2()
        {
            // Arrange
            List<int> tables = new List<int>() { 2, 2, 2, 4 };

            // Act
            TablesReserved(ref tables, 9);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { });
        }
        [TestMethod]
        public void TablesReservedTests_PartySize36_RemoveAll()
        {
            // Arrange
            List<int> tables = new List<int>() { 2, 2, 2, 4, 4, 4, 6, 6, 6 };

            // Act
            TablesReserved(ref tables, 36);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { });
        }
        [TestMethod]
        public void TablesReservedTests_PartySize7_bRemove6and2()
        {
            // Arrange
            List<int> tables = new List<int>() { 2, 2, 2, 6, 6, 6 };

            // Act
            TablesReserved(ref tables, 7);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 2, 2, 6, 6 });
        }
        [TestMethod]
        public void TablesReservedTests_PartySize15_bRemove6and6and2and2()
        {
            // Arrange
            List<int> tables = new List<int>() { 2, 2, 2, 6, 6, 6 };

            // Act
            TablesReserved(ref tables, 15);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 2, 6 });
        }
        [TestMethod]
        public void TablesReservedTests_PartySize13_bRemove6and6and2()
        {
            // Arrange
            List<int> tables = new List<int>() { 2, 2, 2, 6, 6, 6 };

            // Act
            TablesReserved(ref tables, 13);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 2, 2, 6 });
        }
        [TestMethod]
        public void TablesReservedTests_PartySize13_cRemove6and6and4()
        {
            // Arrange
            List<int> tables = new List<int>() { 4, 4, 4, 6, 6, 6 };

            // Act
            TablesReserved(ref tables, 13);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 4, 4, 6 });
        }
        [TestMethod]
        public void TablesReservedTests_PartySize9_cRemove6and4()
        {
            // Arrange
            List<int> tables = new List<int>() { 4, 4, 4, 6, 6, 6 };

            // Act
            TablesReserved(ref tables, 9);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 4, 4, 6, 6 });
        }
        [TestMethod]
        public void TablesReservedTests_PartySize3_cRemove4()
        {
            // Arrange
            List<int> tables = new List<int>() { 4, 4, 4, 6, 6, 6 };

            // Act
            TablesReserved(ref tables, 3);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 4, 4, 6, 6, 6 });
        }
        [TestMethod]
        public void TablesReservedTests_PartySize7_dRemove6and2()
        {
            // Arrange
            List<int> tables = new List<int>() { 2, 4, 6 };

            // Act
            TablesReserved(ref tables, 7);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 4 });
        }
        [TestMethod]
        public void TablesReservedTests_PartySize5_dRemove4and2()
        {
            // Arrange
            List<int> tables = new List<int>() { 2, 4, 6 };

            // Act
            TablesReserved(ref tables, 5);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 2, 4 });
        }
        [TestMethod]
        public void TablesReservedTests_PartySize3_dRemove4()
        {
            // Arrange
            List<int> tables = new List<int>() { 2, 4, 6 };

            // Act
            TablesReserved(ref tables, 3);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 2, 6 });
        }
        [TestMethod]
        public void TablesReservedTests_PartySize3_eRemove4()
        {
            // Arrange
            List<int> tables = new List<int>() { 4, 6 };

            // Act
            TablesReserved(ref tables, 3);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 6 });
        }
        [TestMethod]
        public void TablesReservedTests_PartySize7_eRemove6and4()
        {
            // Arrange
            List<int> tables = new List<int>() { 4, 6 };

            // Act
            TablesReserved(ref tables, 7);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { });
        }
        [TestMethod]
        public void TablesReservedTests_PartySize5_eRemove6()
        {
            // Arrange
            List<int> tables = new List<int>() { 4, 6 };

            // Act
            TablesReserved(ref tables, 5);

            // Assert
            CollectionAssert.AreEqual(tables, new List<int>() { 4 });
        }
    }
}

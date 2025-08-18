using GymMgmt.Domain.Common;

namespace GymMgmt.Domain.Tests.Generics
{
    public class GenericEntityTest
    {
        private sealed class TestId { }

        private sealed class TestEntity : Entity<TestId>
        {
            public TestEntity(TestId id) => Id = id;
        }

        [Fact]
        public void Equals_ReturnsTrue_WhenSameReference()
        {
            var id = new TestId();
            var e1 = new TestEntity(id);
            Assert.True(e1.Equals(e1));
        }

        [Fact]
        public void Equals_ReturnsTrue_WhenSameIdAndType()
        {
            var id = new TestId();
            var e1 = new TestEntity(id);
            var e2 = new TestEntity(id);
            Assert.True(e1.Equals(e2));
        }

        [Fact]
        public void Equals_ReturnsFalse_WhenDifferentType()
        {
            var id = new TestId();
            var e1 = new TestEntity(id);
            var other = new AnotherEntity(id);
            Assert.False(e1.Equals(other));
        }

        [Fact]
        public void Equals_ReturnsFalse_WhenNull()
        {
            var e1 = new TestEntity(new TestId());
            Assert.False(e1.Equals(null));
        }

        [Fact]
        public void EqualityOperators_WorkAsExpected()
        {
            var id = new TestId();
            var e1 = new TestEntity(id);
            var e2 = new TestEntity(id);
            Assert.True(e1 == e2);
            Assert.False(e1 != e2);
            Assert.False(e1 == null);
            Assert.True(e1 != null);
        }

        [Fact]
        public void GetHashCode_ReturnsSameValue_ForSameId()
        {
            var id = new TestId();
            var e1 = new TestEntity(id);
            var e2 = new TestEntity(id);
            Assert.Equal(e1.GetHashCode(), e2.GetHashCode());
        }

        private sealed class AnotherEntity : Entity<TestId>
        {
            public AnotherEntity(TestId id) => Id = id;
        }
    }
}

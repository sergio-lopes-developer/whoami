using FluentAssertions;
using WhoAmI.Domain.Shared.Exceptions;
using WhoAmI.Domain.Tests.Shared.Base.Dummies;

namespace WhoAmI.Domain.Tests.Shared.Base;

public class EntityTests {
    [Fact]
    public void Entity_ShouldBeCreated_WhenIdIsValid() {
        var id = Guid.NewGuid();

        var entity = new DummyEntity(id);

        entity.Id.Should().Be(id);
    }

    [Fact]
    public void Entity_ShouldThrowDomainException_WhenIdIsEmpty() {
        var id = Guid.Empty;

        var act = () => new DummyEntity(id);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Entity_ShouldBeEqual_WhenIdsAreIdentical() {
        var id = Guid.NewGuid();

        var entity = new DummyEntity(id);
        var sameEntity = new DummyEntity(id);

        entity.Should().Be(sameEntity);
        entity.GetHashCode().Should().Be(sameEntity.GetHashCode());
        (entity == sameEntity).Should().BeTrue();
        (entity != sameEntity).Should().BeFalse();
    }

    [Fact]
    public void Entity_ShouldNotBeEqual_WhenIdsAreDifferent() {
        var entity = new DummyEntity(Guid.NewGuid());
        var otherEntity = new DummyEntity(Guid.NewGuid());

        entity.Should().NotBe(otherEntity);
        (entity != otherEntity).Should().BeTrue();
        (entity == otherEntity).Should().BeFalse();
    }
}

using Entities.Models.Base;

namespace Contracts.DataShaping;

public interface IDataShaper<T>
{
    IEnumerable<Entity> ShapeData(IEnumerable<T> entities, string fieldsString);

    Entity ShapeData(T entity, string fieldsString);
}

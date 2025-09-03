using Entities.Models.Base;

namespace Contracts.DataShaping;

public interface IDataShaper<T, TId>
{
    IEnumerable<ShapedEntity<TId>> ShapeData(IEnumerable<T> entities, string fieldsString);

    ShapedEntity<TId> ShapeData(T entity, string fieldsString);
}

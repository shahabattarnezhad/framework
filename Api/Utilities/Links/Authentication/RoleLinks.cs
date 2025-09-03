using Contracts.DataShaping;
using Entities.Models.Base;
using Microsoft.Net.Http.Headers;
using Contracts.Links.Authentication;
using Shared.DTOs.Authentication;
using Entities.LinkModels.Base;

namespace Api.Utilities.Links.Authentication;

public class RoleLinks : IRoleLinks
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IDataShaper<RoleDto, string> _dataShaper;


    public RoleLinks(LinkGenerator linkGenerator, IDataShaper<RoleDto, string> dataShaper)
    {
        _linkGenerator = linkGenerator;
        _dataShaper = dataShaper;
    }


    public LinkResponse TryGenerateLinks(IEnumerable<RoleDto> entitiesDto, string fields, HttpContext httpContext)
    {
        var shapedEntities = ShapeData(entitiesDto, fields);

        if (ShouldGenerateLinks(httpContext))
            return ReturnLinkdedEntities(entitiesDto, fields, httpContext, shapedEntities);

        return ReturnShapedEntities(shapedEntities);
    }


    private List<Entity> ShapeData(IEnumerable<RoleDto> entitiesDto, string fields) =>
                        _dataShaper.ShapeData(entitiesDto, fields)
                                   .Select(entity => entity.Entity)
                                   .ToList();


    private bool ShouldGenerateLinks(HttpContext httpContext)
    {
        var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"]!;

        return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
    }


    private LinkResponse ReturnShapedEntities(List<Entity> shapedEntities) =>
        new LinkResponse { ShapedEntities = shapedEntities };


    private LinkResponse ReturnLinkdedEntities
        (IEnumerable<RoleDto> entitiesDto, string fields, HttpContext httpContext, List<Entity> shapedEntities)
    {
        var entityDtoList = entitiesDto.ToList();

        for (var index = 0; index < entityDtoList.Count(); index++)
        {
            var entityLinks = CreateLinksForEntity(httpContext, entityDtoList[index].Id!, fields);

            shapedEntities[index].Add("Links", entityLinks);
        }

        var entityCollection = new LinkCollectionWrapper<Entity>(shapedEntities);

        var linkedEntities = CreateLinksForEntities(httpContext, entityCollection);

        return new LinkResponse
        {
            HasLinks = true,
            LinkedEntities = linkedEntities
        };
    }


    private List<Link> CreateLinksForEntity(HttpContext httpContext, string id, string fields = "")
    {
        var links = new List<Link>
        {
            new
            Link(_linkGenerator.GetUriByAction(httpContext,"GetRole", values: new { id, fields })!, "self","GET"),
        };

        return links;
    }


    private LinkCollectionWrapper<Entity> CreateLinksForEntities(HttpContext httpContext, LinkCollectionWrapper<Entity> entitiesWrapper)
    {
        entitiesWrapper.Links.Add(new Link
            (_linkGenerator.GetUriByAction(httpContext, "GetRoles", values: new { })!, "self", "GET"));

        return entitiesWrapper;
    }
}

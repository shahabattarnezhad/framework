using Contracts.DataShaping;
using Contracts.Links.Sample;
using Entities.Models.Base;
using Microsoft.Net.Http.Headers;
using Entities.Models.LinkModels.Base;
using Shared.DTOs.Sample.Company;

namespace Api.Utilities.Links;

public class CompanyLinks : ICompanyLinks
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IDataShaper<CompanyDto> _dataShaper;


    public CompanyLinks(LinkGenerator linkGenerator,
                         IDataShaper<CompanyDto> dataShaper)
    {
        _linkGenerator = linkGenerator;
        _dataShaper = dataShaper;
    }


    public LinkResponse TryGenerateLinks(IEnumerable<CompanyDto> entitiesDto,
                                         string fields,
                                         HttpContext httpContext)
    {
        var shapedEntities = ShapeData(entitiesDto, fields);

        if (ShouldGenerateLinks(httpContext))
            return ReturnLinkdedEntities(entitiesDto,
                                          fields,
                                          httpContext,
                                          shapedEntities);

        return ReturnShapedEntities(shapedEntities);
    }


    private List<Entity> ShapeData(IEnumerable<CompanyDto> entitiesDto, string fields) =>
                        _dataShaper.ShapeData(entitiesDto,
                                           fields)
                                   .Select(entity => entity.Entity)
                                   .ToList();


    private bool ShouldGenerateLinks(HttpContext httpContext)
    {
        var mediaType = (MediaTypeHeaderValue)httpContext
                                         .Items["AcceptHeaderMediaType"]!;

        return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas",
            StringComparison.InvariantCultureIgnoreCase);
    }


    private LinkResponse ReturnShapedEntities(List<Entity> shapedEntities) =>
        new LinkResponse { ShapedEntities = shapedEntities };


    private LinkResponse ReturnLinkdedEntities(IEnumerable<CompanyDto> entitiesDto,
                                                string fields,
                                                HttpContext httpContext,
                                                List<Entity> shapedEntities)
    {
        var entityDtoList = entitiesDto.ToList();

        for (var index = 0; index < entityDtoList.Count(); index++)
        {
            var entityLinks = CreateLinksForEntity(httpContext,
                                                         entityDtoList[index].Id,
                                                         fields);

            shapedEntities[index].Add("Links", entityLinks);
        }

        var entityCollection =
            new LinkCollectionWrapper<Entity>(shapedEntities);

        var linkedEntities =
            CreateLinksForEntities(httpContext, entityCollection);

        return new LinkResponse
        {
            HasLinks = true,
            LinkedEntities = linkedEntities
        };
    }


    private List<Link> CreateLinksForEntity(HttpContext httpContext,
                                              Guid id,
                                              string fields = "")
    {
        var links = new List<Link>
        {
            new Link
                (
                _linkGenerator.GetUriByAction(httpContext,
                      "GetCompany",
                      values: new
                      {
                          id,
                          fields
                      })!,
                      "self",
                      "GET"
                ),
            new Link
                (
                     _linkGenerator.GetUriByAction(httpContext,
                     "DeleteCompany",
                     values: new
                     {
                         id
                     })!,
                     "delete_company",
                     "DELETE"
                ),
            new Link
                (
                    _linkGenerator.GetUriByAction(httpContext,
                    "UpdateCompany",
                    values: new
                    {
                        id
                    })!,
                    "update_company",
                    "PUT"
                ),
            new Link
                (
                    _linkGenerator.GetUriByAction(httpContext,
                    "PartiallyUpdateCompany",
                    values: new
                    {
                        id
                    })!,
                    "partially_update_company",
                    "PATCH"
                )
        };

        return links;
    }


    private LinkCollectionWrapper<Entity> CreateLinksForEntities(HttpContext httpContext,
                                LinkCollectionWrapper<Entity> entitiesWrapper)
    {
        entitiesWrapper.Links
                        .Add(new Link(_linkGenerator.GetUriByAction(httpContext,
                        "GetCompanies", values: new { })!,
                        "self", "GET"));

        return entitiesWrapper;
    }
}

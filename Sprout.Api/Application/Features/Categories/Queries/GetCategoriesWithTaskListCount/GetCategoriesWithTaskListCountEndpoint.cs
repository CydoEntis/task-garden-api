﻿using MediatR;
using Sprout.Api.Application.Shared.Models;

namespace Sprout.Api.Application.Features.Categories.Queries.GetCategoriesWithTaskListCount;

public static class GetCategoriesWithTaskListCountEndpoint
{
    public static void MapGetCategoriesWithTaskListCountEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/categories/details", async (
                [AsParameters] GetCategoriesWithTaskListCountQuery query,
                IMediator mediator) =>
            {
                var response = await mediator.Send(query);

                return Results.Ok(
                    ApiResponse<PagedResponse<GetCategoriesWithTaskListCountResponse>>.SuccessWithData(response));
            })
            .WithName("GetAllCategoriesWithTaskListCount")
            .WithTags("Categories")
            .RequireAuthorization()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK);
    }
}
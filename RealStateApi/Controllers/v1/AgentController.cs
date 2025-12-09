using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.Agent;
using RealState.Core.Application.DTOs.PropertyType;
using RealState.Core.Application.Features.Agent.Queries.GetAll;
using RealState.Core.Application.Features.Agent.Queries.GetById;
using RealState.Core.Application.Features.ImprovementType.Queries.GetAll;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealStateApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class AgentController : BaseApiController
    {
        [HttpGet]
        [Authorize(Roles = "Admin,Developer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]

        [SwaggerOperation(
            Summary = "Agent List",
            Description = "Listado de agentes sin filtros"
        )]

        public async Task<IActionResult> Get()
        {
            try
            {
                var agents = await Mediator.Send(new GetAllAgentQuery());
                if (agents is null || agents.Count == 0)
                {
                    return NoContent();
                }

                return Ok(agents);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Developer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]

        [SwaggerOperation(
            Summary = "Agent by Id",
            Description = "Obtener agent por Id"
        )]

        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var propertyType = await Mediator.Send(new GetByIdAgentQuery() { Id = id});
                if (propertyType == null)
                {
                    return NoContent();
                }

                return Ok(propertyType);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("Properties/idProperty")]
        [Authorize(Roles = "Admin,Developer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]

        [SwaggerOperation(
            Summary = "Properties Agent List",
            Description = "Listado de propiedades de un agente por id"
        )]

        public async Task<IActionResult> GetPropertiesByID(string idProperty)
        {
            try
            {
                var propertyType = await Mediator.Send(new GetByIdAgentQuery() { Id = idProperty });
                if (propertyType == null)
                {
                    return NoContent();
                }

                return Ok(propertyType);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



    }
}

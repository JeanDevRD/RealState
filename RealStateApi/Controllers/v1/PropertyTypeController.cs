using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.PropertyType;
using RealState.Core.Application.Features.PropertyType.Commands.CreatePropertyType;
using RealState.Core.Application.Features.PropertyType.Commands.UpdatePropertyType;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealStateApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class PropertyTypeController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyTypeDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Listar tipos de propiedad",
            Description = "Devuelve todos los tipos de propiedad sin filtros."
        )]
        public async Task<IActionResult> Get()
        {
            try
            {
                var propertyTypes = await Mediator.Send(new GetAllPropertyTypeQuery());
                if (propertyTypes is null || propertyTypes.Count == 0)
                {
                    return NoContent();
                }

                return Ok(propertyTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyTypeDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Obtener tipo de propiedad por ID",
            Description = "Devuelve un tipo de propiedad según su identificador."
        )]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var propertyType = await Mediator.Send(new GetByIdPropertyTypeQuery() { Id = id });
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Crear tipo de propiedad",
            Description = "Permite crear un nuevo tipo de propiedad."
        )]
        public async Task<IActionResult> Create([FromBody] CreatePropertyTypeCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = await Mediator.Send(command);
                if (result == 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Creation failed");
                }

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Actualizar tipo de propiedad",
            Description = "Actualiza un tipo de propiedad existente."
        )]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePropertyTypeCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                if (id != command.Id)
                {
                    return BadRequest("El ID de la URL no coincide con el del cuerpo.");
                }

                await Mediator.Send(command);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Eliminar tipo de propiedad",
            Description = "Elimina un tipo de propiedad existente por ID."
        )]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await Mediator.Send(new DeletePropertyTypeCommand() { Id = id });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

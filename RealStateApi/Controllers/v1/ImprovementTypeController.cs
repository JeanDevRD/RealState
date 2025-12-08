using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.ImprovementType;
using RealState.Core.Application.Features.ImprovementType.Commands.CreateImprovementType;
using RealState.Core.Application.Features.ImprovementType.Commands.DeleteImprovementType;
using RealState.Core.Application.Features.ImprovementType.Commands.UpdateImprovementType;
using RealState.Core.Application.Features.ImprovementType.Queries.GetAll;
using RealState.Core.Application.Features.ImprovementType.Queries.GetById;

namespace RealStateApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ImprovementTypeController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImprovementTypeDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var saleTypes = await Mediator.Send(new GetAllImprovementTypeQuery());
                if (saleTypes is null || saleTypes.Count == 0)
                {
                    return NoContent();
                }

                return Ok(saleTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImprovementTypeDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var saleTypes = await Mediator.Send(new GetByIdImproventTypeQuery() { Id = id});
                if (saleTypes == null)
                {
                    return NoContent();
                }

                return Ok(saleTypes);
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
        public async Task<IActionResult> Create([FromBody]CreateImprovementTypeCommand command)
        {
            try
            {
                if (!ModelState.IsValid) 
                {
                    return BadRequest();
                }

                var result = await Mediator.Send(command);
                if(result == 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Creation failed");
                }

                return Created();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateImprovementTypeCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                if(id != command.Id)
                {
                    return BadRequest();
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                await Mediator.Send(new DeleteImprovementTypeCommand() { Id  = id});

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}

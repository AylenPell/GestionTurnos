using Application.Services;
using Contracts.Study.Requests;
using Contracts.Study.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudyController : ControllerBase
{
    private readonly IStudyService _studyService;

    public StudyController(IStudyService studyService)
    {
        _studyService = studyService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var studies = _studyService.GetAll();
        return Ok(studies);
    }

    [HttpGet("{id}")]
    public ActionResult<StudyResponse?> GetById([FromRoute] int id)
    {
        var study = _studyService.GetById(id);
        if (study == null)
        {
            return NotFound();
        }
        return Ok(study);
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPost]
    public ActionResult Create([FromBody] CreateStudyRequest study)
    {
        string message;
        int createdId;

        bool created = _studyService.Create(study, out message, out createdId);

        if (!created)
        {
            return Conflict(new { message });
        }

        return CreatedAtAction(nameof(GetById), new { id = createdId }, new { id = createdId, message });
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPut("{id}")]
    public ActionResult Update([FromRoute] int id, [FromBody] UpdateStudyRequest study)
    {
        string message;

        var isUpdated = _studyService.Update(id, study, out message);

        if (!isUpdated)
            return Conflict(new { message });

        return Ok(new { message });
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{id}")]
    public ActionResult Delete([FromRoute] int id)
    {
        string message;

        var isDeleted = _studyService.Delete(id, out message);
        if (!isDeleted)
            return Conflict(new { message });

        return Ok(new { message });
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPatch("reactivate/{id}")]
    public ActionResult Reactivate([FromRoute] int id)
    {
        string message;
        var isReactivated = _studyService.Reactivate(id, out message);
        
        if (!isReactivated)
            return Conflict(new { message });

        return Ok(new { message });
    }
}

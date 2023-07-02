using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using OutputCacheDemoESP.Entidades;

namespace OutputCacheDemoESP.Controllers
{
[ApiController]
[Route("api/personas")]
public class PersonasController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IOutputCacheStore cacheStore;

    public PersonasController(ApplicationDbContext context, IOutputCacheStore cacheStore)
    {
        this.context = context;
        this.cacheStore = cacheStore;
    }

    [HttpGet]
    [OutputCache(PolicyName = "personas")]
    public async Task<ActionResult<List<Persona>>> Get()
    {
        await Task.Delay(2000);
        return await context.Personas.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<List<Persona>>> Post(Persona persona)
    {
        context.Add(persona);
        await context.SaveChangesAsync();
        await cacheStore.EvictByTagAsync("personas", default);
        return Ok();
    }
}
}

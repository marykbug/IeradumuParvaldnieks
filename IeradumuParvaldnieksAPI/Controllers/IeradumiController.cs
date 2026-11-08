using IeradumuParvaldnieksApplication;
using IeradumuParvaldnieksDataModels;
using IeradumuParvaldnieksKlases;
using Microsoft.AspNetCore.Mvc;

namespace IeradumuParvaldnieksAPI.Controllers
{
    [ApiController]
    [Route("api/ieradumi")]
    public class IeradumiController : ControllerBase
    {
        private readonly IeradumuService service;

        public IeradumiController(IeradumuService service)
        {
            this.service = service;
        }

        // GET api/ieradumi
        [HttpGet]
        public async Task<ActionResult<List<IeradumsDataModel>>> IegutVisus()
        {
            List<Ieradums> ieradumi =
                await service.IegutVisusAsync();

            List<IeradumsDataModel> rezultats =
                new List<IeradumsDataModel>();

            foreach (Ieradums ieradums in ieradumi)
            {
                rezultats.Add(ParveidotParDataModel(ieradums));
            }

            return Ok(rezultats);
        }


        // GET api/ieradumi/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<IeradumsDataModel>> IegutPecId(Guid id)
        {
            try
            {
                Ieradums ieradums =
                    await service.IegutPecIdAsync(id);

                return Ok(
                    ParveidotParDataModel(ieradums)
                );
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        // POST api/ieradumi
        [HttpPost]
        public async Task<ActionResult<IeradumsDataModel>> Izveidot(
            IzveidotIeradumsDataModel modelis)
        {
            try
            {
                IeradumuAtkartosana atkartosana =
                    ParveidotAtkartosanu(modelis.Atkartosana);

                List<NedelasDienas> dienas =
                    ParveidotNedelasDienas(modelis.NedelasDienas);

                Ieradums ieradums =
                    await service.IzveidotAsync(
                        modelis.LietotajsID,
                        modelis.Nosaukums,
                        modelis.Apraksts,
                        atkartosana,
                        dienas
                    );

                IeradumsDataModel rezultats =
                    ParveidotParDataModel(ieradums);

                return CreatedAtAction(
                    nameof(IegutPecId),
                    new { id = ieradums.Id },
                    rezultats
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // PUT api/ieradumi/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Labot(
            Guid id,
            LabotIeradumuDataModel modelis)
        {
            try
            {
                IeradumuAtkartosana atkartosana =
                    ParveidotAtkartosanu(modelis.Atkartosana);

                List<NedelasDienas> dienas =
                    ParveidotNedelasDienas(modelis.NedelasDienas);

                await service.LabotAsync(
                    id,
                    modelis.Nosaukums,
                    modelis.Apraksts,
                    atkartosana,
                    dienas
                );

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // PUT api/ieradumi/{id}/arhivet
        [HttpPut("{id}/arhivet")]
        public async Task<IActionResult> Arhivet(Guid id)
        {
            try
            {
                await service.ArhivetAsync(id);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        // POST api/ieradumi/{id}/izpildes
        [HttpPost("{id}/izpildes")]
        public async Task<IActionResult> AtzimetKaIzpilditu(
            Guid id,
            AtzimetIzpildiDataModel modelis)
        {
            try
            {
                await service.AtzimetKaIzpildituAsync(
                    id,
                    modelis.Datums
                );

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // GET api/ieradumi/{id}/izpildes
        [HttpGet("{id}/izpildes")]
        public async Task<ActionResult<List<IeradumaIzpildeDataModel>>>
            IegutIzpildes(Guid id)
        {
            try
            {
                IReadOnlyList<IeradumaIzpilde> izpildes =
                    await service.IegutIzpildesAsync(id);

                List<IeradumaIzpildeDataModel> rezultats =
                    new List<IeradumaIzpildeDataModel>();

                foreach (IeradumaIzpilde izpilde in izpildes)
                {
                    rezultats.Add(
                        new IeradumaIzpildeDataModel
                        {
                            Id = izpilde.Id,
                            IeradumsID = id,
                            Datums = izpilde.Datums
                        }
                    );
                }

                return Ok(rezultats);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        private IeradumsDataModel ParveidotParDataModel(
            Ieradums ieradums)
        {
            IeradumsDataModel modelis =
                new IeradumsDataModel
                {
                    Id = ieradums.Id,
                    LietotajsID = ieradums.LietotajsID,
                    Nosaukums = ieradums.Nosaukums,
                    Apraksts = ieradums.Apraksts,
                    Status = ieradums.Status.ToString(),
                    Atkartosana = ieradums.Atkartosana.ToString(),
                    Izveidots = ieradums.Izveidots,
                    Labots = ieradums.Labots,
                    IrIzpilditsSodien =
                        ieradums.IrIzpilditsSodien()
                };

            foreach (NedelasDienas diena in ieradums.NedelasDienas)
            {
                modelis.NedelasDienas.Add(
                    diena.ToString()
                );
            }

            return modelis;
        }


        private IeradumuAtkartosana ParveidotAtkartosanu(
            string atkartosana)
        {
            if (Enum.TryParse(
                atkartosana,
                true,
                out IeradumuAtkartosana rezultats))
            {
                return rezultats;
            }

            throw new ArgumentException(
                "Nederīgs atkārtošanās veids."
            );
        }


        private List<NedelasDienas> ParveidotNedelasDienas(
            IEnumerable<string> dienas)
        {
            List<NedelasDienas> rezultats =
                new List<NedelasDienas>();

            foreach (string diena in dienas)
            {
                if (!Enum.TryParse(
                    diena,
                    true,
                    out NedelasDienas nedelasDiena))
                {
                    throw new ArgumentException(
                        "Nederīga nedēļas diena."
                    );
                }

                rezultats.Add(nedelasDiena);
            }

            return rezultats;
        }
        // PUT api/ieradumi/{id}/atjaunot
        [HttpPut("{id}/atjaunot")]
        public async Task<IActionResult> AtjaunotNoArhiva(Guid id)
        {
            try
            {
                await service.AtjaunotNoArhivaAsync(id);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
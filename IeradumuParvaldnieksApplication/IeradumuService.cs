using IeradumuParvaldnieksKlases;
using System;
using System.Collections.Generic;
using System.Text;
namespace IeradumuParvaldnieksApplication
{
    public class IeradumuService
    {
        private readonly IIeradumuRepository repository;
        public IeradumuService(IIeradumuRepository repository)
        {
            this.repository = repository;
        }
        // Iegūst visus ieradumus.
        public async Task<List<Ieradums>> IegutVisusAsync()
        {
            return await repository.IegutVisusAsync();
        }
        // Iegūst vienu ieradumu pēc identifikatora.
        public async Task<Ieradums> IegutPecIdAsync(Guid id)
        {
            Ieradums? ieradums = await repository.IegutPecIdAsync(id);
            if (ieradums == null)
            {
                throw new KeyNotFoundException(
                    "Ieradums nav atrasts."
                );
            }
            return ieradums;
        }
        // Izveido jaunu ieradumu.
        public async Task<Ieradums> IzveidotAsync(
            string lietotajsID,
            string nosaukums,
            string apraksts,
            IeradumuAtkartosana atkartosana,
            IEnumerable<NedelasDienas> nedelasDienas)
        {
            Ieradums jaunsIeradums = new Ieradums(
                Guid.NewGuid(),
                nosaukums,
                lietotajsID,
                apraksts,
                atkartosana,
                nedelasDienas
            );
            await repository.PievienotAsync(jaunsIeradums);
            return jaunsIeradums;
        }
        // Labo esošu ieradumu.
        public async Task LabotAsync(
            Guid id,
            string nosaukums,
            string apraksts,
            IeradumuAtkartosana atkartosana,
            IEnumerable<NedelasDienas> nedelasDienas)
        {
            Ieradums ieradums =
                await IegutPecIdAsync(id);
            ieradums.Labot(
                nosaukums,
                apraksts,
                atkartosana,
                nedelasDienas
            );
            await repository.SaglabatAsync(ieradums);
        }
        // Arhivē ieradumu.
        public async Task ArhivetAsync(Guid id)
        {
            Ieradums ieradums = await IegutPecIdAsync(id);
            ieradums.Arhivet();
            await repository.SaglabatAsync(ieradums);
        }
        // Atzīmē ieradumu kā izpildītu konkrētajā datumā.
        public async Task AtzimetKaIzpildituAsync(
            Guid id,
            DateOnly datums)
        {
            Ieradums ieradums = await IegutPecIdAsync(id);
            ieradums.AtzimetKaIzpilditu(datums);
            await repository.SaglabatAsync(ieradums);
        }
        // Iegūst ieraduma izpildes ierakstus.
        public async Task<IReadOnlyList<IeradumaIzpilde>>
            IegutIzpildesAsync(Guid id)
        {
            Ieradums ieradums = await IegutPecIdAsync(id);
            return ieradums.Izpildes;
        }
        public async Task AtjaunotNoArhivaAsync(Guid id)
        {
            Ieradums ieradums =
                await IegutPecIdAsync(id);

            ieradums.AtjaunotNoArhiva();

            await repository.SaglabatAsync(ieradums);
        }
    }
}

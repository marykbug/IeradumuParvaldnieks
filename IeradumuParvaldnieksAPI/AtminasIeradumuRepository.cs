using IeradumuParvaldnieksApplication;
using IeradumuParvaldnieksKlases;
using IeradumuParvaldnieksTestData;

namespace IeradumuParvaldnieksAPI
{
    public class AtminasIeradumuRepository : IIeradumuRepository
    {
        private readonly List<Ieradums> ieradumi;
        public AtminasIeradumuRepository()
        {
            // API palaišanas brīdī izmanto 1. mājas darba testa datus.
            ieradumi = IeradumsTestData.IzveidotIeradumus();
        }
        public Task<List<Ieradums>> IegutVisusAsync()
        {
            return Task.FromResult(ieradumi);
        }
        public Task<Ieradums?> IegutPecIdAsync(Guid id)
        {
            foreach (Ieradums ieradums in ieradumi)
            {
                if (ieradums.Id == id)
                {
                    return Task.FromResult<Ieradums?>(ieradums);
                }
            }
            return Task.FromResult<Ieradums?>(null);
        }
        public Task PievienotAsync(Ieradums ieradums)
        {
            ieradumi.Add(ieradums);

            return Task.CompletedTask;
        }
        public Task SaglabatAsync(Ieradums ieradums)
        {
            // Ieradums jau atrodas sarakstā kā objekts,
            // tāpēc izmaiņas saglabājas atmiņā.
            return Task.CompletedTask;
        }
    }
}

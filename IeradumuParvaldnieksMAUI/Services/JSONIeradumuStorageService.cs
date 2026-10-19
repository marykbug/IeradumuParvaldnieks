using System.Text.Json;
using IeradumuParvaldnieksKlases;
using IeradumuParvaldnieksMAUI.Models;

namespace IeradumuParvaldnieksMAUI.Services
{
    public class JSONIeradumuStorageService : IIeradumuStorageService
    {
        private readonly string failaCels;
        public JSONIeradumuStorageService()
        {
            failaCels = Path.Combine(
                FileSystem.AppDataDirectory,
                "ieradumi.json"
            );
        }
        public async Task<List<Ieradums>> IeladetAsync()
        {
            // Ja fails vēl neeksistē, nav ko ielādēt.
            if (!File.Exists(failaCels))
            {
                return new List<Ieradums>();
            }

            string json = await File.ReadAllTextAsync(failaCels);

            // Ja fails ir tukšs, atgriež tukšu sarakstu.
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<Ieradums>();
            }

            List<IeradumsLocalModel>? saglabatieIeradumi =
                JsonSerializer.Deserialize<List<IeradumsLocalModel>>(json);

            if (saglabatieIeradumi == null)
            {
                return new List<Ieradums>();
            }

            List<Ieradums> ieradumi = new List<Ieradums>();
            foreach (IeradumsLocalModel modelis in saglabatieIeradumi)
            {
                Ieradums ieradums = new Ieradums(
                    modelis.Id,
                    modelis.Nosaukums,
                    modelis.LietotajsID,
                    modelis.Apraksts,
                    modelis.Atkartosana,
                    modelis.Status,
                    modelis.Izveidots,
                    modelis.Labots,
                    modelis.IzpildesDatumi
                );
                ieradumi.Add(ieradums);
            }
            return ieradumi;
        }
        public async Task SaglabatAsync(IEnumerable<Ieradums> ieradumi)
        {
            List<IeradumsLocalModel> saglabajamieIeradumi =
                new List<IeradumsLocalModel>();

            foreach (Ieradums ieradums in ieradumi)
            {
                IeradumsLocalModel modelis = new IeradumsLocalModel
                {
                    Id = ieradums.Id,
                    LietotajsID = ieradums.LietotajsID,
                    Nosaukums = ieradums.Nosaukums,
                    Apraksts = ieradums.Apraksts,
                    Status = ieradums.Status,
                    Atkartosana = ieradums.Atkartosana,
                    Izveidots = ieradums.Izveidots,
                    Labots = ieradums.Labots
                };

                foreach (IeradumaIzpilde izpilde in ieradums.Izpildes)
                {
                    modelis.IzpildesDatumi.Add(izpilde.Datums);
                }

                saglabajamieIeradumi.Add(modelis);
            }

            JsonSerializerOptions opcijas = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(
                saglabajamieIeradumi,
                opcijas
            );

            await File.WriteAllTextAsync(failaCels, json);
        }
    }
}

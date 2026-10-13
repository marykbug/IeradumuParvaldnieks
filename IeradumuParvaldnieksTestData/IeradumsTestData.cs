using IeradumuParvaldnieksKlases;

namespace IeradumuParvaldnieksTestData
{
    public class IeradumsTestData
    {
        public static List<Ieradums> IzveidotIeradumus()
        {
            List<Ieradums> ieradumi = new List<Ieradums>();

            string lietotajs1 = "demo-user-1";
            string lietotajs2 = "demo-user-2";


            Ieradums ieradums1 = new Ieradums(
                Guid.NewGuid(),
                "Dzert ūdeni",
                lietotajs1,
                "Izdzert pietiekami daudz ūdens dienas laikā.",
                IeradumuAtkartosana.Diena
            );

            ieradums1.AtzimetKaIzpilditu(
                DateOnly.FromDateTime(DateTime.Today)
            );

            ieradums1.AtzimetKaIzpilditu(
                DateOnly.FromDateTime(DateTime.Today.AddDays(-1))
            );


            Ieradums ieradums2 = new Ieradums(
                Guid.NewGuid(),
                "Lasīt grāmatu",
                lietotajs1,
                "Lasīt vismaz 20 minūtes.",
                IeradumuAtkartosana.Diena
            );

            ieradums2.AtzimetKaIzpilditu(
                DateOnly.FromDateTime(DateTime.Today.AddDays(-2))
            );


            Ieradums ieradums3 = new Ieradums(
                Guid.NewGuid(),
                "Sports",
                lietotajs2,
                "Veikt fiziskās aktivitātes sporta zālē.",
                IeradumuAtkartosana.Nedela
            );

            ieradums3.AtzimetKaIzpilditu(
                DateOnly.FromDateTime(DateTime.Today)
            );


            Ieradums ieradums4 = new Ieradums(
                Guid.NewGuid(),
                "Meditācija",
                lietotajs2,
                "Meditēt 10 minūtes.",
                IeradumuAtkartosana.Diena
            );

            ieradums4.AtzimetKaIzpilditu(
                DateOnly.FromDateTime(DateTime.Today.AddDays(-5))
            );

            ieradums4.Arhivet();


            ieradumi.Add(ieradums1);
            ieradumi.Add(ieradums2);
            ieradumi.Add(ieradums3);
            ieradumi.Add(ieradums4);

            return ieradumi;
        }
    }
}

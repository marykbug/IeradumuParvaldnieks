using IeradumuParvaldnieksMAUI.Views;
namespace IeradumuParvaldnieksMAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(
                nameof(IeradumsDetalas),
                typeof(IeradumsDetalas)
            );
        }
    }
}

namespace MAUI.Hybrid.Integration.Example
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage()) { Title = "MAUI.Hybrid.Integration.Example" };
        }
    }
}

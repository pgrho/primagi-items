using Microsoft.Web.WebView2.Core;
using Shipwreck.PrimagiItems.Browser.ViewModels;

namespace Shipwreck.PrimagiItems.Browser.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void webView2_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
    {
        if (webView2.CoreWebView2 is CoreWebView2 cwv && DataContext is MainWindowViewModel vm)
        {
            vm.DocumentTitle = cwv.DocumentTitle;
            cwv.DocumentTitleChanged += (s, e) => vm.DocumentTitle = cwv.DocumentTitle;
        }
    }
}
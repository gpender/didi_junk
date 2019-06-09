namespace DidiWebSocketTest.ViewModels
{
    public class MainVM : BaseVM
    {
        public ScopeVM ScopeVM { get; private set; }
        public WsScopeProtocolVM ScopeProtocolVM { get; private set; }
        public WsTestProtocolVM TestProtocolVM { get; private set; }
        public MainVM(WsScopeProtocolVM scopeProtocolVM, WsTestProtocolVM testVM, ScopeVM scopeVM)
        {
            ScopeVM = scopeVM;
            ScopeProtocolVM = scopeProtocolVM;
            TestProtocolVM = testVM;
        }
    }
}

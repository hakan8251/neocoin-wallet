using NeoModules.Core.KeyPair;
using NeoModules.JsonRpc.Client;
using NeoModules.NEP6;
using NeoModules.Rest.Services;
using NeoModules.RPC;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Agreement.Srp;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace neotest
{
    class Program
    {
        static void Main(string[] args)
        {
            var rpcClient = new RpcClient(new Uri("http://neo.org:10332"));
            var NeoApiServices = new NeoApiService(rpcClient);
            var walletManager = new WalletManager(new NeoScanRestService(NeoScanNet.MainNet), rpcClient);
            string accresult = "";

            try
            {
                var acc = walletManager.GetDefaultAccount();
                accresult = JsonConvert.SerializeObject(acc, Formatting.Indented);
                Console.WriteLine(accresult);
                Process.Start("https://neotracker.io/address/" + acc.Address);
            }
            catch
            {
                var cacc = walletManager.CreateAccount("metinyakarNet", "wordpress").Result;
                cacc.IsDefault = true;
                walletManager.AddAccount(cacc);
                //walletManager.DeleteAccount(cacc.Address.ToAddress());
                var acc = walletManager.GetAccount(cacc.Address);
                accresult = JsonConvert.SerializeObject(acc, Formatting.Indented);
                File.WriteAllText("neoAccResult.txt", accresult);
                Console.WriteLine(accresult); 
                string trackerurl = "https://neotracker.io/address/" + acc.Address.ToAddress();
                Process.Start(trackerurl);
            }
            
            Console.Read();
        }
    }
}

using Service.Core.Client;
using Service.Core.Infrastructure.Communication.Structures;
using System;
using System.Reflection;
using Xunit;

namespace Service.Test
{
    public class TlvServiceExecutorTest
    {
        private readonly DispatchResult encryptInitDispatchResult = new DispatchResult(ServiceActionCode.EncryptInit, new byte[] { 
                /*first param*/
                    /*expected uint*/ 0x00, 0x00, 0x00, 0x01,  
                    
                /*second param*/
                    /*type*/          0x00, 0x00, 0x00, 0x01, 
                    /*length*/        0x00, 0x00, 0x00, 0x01,
                    /*value*/         0x05
            }, false);

        [Fact]
        public void ModelBinderTest()
        {
            //assign
            DispatchResult dispatchResult = encryptInitDispatchResult;
            MethodInfo method = typeof(TlvServiceExecutor).GetMethod(nameof(ServiceActionCode.EncryptInit));

            TlvServiceExecutorModelBinder binder = new TlvServiceExecutorModelBinder();

            //action
            var objects = binder.GetMethodParameters(method, dispatchResult);

            //assert
            Assert.Equal(2, objects.Length);
        }

        [Fact]
        public void EncryptInitTest()
        {
            
        }
    }
}

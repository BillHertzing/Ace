using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AceAgent
{

    class Tcp
    {
        public static int defaultMaxResponseBufferSize = 1024;
        public static async Task<Byte[]> FetchAsync(string host, int port, string tcpRequestMessage, Byte[] responseBuffer)
        {
            return await FetchAsync(Policy.NoOp(), host, port, tcpRequestMessage, defaultMaxResponseBufferSize, CancellationToken.None); }
        public static async Task<Byte[]> FetchAsync(Policy policy, string host, int port, string tcpRequestMessage)
        {
            return await FetchAsync(policy, host, port, tcpRequestMessage, defaultMaxResponseBufferSize, CancellationToken.None);
        }
        public static async Task<Byte[]> FetchAsync(string host, int port, string tcpRequestMessage, CancellationToken cancellationToken)
        {
            return await FetchAsync(Policy.NoOp(), host, port, tcpRequestMessage, defaultMaxResponseBufferSize, cancellationToken);
        }
        public static async Task<Byte[]> FetchAsync(Policy policy, string host, int port, string tcpRequestMessage,  int maxResponseBufferSize, CancellationToken cancellationToken)
        {
            return await policy.ExecuteAsync(async () => {
                // let every exception in this method bubble up
                var data = System.Text.Encoding.ASCII.GetBytes(tcpRequestMessage);
                var socket = new System.Net.Sockets.TcpClient(host, port);
                var stream = socket.GetStream();
                // write the request async   
                await stream.WriteAsync(data, 0, data.Length, cancellationToken);
                Byte[] buffer = new Byte[maxResponseBufferSize];
                // read the response async   
                int numBytesRead = await stream.ReadAsync(buffer, 0, maxResponseBufferSize, cancellationToken);
                return buffer;
            });
        }
    }
}

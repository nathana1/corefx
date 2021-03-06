// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Net.NetworkInformation.Tests
{
    public class ConnectionsParsingTests
    {
        [Fact]
        public static void NumSocketConnectionsParsing()
        {
            FileUtil.NormalizeLineEndings("sockstat", "sockstat_normalized");
            FileUtil.NormalizeLineEndings("sockstat6", "sockstat6_normalized");

            int numTcp = StringParsingHelpers.ParseNumSocketConnections("sockstat_normalized", "TCP");
            Assert.Equal(4, numTcp);

            int numTcp6 = StringParsingHelpers.ParseNumSocketConnections("sockstat6_normalized", "TCP6");
            Assert.Equal(6, numTcp6);

            int numUdp = StringParsingHelpers.ParseNumSocketConnections("sockstat_normalized", "UDP");
            Assert.Equal(12, numUdp);

            int numUdp6 = StringParsingHelpers.ParseNumSocketConnections("sockstat6_normalized", "UDP6");
            Assert.Equal(3, numUdp6);
        }

        [Fact]
        public static void ActiveTcpConnectionsParsing()
        {
            FileUtil.NormalizeLineEndings("tcp", "tcp_normalized0");
            FileUtil.NormalizeLineEndings("tcp6", "tcp6_normalized0");

            TcpConnectionInformation[] infos = StringParsingHelpers.ParseActiveTcpConnectionsFromFiles("tcp_normalized0", "tcp6_normalized0");
            Assert.Equal(11, infos.Length);
            ValidateInfo(infos[0], new IPEndPoint(0xFFFFFF01L, 0x01BD), new IPEndPoint(0L, 0), TcpState.Established);
            ValidateInfo(infos[1], new IPEndPoint(0x12345678L, 0x008B), new IPEndPoint(0L, 0), TcpState.SynSent);
            ValidateInfo(infos[2], new IPEndPoint(0x0101007FL, 0x0035), new IPEndPoint(0L, 0), TcpState.SynReceived);
            ValidateInfo(infos[3], new IPEndPoint(0x0100007FL, 0x0277), new IPEndPoint(0L, 0), TcpState.FinWait1);
            ValidateInfo(infos[4], new IPEndPoint(0x0100007FL, 0x0277), new IPEndPoint(0x00000001L, 0), TcpState.SynReceived);

            ValidateInfo(
                infos[5],
                new IPEndPoint(StringParsingHelpers.ParseHexIPAddress("00000000000000000000000000000000"), 0x01BD),
                new IPEndPoint(StringParsingHelpers.ParseHexIPAddress("00000000000000000000000000000000"), 0x0000),
                TcpState.FinWait2);

            ValidateInfo(
                infos[6],
                new IPEndPoint(StringParsingHelpers.ParseHexIPAddress("00000000000000000000000000000000"), 0x008B),
                new IPEndPoint(StringParsingHelpers.ParseHexIPAddress("00000000000000000000000000000000"), 0x0000),
                TcpState.TimeWait);

            ValidateInfo(
                infos[7],
                new IPEndPoint(StringParsingHelpers.ParseHexIPAddress("00000000000000000000000001000000"), 0x0277),
                new IPEndPoint(StringParsingHelpers.ParseHexIPAddress("00000000000000000000000000000000"), 0x0000),
                TcpState.Closing);

            ValidateInfo(
                infos[8],
                new IPEndPoint(StringParsingHelpers.ParseHexIPAddress("00000000000000000000000001000000"), 0xA696),
                new IPEndPoint(StringParsingHelpers.ParseHexIPAddress("00000000000000000000000001000000"), 0x0277),
                TcpState.CloseWait);

            ValidateInfo(
                infos[9],
                new IPEndPoint(StringParsingHelpers.ParseHexIPAddress("00000000000000000000000001000000"), 0xA69B),
                new IPEndPoint(StringParsingHelpers.ParseHexIPAddress("00000000000000000000000001000000"), 0x0277),
                TcpState.LastAck);

            ValidateInfo(
                infos[10],
                new IPEndPoint(StringParsingHelpers.ParseHexIPAddress("00000000000000000000000001000000"), 0xA697),
                new IPEndPoint(StringParsingHelpers.ParseHexIPAddress("00000000000000000000000001000000"), 0x0277),
                TcpState.Listen);
        }

        [Fact]
        public static void TcpListenersParsing()
        {
            FileUtil.NormalizeLineEndings("tcp", "tcp_normalized1");
            FileUtil.NormalizeLineEndings("tcp6", "tcp6_normalized1");

            IPEndPoint[] listeners = StringParsingHelpers.ParseActiveTcpListenersFromFiles("tcp_normalized1", "tcp6_normalized1");
            Assert.Equal(11, listeners.Length);

            Assert.Equal(new IPEndPoint(0xFFFFFF01, 0x01Bd), listeners[0]);
            Assert.Equal(new IPEndPoint(0x12345678, 0x008B), listeners[1]);
            Assert.Equal(new IPEndPoint(0x0101007F, 0x0035), listeners[2]);
            Assert.Equal(new IPEndPoint(0x0100007F, 0x0277), listeners[3]);
            Assert.Equal(new IPEndPoint(0x0100007F, 0x0277), listeners[4]);

            Assert.Equal(new IPEndPoint(StringParsingHelpers.ParseHexIPAddress("00000000000000000000000000000000"), 0x01BD), listeners[5]);
            Assert.Equal(new IPEndPoint(StringParsingHelpers.ParseHexIPAddress("00000000000000000000000000000000"), 0x008B), listeners[6]);
            Assert.Equal(new IPEndPoint(StringParsingHelpers.ParseHexIPAddress("00000000000000000000000001000000"), 0x0277), listeners[7]);
            Assert.Equal(new IPEndPoint(StringParsingHelpers.ParseHexIPAddress("00000000000000000000000001000000"), 0xA696), listeners[8]);
            Assert.Equal(new IPEndPoint(StringParsingHelpers.ParseHexIPAddress("00000000000000000000000001000000"), 0xA69B), listeners[9]);
            Assert.Equal(new IPEndPoint(StringParsingHelpers.ParseHexIPAddress("00000000000000000000000001000000"), 0xA697), listeners[10]);
        }

        [Fact]
        public static void UdpListenersParsing()
        {
            FileUtil.NormalizeLineEndings("udp", "udp_normalized0");
            FileUtil.NormalizeLineEndings("udp6", "udp6_normalized0");

            IPEndPoint[] listeners = StringParsingHelpers.ParseActiveUdpListenersFromFiles("udp_normalized0", "udp6_normalized0");
            Assert.Equal(15, listeners.Length);

            Assert.Equal(listeners[0], new IPEndPoint(0x00000000, 0x8E15));
            Assert.Equal(listeners[1], new IPEndPoint(0x00000000, 0x14E9));
            Assert.Equal(listeners[2], new IPEndPoint(0x00000000, 0xB50F));
            Assert.Equal(listeners[3], new IPEndPoint(0x0101007F, 0x0035));
            Assert.Equal(listeners[4], new IPEndPoint(0x00000000, 0x0044));
            Assert.Equal(listeners[5], new IPEndPoint(0xFF83690A, 0x0089));
            Assert.Equal(listeners[6], new IPEndPoint(0x3B80690A, 0x0089));
            Assert.Equal(listeners[7], new IPEndPoint(0x00000000, 0x0089));
            Assert.Equal(listeners[8], new IPEndPoint(0xFF83690A, 0x008A));
            Assert.Equal(listeners[9], new IPEndPoint(0x3B80690A, 0x008A));
            Assert.Equal(listeners[10], new IPEndPoint(0x00000000, 0x008A));
            Assert.Equal(listeners[11], new IPEndPoint(0x00000000, 0x0277));

            Assert.Equal(listeners[12], new IPEndPoint(StringParsingHelpers.ParseHexIPAddress("00000000000000000000000000000000"), 0x14E9));
            Assert.Equal(listeners[13], new IPEndPoint(StringParsingHelpers.ParseHexIPAddress("00000000000000000000000000000000"), 0x96D3));
            Assert.Equal(listeners[14], new IPEndPoint(StringParsingHelpers.ParseHexIPAddress("00000000000000000000000000000000"), 0x8B58));
        }

        private static void ValidateInfo(TcpConnectionInformation tcpConnectionInformation, IPEndPoint localEP, IPEndPoint remoteEP, TcpState state)
        {
            Assert.Equal(localEP, tcpConnectionInformation.LocalEndPoint);
            Assert.Equal(remoteEP, tcpConnectionInformation.RemoteEndPoint);
            Assert.Equal(state, tcpConnectionInformation.State);
        }
    }
}
